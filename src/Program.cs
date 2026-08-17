using System;
using System.Collections.Generic;
using Phishing_Project.src.Models;
using Phishing_Project.src.Pipeline;
using Phishing_Project.src.Pipeline.Decorators;
using Phishing_Project.src.Alerting;
using Phishing_Project.src.Alerting.Observers;

namespace Phishing_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Enterprise Phishing Detection Gateway MVP";
            Console.WriteLine("================================================================");
            Console.WriteLine("   INITIALIZING ENTERPRISE EMAIL SECURITY GATEWAY ENGINE v1.0   ");
            Console.WriteLine("================================================================\n");

            // ================================================================
            // STEP 1: ASSEMBLE THE DECORATOR PIPELINE (Nesting Doll Pattern)
            // ================================================================
            // Core canvas initialized at the bottom of the chain
            IEmailInspector baseInspector = new BaseEmailInspector();

            // Layering the header domain verification check
            IEmailInspector headerLayer = new HeaderAnalysisDecorator(baseInspector);

            // Layering the social engineering text sentiment check
            IEmailInspector sentimentLayer = new BodySentimentDecorator(headerLayer);

            // Layering the final high-priority extension check on the outside
            IEmailInspector securityPipeline = new AttachmentScannerDecorator(sentimentLayer);

            Console.WriteLine("[CONFIG] Multi-layered inspection pipeline assembled successfully.");

            // ================================================================
            // STEP 2: SETUP THE PIPELINE MANAGEMENT SUBJECT
            // ================================================================
            EmailPipelineManager manager = new EmailPipelineManager(securityPipeline);

            // ================================================================
            // STEP 3: CONFIGURE AND REGISTER OBSERVATIONAL RESPONDERS
            // ================================================================
            IThreatObserver alertMonitor = new ConsoleAlertObserver();
            IThreatObserver quarantineAction = new MockQurantineObserver();

            manager.RegisterObserver(alertMonitor);
            manager.RegisterObserver(quarantineAction);

            Console.WriteLine("[CONFIG] Threat event observers actively attached to pipeline broadcast.");
            Console.WriteLine("\n[STATUS] Security Gateway is Active. Processing incoming traffic...\n");
            Console.WriteLine("-----------------------------------------------------------------");

            // ================================================================
            // STEP 4: GENERATE SYNTHETIC TEST CASES (MOCK MAIL BOX DATA)
            // ================================================================

            // Test Case 1: Standard internal operational communications
            var cleanEmail = new EmailMessage
            {
                Sender = "manager@mycompany.com",
                ReplyTo = "manager@mycompany.com",
                Subject = "Project Status Update",
                Body = "Hi Team, please send over the latest Q3 slides by tomorrow afternoon. Thanks!",
                AttachmentNames = new List<string> { "slides_draft.pptx" }
            };

            // Test Case 2: Marketing or third party data with passive keywords
            var suspiciousEmail = new EmailMessage
            {
                Sender = "billing-support@payment-update.com",
                ReplyTo = "billing-support@payment-update.com",
                Subject = "Account Verification required",
                Body = "Please verify your account settings to avoid temporary interruptions.",
                AttachmentNames = new List<string> { "receipt.pdf" }
            };

            // Test Case 3: Fully weaponized spear-phishing simulation
            var maliciousEmail = new EmailMessage
            {
                Sender = "support@netflix-security-alert.com",
                ReplyTo = "hacker-mailbox@darkweb.org",
                Subject = "IMMEDIATE ACTION REQUIRED: Password breach",
                Body = "We detected unauthorized access to your billing information. You must perform a password reset requested on your profile or wire transfer funds immediately.",
                AttachmentNames = new List<string> { "billing_details.pdf.exe" }
            };

            // ================================================================
            // STEP 5: SIMULATE PROCESSING INCOMING STREAM
            // ================================================================

            // Simulation 1: Clean Email Evaluation
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n>>> SIMULATING INGESTION 1: Safe Corporate Communication <<<");
            Console.ResetColor();
            var result1 = manager.ProcessEmail(cleanEmail);
            Console.WriteLine($"[RESULT] Ingestion complete. Threat Score: {result1.ThreatScore}/100. Action: Delivered to Inbox.");

            // Simulation 2: Low-Risk Suspicious Email Evaluation
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n>>> SIMULATING INGESTION 2: Low-Level External Risk Heuristics <<<");
            Console.ResetColor();
            var result2 = manager.ProcessEmail(suspiciousEmail);
            Console.WriteLine($"[RESULT] Ingestion complete. Threat Score: {result2.ThreatScore}/100. Action: Delivered with Warning Banner.");
            foreach (var reason in result2.FlaggedReasons)
            {
                Console.WriteLine($"    Flag: {reason}");
            }

            // Simulation 3: Weaponized Targeted Attack Evaluation
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n>>> SIMULATING INGESTION 3: Defending Active Phishing Vector <<<");
            Console.ResetColor();
            // This call cascades through all decorators and instantly calls NotifyObservers() 
            // because the threshold passes 50.
            var result3 = manager.ProcessEmail(maliciousEmail);
            Console.WriteLine($"[RESULT] Processing finalized. Security components returned final score of {result3.ThreatScore}/100.");
            foreach (var reason in result3.FlaggedReasons)
            {
                Console.WriteLine($"    Flag: {reason}");
            }

            Console.WriteLine("\n-----------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("All architectural MVP validation tests completed successfully!");
            Console.ResetColor();
            Console.WriteLine("Press [Enter] to shut down security gateway gateway interface...");
            Console.ReadLine();
        }
    }
}