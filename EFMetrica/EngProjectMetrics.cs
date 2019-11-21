using System;
using System.Collections.Generic;

namespace EFMetrica
{
    public class EngProjectMetrics
    {
        public int Project_id { get; set; }
        public DateTime Executed_at { get; set; }
        public string Analysis_id { get; set; }
        public double Coverage { get; set; }
        public List<Measure> measures { get; set; }

    }

    public class Measure
    {
        public int lines { get; set; }
        public int lines_to_cover { get; set; }
        public int uncovered_lines { get; set; }
    }


}
