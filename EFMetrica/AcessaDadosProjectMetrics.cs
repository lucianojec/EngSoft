using Dapper;
using System.Collections.Generic;

namespace EFMetrica
{
    public class AcessaDadosProjectMetrics
    {
        public static IEnumerable<EngProjectMetrics> DadosProjectMetrics()
        {
            IEnumerable<EngProjectMetrics> projectsMetrics = Conexao.ConnEng.Query<EngProjectMetrics>("select * from project_metrics");
            return projectsMetrics;
        }
    }
}
