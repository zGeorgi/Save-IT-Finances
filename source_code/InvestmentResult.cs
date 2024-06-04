using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class InvestmentResult
    {
        public decimal TotalInvested { get; set; }
        public decimal MaxReturn { get; set; }
        public decimal MinReturn { get; set; }
        public decimal TotalProfitMaxReturn { get; set; }
        public decimal TotalProfitMinReturn { get; set; }
        public decimal TotalFeesMaxReturn { get; set; }
        public decimal TotalFeesMinReturn { get; set; }
        public decimal TotalTaxesMaxReturn { get; set; }
        public decimal TotalTaxesMinReturn { get; set; }
    }
}
