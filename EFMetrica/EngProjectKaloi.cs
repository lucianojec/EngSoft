using System;
using System.Collections.Generic;
using System.Text;

namespace EFMetrica
{
    public class EngProjectKaloi
    {
        public int project_id { get; set; }
        public string pipeline_id { get; set; }
        public string metric { get; set; }
        public DateTime metric_date { get; set; }
        public float value { get; set; }

    }
}