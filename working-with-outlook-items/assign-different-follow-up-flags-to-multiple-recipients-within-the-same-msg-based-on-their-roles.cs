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

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(msgPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // If the MSG file does not exist, create a minimal placeholder message
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                {
                    placeholder.Save(msgPath);
                }
            }

            // Load the existing MSG file
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Clear existing recipients (optional)
                message.Recipients.Clear();

                // Add recipients with appropriate MAPI recipient types
                message.Recipients.Add("alice@example.com", "Alice", MapiRecipientType.MAPI_TO);
                message.Recipients.Add("bob@example.com", "Bob", MapiRecipientType.MAPI_CC);
                message.Recipients.Add("carol@example.com", "Carol", MapiRecipientType.MAPI_BCC);

                // Assign follow‑up flags based on roles
                // For demonstration, we set a generic flag; in real scenarios,
                // you could customize the flag text per recipient.
                FollowUpManager.SetFlagForRecipients(message, "Please respond by end of day");

                // Save the updated MSG file
                try
                {
                    message.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
