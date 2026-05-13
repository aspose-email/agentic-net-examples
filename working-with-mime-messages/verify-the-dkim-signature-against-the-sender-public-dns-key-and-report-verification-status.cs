using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the EML file to be verified
            string emlPath = "sample.eml";

            // Ensure the file exists; if not, create a minimal placeholder message
            if (!File.Exists(emlPath))
            {
                try
                {
                    MailMessage placeholderMessage = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body");
                    placeholderMessage.Save(emlPath);
                    placeholderMessage.Dispose();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Verify the DKIM signature of the EML file
            bool isSignatureValid;
            try
            {
                isSignatureValid = MailMessage.CheckSignature(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during DKIM verification: {ex.Message}");
                return;
            }

            // Report verification status
            if (isSignatureValid)
                Console.WriteLine("DKIM signature is valid.");
            else
                Console.WriteLine("DKIM signature is invalid or not present.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
