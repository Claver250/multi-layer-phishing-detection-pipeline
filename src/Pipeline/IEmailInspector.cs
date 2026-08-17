using System;
using Phishing_Project.src.Models;

namespace Phishing_Project.src.Pipeline
{
    public interface IEmailInspector
    {
        /// <summary>
        ///  Analyzes an incoming email and calculates threat metrics
        /// </summary>
        /// <param name="email">The raw email message to inspect</param>
        /// <returns>An InspectionResult containning the threat score and telemetry tracking</returns>
        InspectionResult Inspect(EmailMessage email);
    }
}
