using System;
using Phishing_Project.src.Models;

namespace Phishing_Project.src.Alerting.Observers
{
    public class ConsoleAlertObserver : IThreatObserver
    {
        public void Update(EmailMessage email, InspectionResult result)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n=======================================");
            Console.WriteLine(" CRITICAL PHISHING THREAT DETECTED ");
            Console.WriteLine("\n=======================================");
            Console.ResetColor();

            Console.WriteLine($"From:          {email.Sender}");
            Console.WriteLine($"Subject:       {email.Subject}");
            Console.WriteLine($"Threat Score:  {result.ThreatScore}/100");
            Console.WriteLine("/nFlags raised: ");

            foreach(var reason in result.FlaggedReasons)
            {
                Console.WriteLine($" - {reason}");
            }

            Console.WriteLine("==========================================\n");
        }
    }
}
