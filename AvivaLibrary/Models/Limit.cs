using System;
using System.Collections.Generic;
using System.Text;

namespace AvivaLibrary.Models
{
    public class Limit
    {
        public int Id { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public decimal Charge { get; set; }
        public char Type { get; set; } // 'P' for pesos, 'P' for percentage 
        public int RuleId { get; set; }
    }
}
