using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFMetrica
{
    public class AcessaDadosProjectKaloi
    {
        public static IEnumerable<EngProjectKaloi> DadosProjectKaloi()
        {
            IEnumerable<EngProjectKaloi> engProjectKalois = Conexao.ConnEng.Query<EngProjectKaloi>("select * from project_kaloi");
            return engProjectKalois;
        }
    }
}
