using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "Sample body"))
                {
                    placeholder.Save(inputPath);
                }
            }

            // Lookup table for Reply-To address replacement
            Dictionary<string, string> replyLookup = new Dictionary<string, string>();
            replyLookup.Add("old@example.com", "new@example.com");
            replyLookup.Add("another@example.com", "updated@example.com");

            // Load the message, update ReplyTo, and save changes
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                string currentReplyTo = message.ReplyTo;

                if (replyLookup.TryGetValue(currentReplyTo, out string newReplyTo))
                {
                    message.ReplyTo = newReplyTo;
                }
                else
                {
                    // If no mapping found, set a default Reply-To address
                    message.ReplyTo = "default@example.com";
                }

                message.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
