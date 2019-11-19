using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EFMetrica
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Migração de dados");

            var parametros = new DynamicParameters();

            Conexao.ConectaEng();
            IEnumerable<EngProject> engprojects = Conexao.ConnEng.Query<EngProject>("select project_id, sonar_id from projects where sonar_key <> ''");
            IEnumerable<EngProjectMetrics> projectsMetrics = Conexao.ConnEng.Query<EngProjectMetrics>("select * from project_metrics");
            Conexao.DesconectaEng();


            Conexao.ConectaSonar();
            var flagAnalise = true;
            var flag = true;
            string analise_id = string.Empty;
            string analise_id2 = string.Empty;
            foreach (var engproject in engprojects)
            {
                IEnumerable<SonarProject> projects = Conexao.ConnSonar.Query<SonarProject>("select " +
                                                                                           "    projects.id, projects.kee, projects.name," +
                                                                                           "    snapshots.build_date, project_measures.analysis_uuid, project_measures.metric_id, project_measures.value " +
                                                                                           "  from " +
                                                                                           "    projects " +
                                                                                           "      left join snapshots on " +
                                                                                           "        projects.uuid = snapshots.component_uuid " +
                                                                                           "        and snapshots.build_date >= 1564628400000 " +
                                                                                           "      left join project_measures on " +
                                                                                           "        project_measures.component_uuid = snapshots.component_uuid and " +
                                                                                           "        project_measures.analysis_uuid = snapshots.uuid and " +
                                                                                           "        project_measures.metric_id in (1, 37, 39, 41) " +
                                                                                           " where projects.id = " + engproject.sonar_id +
                                                                                           "   and projects.tags like '%#saj6%' " +
                                                                                           " order by build_date, analysis_uuid, project_measures.metric_id ");


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

                    if (engproject.project_id == 2677)
                        flag = flag;

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
            Console.WriteLine("Finalizado Migração de dados");
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
