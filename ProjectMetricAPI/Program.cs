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

            //Conecta com a base ENG e importa informações das tabelas "project_metrics_integration" e "project"
            Conexao.ConectaEng();
            IEnumerable<EngProjectMetricsIntegration> projectsMetricsIntegration = Conexao.ConnEng.Query<EngProjectMetricsIntegration>("select * from project_metrics_integration");
            IEnumerable<EngProject> engprojects = AcessaDadosProject.DadosProjects();
            Conexao.DesconectaEng();

            //Deserializa a API e o json
            IEnumerable<IntegrationAPI> resultado = JsonConvert.DeserializeObject<IEnumerable<IntegrationAPI>>(jeisson);

            using (var db = new NpgsqlConnection(Conexao.strConnEng))
            {
                //Varre a API e vai comparando as informações com a tabela PROJECT E PROJECT_METRICS_INTEGRATION. Controle os dados através de flags
                foreach (var json in resultado)
                {
                    var flag = true;
                    var flagProject = false;

                    const string sql = @"INSERT INTO project_metrics_integration (project_id, executed_at, covered, total, percentege ) 
                                                                           VALUES(@param2, @param3, @param4, @param5, @param6)";


                    foreach (var jsonItem in json.itens)
                    {

                        foreach (var engproject in engprojects)
                        {
                            if (json.idGit == engproject.project_id)
                            {
                                flagProject = true;
                                break;
                            }

                        }

                        if (flagProject == false)
                            Email.Enviar(json.idGit);


                        foreach (var pmIntegration in projectsMetricsIntegration)
                        {
                            if ((jsonItem.criado == pmIntegration.executed_at) || (flagProject == false))
                            {
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            var percetual = jsonItem.percentual.Replace('.', ',');

                            db.Execute(sql, new
                            {
                                param2 = json.idGit,
                                param3 = jsonItem.criado,
                                param4 = jsonItem.cobertos,
                                param5 = jsonItem.total,
                                param6 = Convert.ToDouble(percetual.Substring(0, percetual.Length - 1))
                            },
                                    commandType: CommandType.Text);
                            flag = true;
                            flagProject = false;
                        }
                    }
                }
            }

            Log.Salvar("MetricAPI");
        }
    }
}
