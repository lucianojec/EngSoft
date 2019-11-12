using System;

namespace EFMetrica
{
    public class EngProjectMetrics
    {
        public int Project_id { get; set; }
        public DateTime Executed_at { get; set; }
        public string Analysis_id { get; set; }
        public double Coverage { get; set; }

    }
}
