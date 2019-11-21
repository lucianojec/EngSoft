using System;

namespace EFMetrica
{
    public class KaloiMetrics
    {
        public int project_id { get; set; }
        public string pipeline_id { get; set; }
        public string metric { get; set; }
        public DateTime metric_date { get; set; }
        public float value { get; set; }

    }
}
