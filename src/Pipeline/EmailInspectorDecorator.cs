using System;
using Phishing_Project.src.Models;

namespace Phishing_Project.src.Pipeline
{
    public abstract class EmailInspectorDecorator : IEmailInspector
    {
        protected readonly IEmailInspector _wrapInspector;

        protected EmailInspectorDecorator(IEmailInspector wrapInspector)
        {
            _wrapInspector = wrapInspector;
        }

        public virtual InspectionResult Inspect(EmailMessage email)
        {
            return _wrapInspector.Inspect(email);
        }
    }
}
