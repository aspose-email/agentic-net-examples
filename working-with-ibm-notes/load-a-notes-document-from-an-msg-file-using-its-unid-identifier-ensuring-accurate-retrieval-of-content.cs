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
            // Path to the MSG file that contains the Notes document
            string msgFilePath = "note.msg";

            // The UNID identifier of the Notes document we want to retrieve
            string targetUnid = "YOUR_UNID_HERE";

            // Verify that the MSG file exists before attempting to load it
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                return;
            }

            // Load the MSG file as a MapiMessage
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                // The ItemId property holds the unique identifier (UNID) for Notes documents
                string messageUnid = msg.ItemId;

                if (string.Equals(messageUnid, targetUnid, StringComparison.OrdinalIgnoreCase))
                {
                    // The UNID matches – output the relevant content
                    Console.WriteLine("Subject: " + msg.Subject);
                    Console.WriteLine("Body: " + msg.Body);
                    Console.WriteLine("Sender: " + msg.SenderName);
                    Console.WriteLine("Sent: " + msg.ClientSubmitTime);
                }
                else
                {
                    Console.WriteLine("The specified UNID was not found in the provided MSG file.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
