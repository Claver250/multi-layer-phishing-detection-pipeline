using System;
using Phishing_Project.src.Models;

namespace Phishing_Project.src.Pipeline.Decorators
{
    public class HeaderAnalysisDecorator : EmailInspectorDecorator
    {
        public HeaderAnalysisDecorator(IEmailInspector wrapinspector) : base(wrapinspector) { }
        public override InspectionResult Inspect(EmailMessage email)
        {
            var result = base.Inspect(email);

            if (!string.IsNullOrWhiteSpace(email.Sender) && !string.IsNullOrWhiteSpace(email.ReplyTo))
            {
                if (email.Sender.ToLower() !=  email.ReplyTo.ToLower())
                {
                    result.ThreatScore += 30;
                    result.FlaggedReasons.Add("Mismatched Sender and Reply-To Address (+30 points)");
                }
            }
            return result;
        }
    }
}
