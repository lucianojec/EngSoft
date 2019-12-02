using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace EFMetrica
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {

                Console.WriteLine("Iniciando Migração de dados");

                var parametros = new DynamicParameters();
                var parametrosKaloi = new DynamicParameters();

                Conexao.ConectaEng();
                IEnumerable<EngProject> engprojects = AcessaDadosProject.DadosProjects();
                IEnumerable<EngProjectMetrics> projectsMetrics = AcessaDadosProjectMetrics.DadosProjectMetrics();
                Conexao.DesconectaEng();

                Conexao.ConectaSonar();
                var flagAnalise = true;
                var flag = true;
                string analise_id = string.Empty;
                string analise_id2 = string.Empty;
                foreach (var engproject in engprojects)
                {

                    IEnumerable<SonarProject> projects = AcessaDadosSonar.DadosSonar(engproject.sonar_id);

                    if (flagAnalise)
                    {
                        analise_id2 = projects.FirstOrDefault().analysis_uuid;
                    }
                    flagAnalise = false;

                    foreach (var project in projects)
                    {
                        analise_id = project.analysis_uuid;

                        if (analise_id2 != analise_id)
                        {
                            if (flag)
                            {
                                Conexao.ConectaEng();
                                Conexao.ConnEng.Execute("Insert into project_metrics (project_id, Executed_at, Analysis_id, coverage, lines, lines_to_cover, uncovered_lines)" +
                                                                            " Values(@project_id, @Executed_at, @Analysis_id, @Coverage, @lines, @lines_to_cover, @uncovered_lines)", parametros);
                                Conexao.DesconectaEng();
                            }
                            flag = true;
                        }

                        parametros.Add("project_id", engproject.project_id, DbType.Int32);
                        parametros.Add("Executed_at", TimeStampToDateTime(project.build_date), DbType.DateTime);
                        parametros.Add("Analysis_id", project.analysis_uuid, DbType.String);
                        switch (project.metric_id)
                        {
                            case 1:
                                parametros.Add("lines", project.value, DbType.VarNumeric);
                                break;
                            case 37:
                                parametros.Add("coverage", project.value, DbType.VarNumeric);
                                break;
                            case 39:
                                parametros.Add("lines_to_cover", project.value, DbType.VarNumeric);
                                break;
                            case 41:
                                parametros.Add("uncovered_lines", project.value, DbType.VarNumeric);
                                break;
                        }
                        if (project.analysis_uuid == null)
                            flag = false;

                        foreach (var projectMetric in projectsMetrics)
                        {
                            if ((project.analysis_uuid == projectMetric.Analysis_id) || (project.analysis_uuid == null))
                                flag = false;
                        }
                        analise_id2 = project.analysis_uuid;
                    }
                }
                Conexao.DesconectaSonar();


                //=============BASE KALOI=============


                Conexao.ConectaEng();
                IEnumerable<EngProject> engprojects2 = AcessaDadosProject.DadosProjects();
                IEnumerable<EngProjectKaloi> engProjectKalois = AcessaDadosProjectKaloi.DadosProjectKaloi();
                Conexao.DesconectaEng();

                flag = true;

                foreach (var engproject2 in engprojects2)
                {

                    Conexao.ConectaKaloi();
                    IEnumerable<KaloiMetrics> engKaloiMetrics = AcessaDadosKaloi.DadosKaloi(engproject2.project_id);
                    Conexao.DesconectaKaloi();

                    foreach (var engKaloiMetric in engKaloiMetrics)
                    {

                        parametrosKaloi.Add("project_id", engKaloiMetric.project_id, DbType.Int32);
                        parametrosKaloi.Add("pipeline_id", engKaloiMetric.pipeline_id, DbType.String);
                        parametrosKaloi.Add("metric", engKaloiMetric.metric, DbType.String);
                        parametrosKaloi.Add("metric_date", (engKaloiMetric.metric_date), DbType.DateTime);
                        parametrosKaloi.Add("value", engKaloiMetric.value, DbType.Int32);

                        foreach (var engProjectKaloi in engProjectKalois)
                        {
                            if ((engKaloiMetric.project_id == engProjectKaloi.project_id) && (engKaloiMetric.pipeline_id == engProjectKaloi.pipeline_id) && (engKaloiMetric.metric_date == engProjectKaloi.metric_date))
                            {
                                flag = false;
                            }
                        }

                        if (flag)
                        {
                            Conexao.ConectaEng();
                            Conexao.ConnEng.Execute("insert into project_kaloi (project_id, pipeline_id, metric, metric_date, value) " +
                                "Values(@project_id, @pipeline_id, @metric, @metric_date, @value)", parametrosKaloi);
                            Conexao.DesconectaEng();
                        }
                        flag = true;
                    }
                }

                Console.WriteLine("Finalizado Migração de dados");
            }
            catch (Exception)
            {                

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("engsoft12345@gmail.com");
                    mail.To.Add("luciano.fagundes@softplan.com.br");
                    mail.Subject = "[ERROR] Verificar aplicação de Metricas";
                    mail.Body = "<h1>Ocorreu um erro na migração dos dados das base Sonar/Kaloi</h1>";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("engsoft12345@gmail.com", "agesune1");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

            }

            Log.Salvar("SonarKaloi");
        }


        public static DateTime TimeStampToDateTime(Int64 timeStamp)
        {
            // Java timestamp is milliseconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(timeStamp).ToLocalTime();
            return dtDateTime;
        }

        
    }
}
