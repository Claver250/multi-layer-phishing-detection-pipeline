using System;
using Phishing_Project.src.Models;

namespace Phishing_Project.src.Pipeline.Decorators
{
    public class BodySentimentDecorator : EmailInspectorDecorator
    {
        private readonly List<string> _phishingKeywords = new List<string>
        {
            "immediate action required",
            "verify your account",
            "wire transfer",
            "urgent assistance",
            "password reset required"
        };
        public  BodySentimentDecorator(IEmailInspector wrapInspector) : base(wrapInspector) { }
        public override InspectionResult Inspect(EmailMessage email)
        {
            var result = base.Inspect(email);

            if (!string.IsNullOrWhiteSpace(email.Body))
            {
                string lowerBody = email.Body.ToLower();

                foreach (var keyword in _phishingKeywords)
                {
                    if (lowerBody.Contains(keyword)) 
                    {
                        result.ThreatScore += 15;
                        result.FlaggedReasons.Add($"Urgent social engneering keyword detected: '{keyword}' (+15 points)");
                    }
                }
            }
            return result;
        }
    }
}
