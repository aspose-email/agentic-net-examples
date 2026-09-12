using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file that represents the Notes document.
            const string msgPath = "document.msg";

            // Verify that the input file exists.
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

            // Load the MSG file using MapiMessage (Aspose.Email provides this for Outlook messages).
            MapiMessage mapMsg = MapiMessage.Load(msgPath);

            // Display basic properties.
            Console.WriteLine("Subject: " + mapMsg.Subject);
            Console.WriteLine("From: " + mapMsg.SenderName);
            Console.WriteLine("Body: " + mapMsg.Body);

            // NOTE: Aspose.Email does not expose a NotesDocument class or UNID handling in the
            // current API surface. If such functionality is required, it must be implemented
            // with a different library or a future version of Aspose.Email that provides
            // NotesDocument support.
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
