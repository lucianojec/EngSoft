using Dapper;
using System.Collections.Generic;

namespace EFMetrica
{
    public class AcessaDadosSonar
    {
        public static IEnumerable<SonarProject> DadosSonar(int sonarID)
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
                                                                                           "        project_measures.metric_id in (1, 37, 39, 41, 99, 100, 113, 117) " +
                                                                                           " where projects.id = " + sonarID +
                                                                                           "   and projects.tags like '%#saj6%' " +
                                                                                           " order by build_date, analysis_uuid, project_measures.metric_id ");

            return projects;
        }
    }
}


