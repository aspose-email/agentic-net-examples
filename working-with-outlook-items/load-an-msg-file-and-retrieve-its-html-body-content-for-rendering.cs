using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "This is a placeholder plain‑text body."))
                    {
                        placeholder.Save(msgPath);
                    }

                    Console.WriteLine($"Placeholder MSG created at '{msgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and retrieve its HTML body.
            try
            {
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    string htmlBody = msg.BodyHtml ?? string.Empty;
                    Console.WriteLine("HTML Body Content:");
                    Console.WriteLine(htmlBody);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG or retrieve HTML body: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
