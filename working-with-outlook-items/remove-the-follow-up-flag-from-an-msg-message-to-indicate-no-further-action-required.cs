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
                using (MapiMessage placeholder = new MapiMessage(
                    "from@example.com",
                    "to@example.com",
                    "Sample Message",
                    "This is a placeholder message."))
                {
                    placeholder.Save(msgPath, SaveOptions.DefaultMsgUnicode);
                }
            }

            // Load the MSG message, clear the follow‑up flag, and save the changes.
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                FollowUpManager.ClearFlag(message);
                message.Save(msgPath, SaveOptions.DefaultMsgUnicode);
                Console.WriteLine("Follow‑up flag cleared successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
