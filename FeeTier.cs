using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class FeeTier
    {
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal FeePercentage { get; set; }
    }
}
