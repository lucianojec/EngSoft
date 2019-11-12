using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

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
            foreach (var engproject in engprojects)
            {
                IEnumerable<SonarProject> projects = Conexao.ConnSonar.Query<SonarProject>("select " +
                                                                                      "projects.id, projects.kee, projects.name," +
                                                                                      "snapshots.build_date, project_measures.analysis_uuid, project_measures.value " +
                                                                                      "from " +
                                                                                      "projects " +
                                                                                      "left join snapshots on " +
                                                                                      "projects.uuid = snapshots.component_uuid " +
                                                                                       "and snapshots.build_date >= 1564628400000 " +
                                                                                      "left join project_measures on " +
                                                                                      "project_measures.component_uuid = snapshots.component_uuid and " +
                                                                                      "project_measures.analysis_uuid = snapshots.uuid and " +
                                                                                      //Verificar com o quadros esse parametro metric_id = 37
                                                                                      "project_measures.metric_id = 37 " +
                                                                                      "where projects.id = " + engproject.sonar_id +
                                                                                      "and projects.tags like '%#saj6%' " +
                                                                                      "order by projects.id, snapshots.created_at desc ");


                foreach (var project in projects)
                {
                    var flag = true;

                    parametros.Add("project_id", engproject.project_id, DbType.Int32);
                    parametros.Add("Executed_at", TimeStampToDateTime(project.build_date), DbType.DateTime);
                    parametros.Add("Analysis_id", project.analysis_uuid, DbType.String);
                    parametros.Add("Coverage", project.value, DbType.VarNumeric);

                    foreach (var projectMetric in projectsMetrics)
                    {
                        if ((project.analysis_uuid == projectMetric.Analysis_id) || (project.analysis_uuid == null))
                        {
                            flag = false;
                        }
                    }

                    if (flag)
                    {
                        Conexao.ConectaEng();
                        Conexao.ConnEng.Execute("Insert into project_metrics (project_id, Executed_at, Analysis_id, Coverage) Values(@project_id, @Executed_at, @Analysis_id, @Coverage)", parametros);
                        Conexao.DesconectaEng();
                        flag = true;
                    }
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
