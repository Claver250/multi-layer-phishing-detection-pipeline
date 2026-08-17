using System;
using Phishing_Project.src.Models;

namespace Phishing_Project.src.Pipeline
{
    public class BaseEmailInspector : IEmailInspector
    {
        public InspectionResult Inspect(EmailMessage email)
        {
            return new InspectionResult();
        }
    }
}
