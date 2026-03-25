---
name: working-with-amp-html-emails
description: C# examples for AMP HTML emails using Aspose.Email for .NET
language: csharp
framework: net8.0
parent: ../agents.md
---

# AGENTS - working-with-amp-html-emails

## Persona
You are a C# developer working with AMP HTML email content using Aspose.Email for .NET. This folder contains standalone `.cs` examples for AMP message creation, conversion, and SMTP delivery. See parent [agents.md](../agents.md) for repo-wide rules.

## Scope
- Examples for constructing, converting, and sending AMP-enabled messages.
- Files are standalone console samples stored in this folder.

## Required Namespaces
- `using Aspose.Email;` (16/16 files)
- `using Aspose.Email.Amp;` (9/16 files)
- `using Aspose.Email.Mapi;` (5/16 files)
- `using Aspose.Email.Mime;` (4/16 files)
- `using Aspose.Email.Clients;` (3/16 files)
- `using Aspose.Email.Clients.Smtp;` (3/16 files)
- `using System;` (16/16 files)
- `using System.IO;` (14/16 files)
- `using System.Text;` (3/16 files)
- `using System.Collections.Generic;` (1/16 files)
- `using System.Reflection;` (1/16 files)

## Common Code Pattern
Create AMP content as an alternate view, keep plain/HTML fallbacks, and save or send:
```csharp
MailMessage message = new MailMessage("from@example.com", "to@example.com")
{
    Subject = "AMP demo",
    Body = "Plain text fallback"
};
string ampHtml = File.ReadAllText("amp-template.html");
AlternateView ampView = AlternateView.CreateAlternateViewFromString(ampHtml, null, "text/x-amp-html");
message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("<p>HTML fallback</p>", null, "text/html"));
message.AlternateViews.Add(ampView);
message.Save("output.msg", SaveOptions.DefaultMsgUnicode);
```

## Files in this folder
| File | Key APIs | Description |
|------|----------|-------------|
| [create-and-send-amp-email-via-smtp.cs](./create-and-send-amp-email-via-smtp.cs) | `MailMessage`, `AmpMessage`, `SmtpClient` | Build AMP message and send over SMTP with STARTTLS. |
| [import-amp-message-and-print-amp-html.cs](./import-amp-message-and-print-amp-html.cs) | `AmpMessage` | Load AMP MSG and print AMP HTML part. |
| [create-mailmessage-and-save-as-msg.cs](./create-mailmessage-and-save-as-msg.cs) | `MailMessage` | Build basic message with alternate views and save as MSG. |
| [build-amp-message-and-save-to-msg.cs](./build-amp-message-and-save-to-msg.cs) | `AmpMessage` | Compose AMP message programmatically and persist to MSG. |
| [create-amp-alternate-view-and-save-msg.cs](./create-amp-alternate-view-and-save-msg.cs) | `AlternateView`, `AmpMessage` | Add AMP alternate view to MailMessage and save. |
| [attach-msg-as-alternate-view-and-save-eml.cs](./attach-msg-as-alternate-view-and-save-eml.cs) | `MapiMessage`, `AlternateView` | Attach MSG content as alternate view and save EML. |
| [add-plain-and-html-views-and-save-msg.cs](./add-plain-and-html-views-and-save-msg.cs) | `MailMessage`, `AlternateView` | Add plain + HTML views then save MSG. |
| [send-msg-file-via-smtp.cs](./send-msg-file-via-smtp.cs) | `MapiMessage`, `SmtpClient` | Load MSG and send through SMTP. |
| [read-amp-alternate-view-from-msg.cs](./read-amp-alternate-view-from-msg.cs) | `AlternateView` | Read AMP part from MSG and display content type. |
| [convert-msg-to-amp-message-and-save.cs](./convert-msg-to-amp-message-and-save.cs) | `AmpMessage`, `MapiMessage` | Convert MSG to `AmpMessage` and save. |
| [create-amp-message-and-save-via-stream.cs](./create-amp-message-and-save-via-stream.cs) | `AmpMessage` | Create AMP message and write via stream. |
| [create-amp-email-and-save-msg.cs](./create-amp-email-and-save-msg.cs) | `AmpMessage`, `MailMessage` | Compose AMP email and save as MSG. |
| [send-amp-msg-via-smtp.cs](./send-amp-msg-via-smtp.cs) | `MapiMessage`, `SmtpClient` | Convert AMP MSG to MailMessage and send. |
| [import-amp-message-and-list-alternate-views.cs](./import-amp-message-and-list-alternate-views.cs) | `AmpMessage` | Enumerate alternate views inside AMP message. |
| [enumerate-mapi-message-capabilities.cs](./enumerate-mapi-message-capabilities.cs) | `MapiMessage` | List MAPI capability flags for a message. |
| [list-available-amp-components.cs](./list-available-amp-components.cs) | `AmpMessage` | Reflect available AMP components exposed by the API. |

## Category Statistics
- Total examples: 16

## Category-Specific Tips
### Key API Surface
- `AmpMessage` / `AlternateView` for AMP HTML content
- `MailMessage` and `MapiMessage` for MIME/MSG handling
- `SmtpClient` with `SecurityOptions.Auto` for TLS/STARTTLS
- `SaveOptions.DefaultMsgUnicode` to preserve AMP markup

### Rules
- Always include text/HTML fallbacks when sending AMP.
- When converting `MapiMessage` to `MailMessage`, call `ConvertToMailMessage()` then attach AMP view if needed.
- Use `AlternateView.ContentType = "text/x-amp-html"` for AMP parts.

### Warnings
- Missing fallbacks may cause AMP-capable clients to drop rendering; keep plain text in Body.
- `SmtpClient` requires valid host/credentials; sample code uses placeholders.
- Streams must be closed; wrap file access in `using` blocks.

## General Tips
- See parent [agents.md](../agents.md) for repository-wide boundaries and testing guide.

<!-- AUTOGENERATED:START -->
Updated: 2026-03-24 | Run: `20260324_002`
<!-- AUTOGENERATED:END -->
