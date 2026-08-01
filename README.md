# Aspose.Email for .NET Examples

Build-validated C# examples for Aspose.Email for .NET, organized for developers, AI coding agents, and LLM-based development tools.

## About
Agent-generated C# examples for Aspose.Email for .NET, compiled, executed, and validated by an agentic pipeline. See [AGENTS.md](./AGENTS.md) for coding-agent instructions and [llms.txt](./llms.txt) for a machine-readable repository map.

[products.aspose.com/email/net/](https://products.aspose.com/email/net/)

## Overview
This repository provides working code examples demonstrating Aspose.Email for .NET capabilities. All examples are automatically generated, compiled, and validated using the Aspose.Email Examples Generator.

## Repository Structure
Examples are organized by feature category:
- `convert-between-formats/` - 135 example(s)
- `convert-thunderbird-mbox-files/` - 189 example(s)
- `programming-email-verification/` - 40 example(s)
- `programming-with-gmail/` - 142 example(s)
- `read-and-export-zimbra-tgz-files/` - 30 example(s)
- `working-with-amp-html-emails/` - 44 example(s)
- `working-with-exchange-ews-client/` - 557 example(s)
- `working-with-exchange-webdav-client/` - 156 example(s)
- `working-with-ibm-notes/` - 64 example(s)
- `working-with-imap-client/` - 303 example(s)
- `working-with-microsoft-graph-client/` - 39 example(s)
- `working-with-mime-messages/` - 343 example(s)
- `working-with-outlook-items/` - 511 example(s)
- `working-with-outlook-storage-files/` - 189 example(s)
- `working-with-pop3-client/` - 166 example(s)
- `working-with-smtp-client/` - 167 example(s)
- `zimbra/` - 9 example(s)

Each category contains standalone `.cs` files that can be compiled and run independently.

## Frequently Asked Questions

### How do I load and save EML or MSG files with Aspose.Email for .NET?

Use `MailMessage.Load(path)` for MIME formats such as EML and `MapiMessage.Load(path)` for Outlook MSG/OFT files. Save with verified options such as `SaveOptions.DefaultEml` or `SaveOptions.DefaultMsgUnicode`. See the message conversion and Outlook item folders for standalone examples.

### How do I convert email messages to HTML, MHTML, PDF, or other formats?

Load the source message, save through an Aspose.Email format such as MHTML when needed, and use Aspose.Words for visual exports like PDF or DOCX. Conversion examples live in the format conversion and MIME message categories.

### How do I send email using SMTP in Aspose.Email for .NET?

Create a `MailMessage`, configure `SmtpClient` with host, port, credentials, and `SecurityOptions`, then call `Send` inside a try/catch block. The SMTP category includes examples for sending, timeouts, delivery notifications, and retry/error handling.

### How do I receive or list email messages with IMAP or POP3?

Use `ImapClient` or `Pop3Client` from `Aspose.Email.Clients`, validate placeholder credentials before network calls, select or list folders where applicable, and dispose clients with `using`. IMAP and POP3 categories contain compile-validated examples.

### How do I work with Exchange EWS in Aspose.Email for .NET?

Create `IEWSClient` with `EWSClient.GetEWSClient(...)`, use Exchange folder/message types from the verified namespaces, and keep EWS samples separate from WebDav or Graph client patterns. See the Exchange EWS category for appointment, message, and folder examples.

### How do I use Microsoft Graph examples with Aspose.Email for .NET?

Use `Aspose.Email.Clients.Graph`, create `IGraphClient` with `GraphClient.GetClient(tokenProvider, tenantId)`, prefer `KnownFolders.Inbox`, and use verified overloads such as `ListMessages(folderId, null)`. The Graph category contains examples for mailbox, folder, and classification workflows.

### How do I process PST, OST, MBOX, or Zimbra TGZ storage files?

Use the storage APIs under `Aspose.Email.Storage.*` for PersonalStorage, MBOX readers, and Zimbra TGZ export flows. The storage and conversion categories include examples for extracting messages, preserving folders, retrying IO, and reporting conversion results.

### How do I create or read calendar appointments and ICS files?

Use `Appointment` for iCalendar/ICS flows and `MapiCalendar` for Outlook MSG calendar items. Exchange appointment CRUD should use EWS APIs such as `CreateAppointment` and `CancelAppointment`. Calendar and Outlook item categories show the supported patterns.

### How do I work with contacts and VCF files?

Use `Aspose.Email.PersonalInfo.Contact` and related email/phone/address collections, then save contacts with VCard formats when needed. Contact extraction and VCF examples are available in the Outlook item, MIME, and contact-related categories.

### How do I extract or save email attachments?

Load the message, iterate its attachment collection, create output directories before saving files, and wrap file operations in try/catch. Attachment examples appear across MIME, MSG, Exchange, Graph, and storage categories.

### Can these examples be used by AI coding agents like Claude, Copilot, or Cursor?

Yes. The repository includes root and per-category `AGENTS.md`, `llms.txt`, `readiness.json`, and `index.json` files so AI coding agents can navigate examples, categories, namespaces, and validation metadata programmatically.

### Do I need an Aspose.Email license to run these examples?

Aspose.Email can run in evaluation mode with product limitations, but production usage requires a valid license. Apply a license before production workflows and see https://purchase.aspose.com/buy or the temporary license page for evaluation options.

## Getting Started

### Prerequisites
- .NET SDK (net8.0 or compatible version)
- Aspose.Email for .NET NuGet package
- Valid Aspose license (for production use)

### Running Examples

Each example is a self-contained C# file. To run an example:
```bash
cd <CategoryFolder>
dotnet new console -o ExampleProject
cd ExampleProject
dotnet add package Aspose.Email
# Copy the example .cs file as Program.cs
dotnet run
```

## Code Patterns

### Loading a message
```csharp
using Aspose.Email;
using Aspose.Email.Mime;

MailMessage message = MailMessage.Load("input.eml");
Console.WriteLine(message.Subject);
```

### Error Handling
```csharp
if (!File.Exists(inputPath))
{
    Console.Error.WriteLine($"Error: File not found – {inputPath}");
    return;
}

try
{
    // Operations
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
}
```

### Important Notes
- Examples are single-file console applications; do not add multi-file projects.
- Dispose clients/streams with `using` when applicable.
- Avoid hardcoding secrets or license keys.

## Generation Architecture

This repository is produced by the Aspose.Email product-specific examples generator. It uses the shared Examples RAG/MCP platform for retrieval and skeleton code generation, then applies Aspose.Email-specific rules, validation, compile/run checks, repair, and publishing.

- The shared Task Generator supplies versioned product tasks.
- The shared Examples RAG/MCP platform retrieves product API context and generates skeleton code.
- The Aspose.Email generator owns product-specific rules and compile/runtime guardrails.

## Agentic .NET Ecosystem

Other Aspose products with agentic, build-validated example repositories:

| Product | Repository | Focus |
|---------|------------|-------|
| Aspose.Words for .NET | [aspose-words/agentic-net-examples](https://github.com/aspose-words/agentic-net-examples) | Word processing, DOCX, mail merge |
| Aspose.Cells for .NET | [aspose-cells/agentic-net-examples](https://github.com/aspose-cells/agentic-net-examples) | Spreadsheets, Excel, charts |
| Aspose.PDF for .NET | [aspose-pdf/agentic-net-examples](https://github.com/aspose-pdf/agentic-net-examples) | PDF creation, conversion, document automation |
| Aspose.HTML for .NET | [aspose-html/agentic-net-examples](https://github.com/aspose-html/agentic-net-examples) | HTML conversion, DOM editing |
| Aspose.Imaging for .NET | [aspose-imaging/agentic-net-examples](https://github.com/aspose-imaging/agentic-net-examples) | Image conversion, manipulation |
| Aspose.Slides for .NET | [aspose-slides/agentic-net-examples](https://github.com/aspose-slides/agentic-net-examples) | Presentations, PowerPoint |
| Aspose.Email for .NET | [aspose-email/agentic-net-examples](https://github.com/aspose-email/agentic-net-examples) | Email, calendars, messaging |
| Aspose.BarCode for .NET | [aspose-barcode/agentic-net-examples](https://github.com/aspose-barcode/agentic-net-examples) | Barcode generation and recognition |

## Related Resources

### Official Documentation
- [Aspose.Email for .NET Documentation](https://docs.aspose.com/email/net/) - Guides, tutorials, and feature overviews
- [API Reference](https://reference.aspose.com/email/net/) - Complete class/method reference
- [Release Notes](https://releases.aspose.com/email/net/release-notes/) - Version history and changelogs

### Downloads & Packages
- [NuGet Package](https://www.nuget.org/packages/Aspose.Email) - Install via `dotnet add package Aspose.Email`
- [Direct Downloads](https://releases.aspose.com/email/net/) - MSI/ZIP installers and DLLs

### Community & Support
- [Aspose.Email Forum](https://forum.aspose.com/c/email/12) - Community Q&A and official support
- [Aspose Blog - Email](https://blog.aspose.com/category/email/) - Tutorials, tips, and product updates
- [GitHub Issues](https://github.com/aspose-email/agentic-net-examples/issues) - Bug reports and feature requests

### AI-Friendly Navigation
- [Coding Agent Guide](./AGENTS.md) - Instructions for AI coding agents and code-generation tools
- [LLM Repository Map](./llms.txt) - Compact machine-readable navigation

### Licensing & Purchase
- [Purchase](https://purchase.aspose.com/buy) - Commercial license options
- [Temporary License](https://purchase.aspose.com/temporary-license/) - Full-feature evaluation license

## License
All examples use [Aspose.Email for .NET](https://products.aspose.com/email/net/) and require a valid license for production use. See [licensing options](https://purchase.aspose.com/buy).

---
*Generated and validated by a product-specific examples generator using shared Examples RAG/MCP infrastructure. See the [agentic examples metrics section](https://metrics.aspose.com/agents/sections/examples) | For AI-friendly guidance, see [AGENTS.md](./AGENTS.md) | Last updated: 2026-08-01*
