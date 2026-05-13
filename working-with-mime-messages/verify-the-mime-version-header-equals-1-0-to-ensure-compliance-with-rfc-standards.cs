using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";

            // Ensure the file exists; create a minimal placeholder if missing
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

                try
                {
                    using (StreamWriter writer = new StreamWriter(emlPath, false))
                    {
                        writer.WriteLine("MIME-Version: 1.0");
                        writer.WriteLine("Subject: Placeholder");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder email body.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML message and validate the MIME-Version header
            try
            {
                using (MailMessage message = MailMessage.Load(emlPath))
                {
                    string mimeVersion = message.Headers["MIME-Version"];
                    if (mimeVersion == "1.0")
                    {
                        Console.WriteLine("MIME-Version header is compliant (1.0).");
                    }
                    else
                    {
                        Console.WriteLine($"MIME-Version header is non‑compliant: '{mimeVersion ?? "missing"}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading or processing the EML file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
