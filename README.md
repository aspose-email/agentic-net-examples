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
- `programming-email-verification/` - 42 example(s)
- `programming-with-gmail/` - 154 example(s)
- `read-and-export-zimbra-tgz-files/` - 30 example(s)
- `working-with-amp-html-emails/` - 46 example(s)
- `working-with-exchange-ews-client/` - 568 example(s)
- `working-with-exchange-webdav-client/` - 175 example(s)
- `working-with-ibm-notes/` - 65 example(s)
- `working-with-imap-client/` - 322 example(s)
- `working-with-microsoft-graph-client/` - 49 example(s)
- `working-with-mime-messages/` - 348 example(s)
- `working-with-outlook-items/` - 514 example(s)
- `working-with-outlook-storage-files/` - 194 example(s)
- `working-with-pop3-client/` - 176 example(s)
- `working-with-smtp-client/` - 174 example(s)
- `zimbra/` - 9 example(s)

Each category contains standalone `.cs` files that can be compiled and run independently.

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
*Maintained by an [agentic example generation workflow](https://metrics.aspose.com/agents/sections/examples) | For AI-friendly guidance, see [AGENTS.md](./AGENTS.md) | Last updated: 2026-07-03*
