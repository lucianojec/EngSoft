using Dapper;
using System.Collections.Generic;

namespace ProjectMetricAPI
{
    public class AcessaDadosProject
    {
        public static IEnumerable<EngProject> DadosProjects()
        {
            IEnumerable<EngProject> engprojects = Conexao.ConnEng.Query<EngProject>("select project_id, sonar_id from projects where sonar_key <> ''");
            return engprojects;
        }
    }
}
