namespace EFMetrica
{
    public class AcessaDadosSonar
    {

        //public List<SonarProject> Dado()
        //{
        //    Conexao.ConectaSonar();

        //    IEnumerable<SonarProject> projects = Conexao.Conn.Query<SonarProject>("select " +
        //                                                                      "projects.id, projects.kee, projects.name," +
        //                                                                      "snapshots.build_date, project_measures.analysis_uuid, project_measures.value " +
        //                                                                      "from " +
        //                                                                      "projects " +
        //                                                                        "left join snapshots on " +
        //                                                                          "projects.uuid = snapshots.component_uuid " +
        //                                                                          "and snapshots.build_date >= 1564628400000 " +
        //                                                                        "left join project_measures on " +
        //                                                                          "project_measures.component_uuid = snapshots.component_uuid and " +
        //                                                                          "project_measures.analysis_uuid = snapshots.uuid and " +
        //                                                                          "project_measures.metric_id = 37 " +
        //                                                                    "where projects.id = 135764-- id do projeto no sonar(projects.sonar_id) " +
        //                                                                      "and projects.tags like '%#saj6%' " +
        //                                                                    "order by projects.id, snapshots.created_at desc ");

        //    DataTable dt = new DataTable();

        //    Console.WriteLine("{0} - {1} - {2} ", "id", "kee", "name");
        //    foreach (var project in projects)
        //        return project;


        //Console.WriteLine("{0} - {1} - {2}  - {3} - {4} - {5}", project.id, project.kee, project.name, project.build_date, project.analysis_uuid, project.value);

        //}
    }
}


