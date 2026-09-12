using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

namespace AsposeEmailMIMEValidation
{
    class Program
    {
        static void Main()
        {
            try
            {
                string emlPath = "sample.eml";

                // Ensure the input file exists; create a minimal placeholder if it does not.
                if (!File.Exists(emlPath))
                {
                    try
                    {
                        string placeholder = "From: placeholder@example.com\r\nTo: placeholder@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder EML file.";
                        File.WriteAllText(emlPath, placeholder);
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder EML file: {ioEx.Message}");
                        return;
                    }
                }

                // Validate the MIME message.
                MessageValidationResult validationResult;
                try
                {
                    validationResult = MessageValidator.Validate(emlPath);
                }
                catch (Exception validateEx)
                {
                    Console.Error.WriteLine($"Validation process failed: {validateEx.Message}");
                    return;
                }

                // Log detailed error information.
                if (validationResult?.Errors == null || validationResult.Errors.Count == 0)
                {
                    Console.WriteLine("No validation errors were found in the MIME message.");
                }
                else
                {
                    Console.WriteLine($"Found {validationResult.Errors.Count} validation error(s):");
                    foreach (MessageValidationError error in validationResult.Errors)
                    {
                        // Use the error's ToString() representation to avoid reliance on a missing property.
                        Console.WriteLine($"Line {error.LineNumber}: {error} (Type: {error.ErrorType})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
