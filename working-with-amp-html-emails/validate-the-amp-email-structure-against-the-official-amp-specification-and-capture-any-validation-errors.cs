using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            const string filePath = "amp_email.eml";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(filePath))
            {
                try
                {
                    const string placeholder = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Test AMP Email\r\nMIME-Version: 1.0\r\nContent-Type: text/plain; charset=utf-8\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(filePath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Validate the AMP email structure.
            MessageValidationResult result = MessageValidator.Validate(filePath);

            if (result.IsSuccess)
            {
                Console.WriteLine("AMP email validation succeeded. No errors found.");
            }
            else
            {
                Console.WriteLine("AMP email validation failed. Errors:");
                foreach (MessageValidationError error in result.Errors)
                {
                    Console.WriteLine($"- Line {error.LineNumber}: {error.Description} (Type: {error.ErrorType})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
