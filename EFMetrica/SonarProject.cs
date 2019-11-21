using System;

namespace EFMetrica
{
    public class SonarProject
    {
        public int id { get; set; }

        public string kee { get; set; }

        public string name { get; set; }
        public Int64 build_date { get; set; }
        public string analysis_uuid { get; set; }
        public int metric_id { get; set; }
        public double value { get; set; }

    }
}