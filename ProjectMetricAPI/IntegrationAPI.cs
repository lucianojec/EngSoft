using System;
using System.Collections.Generic;

namespace ProjectMetricAPI
{
    public class IntegrationAPI
    {
        public string nome { get; set; }
        public int idGit { get; set; }
        public string ambiente { get; set; }
        public List<Iten> itens { get; set; }
    }

    public class Iten
    {
        public int total { get; set; }
        public double cobertos { get; set; }
        public string percentual { get; set; }
        public DateTime criado { get; set; }
    }
}
