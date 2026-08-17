using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phishing_Project.src.Models
{
    public class InspectionResult
    {
        public int ThreatScore { get; set; }
        public List<string> FlaggedReasons { get; set; } = new List<string>();
        public bool IsCritical => ThreatScore >= 50;
    }
}
