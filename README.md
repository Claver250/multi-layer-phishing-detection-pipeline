# Phishing Detector

An enterprise-style email security gateway built in **C# (.NET 8)** that inspects incoming emails for phishing indicators and reacts to high-risk messages — architected around the **Decorator** and **Observer** design patterns so that threat *detection* and threat *response* stay fully decoupled.

Originally built as the term project for **CSC 322 – A Modern Programming Language (C#)** at the University of Lagos.

## Why this architecture

Traditional email filters cram every rule (header checks, keyword scans, attachment checks) and every response (logging, alerting, quarantining) into one tangled block of `if`/`else` logic. That's brittle: adding one new rule risks breaking the others, and detection logic ends up welded to response logic.

This project splits the problem in two:

- **Decorator pattern** → builds the inspection pipeline as a stack of independent, composable checks that each wrap the next and add to a shared threat score. New checks can be added, removed, or reordered without touching existing ones.
- **Observer pattern** → decouples "a threat was found" from "what happens next." The pipeline manager just broadcasts an event; any number of independent observers (console alert, quarantine, and — in future — Slack/SIEM/db logging) can subscribe without the manager knowing or caring who's listening.

## How it works

An `EmailMessage` is passed through a chain of decorators, each of which inspects one dimension of the message and adds to a shared `InspectionResult`:

```
AttachmentScannerDecorator
  └── BodySentimentDecorator
        └── HeaderAnalysisDecorator
              └── BaseEmailInspector   (starting score: 0)
```

| Check | Decorator | Flags | Penalty |
|---|---|---|---|
| Sender/Reply-To mismatch | `HeaderAnalysisDecorator` | Sender and Reply-To domains don't match | +30 |
| Social-engineering language | `BodySentimentDecorator` | Body contains phrases like *"immediate action required," "verify your account," "wire transfer," "urgent assistance," "password reset required"* (one hit per phrase found) | +15 each |
| Dangerous attachment | `AttachmentScannerDecorator` | `.exe`, `.bat`, `.scr` extension | +40 |
| Disguised attachment | `AttachmentScannerDecorator` | Double extension, e.g. `invoice.pdf.exe`, `notes.txt.scr` | +50 |

Once the score is tallied, `EmailPipelineManager` (the Observer subject) checks `InspectionResult.IsCritical` (`ThreatScore >= 50`). If it's tripped, every registered `IThreatObserver` is notified:

- **`ConsoleAlertObserver`** — prints a formatted high-visibility alert with the flagged reasons.
- **`MockQurantineObserver`** — simulates isolating the message from the inbox.

`Program.cs` wires the pipeline and observers together and runs three synthetic test emails through the gateway on startup — a clean email, a mildly suspicious one, and a weaponized spear-phishing attempt — printing the score and any raised flags for each.

## Project structure

```
src/
├── Program.cs                              # Assembles the pipeline + observers, runs the demo
├── Models/
│   ├── EmailMessage.cs                     # Sender, ReplyTo, Subject, Body, AttachmentNames
│   └── InspectionResult.cs                 # ThreatScore, FlaggedReasons, IsCritical (score >= 50)
├── Pipeline/
│   ├── IEmailInspector.cs                  # Component interface: Inspect(EmailMessage)
│   ├── BaseEmailInspector.cs               # Concrete component — innermost anchor, score starts at 0
│   ├── EmailInspectorDecorator.cs          # Abstract decorator — wraps an IEmailInspector
│   └── Decorators/
│       ├── HeaderAnalysisDecorator.cs      # Sender vs Reply-To domain check
│       ├── BodySentimentDecorator.cs       # Urgency/social-engineering keyword scan
│       └── AttachmentScannerDecorator.cs   # Executable / disguised attachment check
└── Alerting/
    ├── IEmailSubject.cs                    # Subject interface: Register/Remove/NotifyObserver
    ├── IThreatObserver.cs                  # Observer interface: Update(email, result)
    ├── EmailPipelineManager.cs             # Concrete subject — runs the pipeline, notifies on critical score
    └── Observers/
        ├── ConsoleAlertObserver.cs         # Prints a red console alert
        └── MockQurantineObserver.cs        # Simulates quarantining the message
```

## Getting started

**Prerequisites:** .NET 8 SDK

```bash
git clone <this-repo-url>
cd PhishingDetector
dotnet run --project "Phishing Project.csproj"
```

This builds the pipeline, registers both observers, and runs the three built-in test emails, printing each threat score and any triggered alerts to the console.

## Example output

Running a fully weaponized test email (spoofed reply-to, urgency phrases, and a `.pdf.exe` attachment) produces something like:

```
>>> SIMULATING INGESTION 3: Defending Active Phishing Vector <<<
=======================================
 CRITICAL PHISHING THREAT DETECTED
=======================================
From:          support@netflix-security-alert.com
Subject:       IMMEDIATE ACTION REQUIRED: Password breach
Threat Score:  95/100
Flags raised:
 - Mismatched Sender and Reply-To Address (+30 points)
 - Urgent social engneering keyword detected: 'password reset required' (+15 points)
 - Urgent social engneering keyword detected: 'wire transfer' (+15 points)
 - High-risk double extension attachment: 'billing_details.pdf.exe' (+50 points)
[QUARANTINE ACTION] Isolating email from 'support@netflix-security-alert.com'...
```

## Design principles this demonstrates

- **Single Responsibility Principle** — each decorator owns exactly one detection rule; each observer owns exactly one response action.
- **Open/Closed Principle** — new checks or new response channels can be added by writing a new class, with zero changes to existing pipeline or manager code.

## Roadmap

Ideas for turning this from an MVP into something production-grade:

- [ ] Async/concurrent inspection (`InspectAsync`) so decorators that hit external services (SPF/DNS lookups, threat-intel APIs) don't block each other
- [ ] Swap keyword matching in `BodySentimentDecorator` for an NLP/ML classifier for subtler social-engineering and BEC detection
- [ ] Real mail ingestion via Gmail/Microsoft Graph API or MTA/Milter integration, instead of the hardcoded sample emails in `Program.cs`
- [ ] Additional observers: SIEM logging (Splunk/Sentinel/ElasticSearch), Slack/Teams webhook alerts, database persistence via EF Core
- [ ] Move pipeline composition and score thresholds into DI + config (`appsettings.json`) instead of hardcoding the decorator stack in `Program.cs`

## License

No license file is currently included. Add one (e.g., MIT) if you intend this to be reused by others.
