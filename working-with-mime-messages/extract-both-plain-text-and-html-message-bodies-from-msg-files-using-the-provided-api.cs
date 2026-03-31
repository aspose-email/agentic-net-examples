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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file inside a using block to ensure proper disposal
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Extract plain‑text body
                string plainBody = msg.Body;

                // Extract HTML body (may be empty if not present)
                string htmlBody = msg.BodyHtml;

                Console.WriteLine("Plain Text Body:");
                Console.WriteLine(string.IsNullOrEmpty(plainBody) ? "(none)" : plainBody);

                Console.WriteLine("\nHTML Body:");
                Console.WriteLine(string.IsNullOrEmpty(htmlBody) ? "(none)" : htmlBody);
            }
        }
        catch (Exception ex)
        {
            // Gracefully report any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
