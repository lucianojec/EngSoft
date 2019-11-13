using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMetricAPI
{
    public class EngProjectMetricsIntegration
    {
        public int integration_id { get; set; }
        public int project_id { get; set; }
        public DateTime executed_at { get; set; }
        public double coverage { get; set; }
        public int total { get; set; }
        public int percentege { get; set; }

    }
}
