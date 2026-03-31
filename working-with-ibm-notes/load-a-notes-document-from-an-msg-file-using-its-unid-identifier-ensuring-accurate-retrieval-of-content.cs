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
            const string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Sample Subject",
                        "This is a placeholder message body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file.
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Retrieve the UNID property (PR_NOTE_UNID = 0x0E0F). It is stored as binary.
                const long UnidTag = 0x0E0F;
                byte[] unidBytes = message.GetPropertyBytes(UnidTag);
                string unidHex = unidBytes != null ? BitConverter.ToString(unidBytes).Replace("-", "") : "N/A";

                Console.WriteLine($"UNID: {unidHex}");
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"Body: {message.Body}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
