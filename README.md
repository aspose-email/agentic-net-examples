# Aspose.Email for .NET Examples

AI-friendly repository containing validated C# examples for Aspose.Email for .NET.

## Overview
This repository provides working code examples demonstrating Aspose.Email for .NET capabilities. All examples are generated and compiled automatically; only passing samples are committed.

## Repository Structure
Examples are organized by feature category:
- `working-with-amp-html-emails/` - 16 example(s)

Each category contains standalone `.cs` files that can be compiled and run independently.

## Getting Started
### Prerequisites
- .NET SDK (net8.0 or compatible)
- Aspose.Email for .NET NuGet package
- Valid Aspose license for production use

### Running Examples
Each example is a self-contained C# file. To run an example:
```bash
cd working-with-amp-html-emails
# optionally inspect the file
# dotnet new console -n ExampleProject --framework net8.0
# copy the sample as Program.cs inside ExampleProject
# dotnet add package Aspose.Email
# dotnet run
```

## Code Patterns
### Building an AMP-enabled message
```csharp
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Mime;

MailMessage message = new MailMessage("from@example.com", "to@example.com", "Subject", "Text fallback");
AmpMessage amp = AmpMessage.Load("amp-template.html");
AlternateView ampView = AlternateView.CreateAlternateViewFromString(amp.AmpHtml, null, "text/x-amp-html");
message.AlternateViews.Add(ampView);
message.Save("output.msg", SaveOptions.DefaultMsgUnicode);
```

### Sending via SMTP
```csharp
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

SmtpClient client = new SmtpClient("smtp.example.com", 587, "user", "password")
{
    SecurityOptions = SecurityOptions.Auto
};
client.Send(message);
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

## Important Notes
- Examples are single-file console applications; do not add multi-file projects.
- Always dispose `SmtpClient`/streams with `using` when applicable.
- Provide plain-text and HTML fallbacks alongside AMP content when sending email.

## Related Resources
- [Aspose.Email for .NET Documentation](https://docs.aspose.com/email/net/)
- [API Reference](https://reference.aspose.com/email/net/)
- [Aspose Forum](https://forum.aspose.com/c/email/12)
- [AI Agent Guide](./agents.md) - repository-wide conventions

## License
Examples use Aspose.Email for .NET and require a valid license for production use. See [licensing](https://purchase.aspose.com/).

---
*This repository is maintained by automated code generation. For AI-friendly guidance, see [agents.md](./agents.md).* 
