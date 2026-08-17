using System;
using Phishing_Project.src.Models;

namespace Phishing_Project.src.Alerting
{
    public interface IThreatObserver
    {
        void Update(EmailMessage message, InspectionResult result);
    }
}
