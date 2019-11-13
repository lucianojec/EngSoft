using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace ProjectMetricAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var parametros = new DynamicParameters();

            string url = @"https://integrationtestes.herokuapp.com/apis";
            var w = new WebClient();
            var jeisson = string.Empty;
            try
            {
                jeisson = w.DownloadString(url);
            }
            catch (Exception) { }

            Conexao.ConectaEng();
            IEnumerable<EngProjectMetricsIntegration> projectsMetricsIntegration = Conexao.ConnEng.Query<EngProjectMetricsIntegration>("select * from project_metrics_integration");
            Conexao.DesconectaEng();

            IEnumerable<IntegrationAPI> resultado = JsonConvert.DeserializeObject<IEnumerable<IntegrationAPI>>(jeisson);


            using (var db = new NpgsqlConnection(Conexao.strConnEng))
            {
                foreach (var json in resultado)
                {
                    var flag = true;

                    const string sql = @"INSERT INTO project_metrics_integration (project_id, executed_at, coverage, total, percentege ) 
                                                                           VALUES(@param2, @param3, @param4, @param5, @param6)";
                    foreach (var jsonItem in json.itens)
                    {

                        foreach (var pmIntegration in projectsMetricsIntegration)
                        {
                            if (jsonItem.criado == pmIntegration.executed_at)
                            {
                                flag = false;
                            }
                        }

                        if (flag)
                        {

                            db.Execute(sql, new
                            {
                                param2 = json.idGit,
                                param3 = jsonItem.criado,
                                param4 = jsonItem.cobertos,
                                param5 = jsonItem.total,
                                param6 = Convert.ToDouble(jsonItem.percentual.Substring(0, jsonItem.percentual.Length - 1))
                            },
                                    commandType: CommandType.Text);
                            flag = true;
                        }
                    }
                }
            }

        }
    }
}
