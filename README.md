# Aspose.Email for .NET Examples

AI-friendly repository containing validated C# examples for Aspose.Email for .NET API.

## Overview
This repository provides working code examples demonstrating Aspose.Email for .NET capabilities. All examples are automatically generated, compiled, and validated using the Aspose.Email Examples Generator.

## Repository Structure
Examples are organized by feature category:
- `zimbra/` - 5 example(s)

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
- [AI Agent Guide](./agents.md) - For AI agents and code generation tools

## License
All examples use Aspose.Email for .NET and require a valid license for production use. See [licensing](https://purchase.aspose.com/).

---
*This repository is maintained by automated code generation. For AI-friendly guidance, see [agents.md](./agents.md).*