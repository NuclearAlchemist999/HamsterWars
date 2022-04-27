using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamsterWars_DataAccess.Models
{
    public class PercentModel
    {
        public string? Name { get; set; }
        public double WinPercentRate { get; set; }
        public double LossPercentRate { get; set; }
    }
}
