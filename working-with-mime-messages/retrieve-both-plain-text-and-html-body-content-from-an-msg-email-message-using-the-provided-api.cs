using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

// Author: Aspose.Email example - retrieve plain‑text and HTML bodies from an MSG file
class Program
{
    static void Main()
    {
        try
        {
            const string msgPath = "sample.msg";

            // Guard against missing input file
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

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the Outlook MSG message
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Plain‑text body (Body property)
            string plainBody = msg.Body;

            // HTML body (BodyHtml property)
            string htmlBody = msg.BodyHtml;

            Console.WriteLine("Plain‑Text Body:");
            Console.WriteLine(plainBody);
            Console.WriteLine();

            Console.WriteLine("HTML Body:");
            Console.WriteLine(htmlBody);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
