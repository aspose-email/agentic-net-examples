using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Output file path
            string outputPath = "EncodedSubject.eml";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Original non‑ASCII subject
            string originalSubject = "Привет мир";

            // Encode the subject using RFC 2047 (UTF‑8 Base64)
            string encodedSubject = "=?utf-8?B?" + Convert.ToBase64String(Encoding.UTF8.GetBytes(originalSubject)) + "?=";

            // Create the mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                // Set the encoded subject directly
                message.Subject = encodedSubject;
                // Set the subject encoding for completeness
                message.SubjectEncoding = Encoding.UTF8;
                message.Body = "This email has an encoded subject line.";

                // Save the message to a file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine("Message saved to " + outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to save message: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
