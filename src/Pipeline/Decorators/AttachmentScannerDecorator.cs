using System;
using Phishing_Project.src.Models;

namespace Phishing_Project.src.Pipeline.Decorators
{
    public class AttachmentScannerDecorator : EmailInspectorDecorator
    {
        public AttachmentScannerDecorator(IEmailInspector wrapInspector) : base(wrapInspector) { }

        public override InspectionResult Inspect(EmailMessage email)
        {
            var result = base.Inspect(email);

            if (email.AttachmentNames != null)
            {
                foreach (var attachment in email.AttachmentNames)
                {
                    string lowerAttachment = attachment.ToLower();

                    if (lowerAttachment.Contains(".pdf.exe") || lowerAttachment.Contains(".txt.scr"))
                    {
                        result.ThreatScore += 50;
                        result.FlaggedReasons.Add($"High-risk double extension attachment: '{attachment}' (+50 points)");
                    }
                    else if (lowerAttachment.EndsWith(".exe") || lowerAttachment.EndsWith(".bat") || lowerAttachment.EndsWith(".scr"))
                    {
                        result.ThreatScore += 40;
                        result.FlaggedReasons.Add($"Executable attachment detected: '{attachment}' (+40 points)");
                    }
                }
            }
            return result;
        }
    }
}
