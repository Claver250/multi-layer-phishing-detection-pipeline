using System;
using System.Collections.Generic;
using Phishing_Project.src.Models;
using Phishing_Project.src.Pipeline;

namespace Phishing_Project.src.Alerting
{
    public class EmailPipelineManager : IEmailSubject
    {
        private readonly List<IThreatObserver> _observers = new List<IThreatObserver>();
        private readonly IEmailInspector _pipeline;
        public EmailPipelineManager(IEmailInspector pipeline)
        {
            _pipeline = pipeline;
        }

        public void RegisterObserver(IThreatObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }
        public void RemoveObserver(IThreatObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }
        public void NotifyObserver(EmailMessage email, InspectionResult result)
        {
            foreach (var observer in _observers)
            {
                observer.Update(email, result);
            }
        }

        public InspectionResult ProcessEmail(EmailMessage email)
        {
            InspectionResult result = _pipeline.Inspect(email);

            if (result.IsCritical)
            {
                NotifyObserver(email, result);
            }
            return result;
        }
    }
}
