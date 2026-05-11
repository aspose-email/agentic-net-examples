using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "message.eml";

            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {emlPath}");
                return;
            }

            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                bool dkimSignatureFound = false;

                foreach (string headerName in mailMessage.Headers.Keys)
                {
                    if (string.Equals(headerName, "DKIM-Signature", StringComparison.OrdinalIgnoreCase))
                    {
                        dkimSignatureFound = true;
                        break;
                    }
                }

                Console.WriteLine($"DKIM signature present: {dkimSignatureFound}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
