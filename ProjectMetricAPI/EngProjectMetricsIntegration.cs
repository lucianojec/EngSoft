using System;

namespace ProjectMetricAPI
{
    public class EngProjectMetricsIntegration
    {
        public int integration_id { get; set; }
        public int project_id { get; set; }
        public DateTime executed_at { get; set; }
        public double coverage { get; set; }
        public int total { get; set; }
        public double percentege { get; set; }

    }
}
