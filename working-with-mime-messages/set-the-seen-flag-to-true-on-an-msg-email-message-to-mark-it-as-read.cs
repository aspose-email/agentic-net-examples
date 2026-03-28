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
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage("Placeholder Subject", "Placeholder Body", "sender@example.com", "receiver@example.com"))
                {
                    placeholder.Save(msgPath);
                }
            }

            // Load the MSG file.
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Mark the message as read.
                message.SetMessageFlags(MapiMessageFlags.MSGFLAG_READ);

                // Save the updated message back to the same file.
                message.Save(msgPath);
            }

            Console.WriteLine("Message marked as read successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
