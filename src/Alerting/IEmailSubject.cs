using System;

namespace Phishing_Project.src.Alerting
{
    public interface IEmailSubject
    {
        void RegisterObserver(IThreatObserver observer);
        void RemoveObserver(IThreatObserver observer);
        void NotifyObserver(Models.EmailMessage email, Models.InspectionResult result);
    }
}
