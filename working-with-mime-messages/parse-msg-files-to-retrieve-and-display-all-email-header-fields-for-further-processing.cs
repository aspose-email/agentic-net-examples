using Aspose.Email.Mime;
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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify the file exists before attempting to load
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

            // Load the Outlook Message file
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Display basic properties
            Console.WriteLine($"Subject: {msg.Subject}");
            Console.WriteLine($"From: {msg.SenderName} <{msg.SenderEmailAddress}>");
            Console.WriteLine($"Body: {msg.Body}");

            // Retrieve and display all header fields, if the Headers collection is available
            HeaderCollection headers = msg.Headers;
            if (headers != null)
            {
                Console.WriteLine("Headers:");
                foreach (string headerName in headers.AllKeys)
                {
                    Console.WriteLine($"{headerName}: {headers[headerName]}");
                }
            }

            // List attachment file names
            foreach (MapiAttachment attachment in msg.Attachments)
            {
                Console.WriteLine($"Attachment: {attachment.FileName}");
            }
        }
        catch (Exception ex)
        {
            // Output any unexpected errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
