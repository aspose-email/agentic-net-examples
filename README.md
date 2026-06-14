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
- `convert-thunderbird-mbox-files/` - 182 example(s)
- `programming-email-verification/` - 40 example(s)
- `programming-with-gmail/` - 142 example(s)
- `read-and-export-zimbra-tgz-files/` - 30 example(s)
- `working-with-amp-html-emails/` - 44 example(s)
- `working-with-exchange-ews-client/` - 557 example(s)
- `working-with-exchange-webdav-client/` - 156 example(s)
- `working-with-ibm-notes/` - 64 example(s)
- `working-with-imap-client/` - 303 example(s)
- `working-with-microsoft-graph-client/` - 38 example(s)
- `working-with-mime-messages/` - 343 example(s)
- `working-with-outlook-items/` - 511 example(s)
- `working-with-outlook-storage-files/` - 189 example(s)
- `working-with-pop3-client/` - 166 example(s)
- `working-with-smtp-client/` - 167 example(s)
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

## Related Resources
- [Aspose.Email for .NET Documentation](https://docs.aspose.com/email/net/)
- [API Reference](https://reference.aspose.com/email/net/)
- [Aspose Forum](https://forum.aspose.com/c/email/12)
- [Coding Agent Guide](./AGENTS.md) - Instructions for AI coding agents and code-generation tools
- [LLM Repository Map](./llms.txt) - Compact machine-readable navigation

## Related Agentic .NET Example Repositories
- [Aspose.Words](https://github.com/aspose-words/agentic-net-examples)
- [Aspose.Slides](https://github.com/aspose-slides/agentic-net-examples)
- [Aspose.HTML](https://github.com/aspose-html/agentic-net-examples)

## License
All examples use Aspose.Email for .NET and require a valid license for production use. See [licensing](https://purchase.aspose.com/).

---
*This repository is maintained by automated code generation. For coding-agent guidance, see [AGENTS.md](./AGENTS.md).*
