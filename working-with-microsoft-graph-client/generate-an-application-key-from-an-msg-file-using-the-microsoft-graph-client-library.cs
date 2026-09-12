using Aspose.Email;
using System;
using System.IO;
using System.Security.Cryptography;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        const string msgFilePath = "sample.msg";

        // Ensure the MSG file exists; create a minimal placeholder if missing.
        try
        {
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                // Create an empty MSG file placeholder.
                File.WriteAllBytes(msgFilePath, new byte[0]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to prepare the MSG file: {ex.Message}");
            return;
        }

        // Load the MSG file and generate a SHA‑256 key.
        try
        {
            // Load the Outlook message.
            MapiMessage msg = MapiMessage.Load(msgFilePath);

            // Obtain the raw bytes of the message file.
            byte[] rawBytes = File.ReadAllBytes(msgFilePath);

            // Compute SHA‑256 hash.
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(rawBytes);
                string key = BitConverter.ToString(hash).Replace("-", string.Empty);
                Console.WriteLine($"Application key (SHA‑256) for '{msgFilePath}': {key}");
            }

            // Optionally, you can use properties of the loaded message, e.g.:
            // Console.WriteLine($"Subject: {msg.Subject}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing the MSG file: {ex.Message}");
        }
    }
}
