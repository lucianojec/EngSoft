using Dapper;
using System.Collections.Generic;

namespace EFMetrica
{
    public class AcessaDadosKaloi
    {
        public static IEnumerable<KaloiMetrics> DadosKaloi(int projectId)
        {
            IEnumerable<KaloiMetrics> engprojects = Conexao.ConnKaloi.Query<KaloiMetrics>("select " +
                                                                                            "pipeline_id, " +
                                                                                            "project_id, " +
                                                                                            "metric, " +
                                                                                            "metric_date, " +
                                                                                            "value " +
                                                                                         "from metrics " +
                                                                                        "where branch_name = 'master'" +
                                                                                        "  and metric_date >= '2019-08-01 00:00:00 '"+
                                                                                        "  and project_id = " + projectId);            
            return engprojects;
        }
    }
}
