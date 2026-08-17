using System;
using Phishing_Project.src.Models;

namespace Phishing_Project.src.Alerting.Observers
{
    public class MockQurantineObserver : IThreatObserver
    {
        public void Update(EmailMessage email, InspectionResult result)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[QUARANTINE ACTION] Isolating email from '{email.Sender}'...");
            Console.WriteLine($"[QUARANTINE ACTION] moving message '{email.Subject}' to secure thraet vault");
            Console.WriteLine($"[QUARANTINE ACTION] User inbox acccess revoked for this message ID.");

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
