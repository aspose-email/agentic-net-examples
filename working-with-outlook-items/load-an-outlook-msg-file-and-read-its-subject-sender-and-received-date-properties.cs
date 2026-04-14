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
            // Path to the Outlook MSG file
            string msgPath = "message.msg";

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

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file and read its properties
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                Console.WriteLine("Subject: " + msg.Subject);
                Console.WriteLine("Sender: " + msg.SenderName);
                Console.WriteLine("Received: " + msg.ClientSubmitTime);
            }
        }
        catch (Exception ex)
        {
            // Output any unexpected errors
            Console.Error.WriteLine(ex.Message);
        }
    }
}
