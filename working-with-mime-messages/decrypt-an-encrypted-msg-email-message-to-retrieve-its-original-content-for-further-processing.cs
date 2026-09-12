using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        const string inputPath = "encrypted.msg";
        const string outputPath = "decrypted.msg";

        try
        {
            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                // Create a simple unencrypted message as a placeholder.
                MailMessage placeholder = new MailMessage();
                placeholder.From = "placeholder@example.com";
                placeholder.To = "placeholder@example.com";
                placeholder.Subject = "Placeholder Message";
                placeholder.Body = "This is a placeholder message.";
                placeholder.Save(inputPath, SaveOptions.DefaultMsg);
            }

            // Load the MSG file.
            MapiMessage encryptedMsg = MapiMessage.Load(inputPath);

            // Decrypt the message. If the message is not encrypted, Decrypt returns the same instance.
            MapiMessage decryptedMsg = encryptedMsg.Decrypt();

            // Convert to MailMessage for further processing or saving.
            MailConversionOptions conversionOpts = new MailConversionOptions();
            MailMessage mail = decryptedMsg.ToMailMessage(conversionOpts);

            // Save the decrypted message.
            mail.Save(outputPath, SaveOptions.DefaultMsg);

            Console.WriteLine($"Decrypted message saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing the message: {ex.Message}");
            // Gracefully exit without throwing.
        }
    }
}
