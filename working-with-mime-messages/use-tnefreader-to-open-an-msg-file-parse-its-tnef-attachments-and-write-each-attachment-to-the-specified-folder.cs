using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

namespace AsposeEmailTnefExtraction
{
    // Author: Aspose.Email example for extracting attachments from an MSG file.
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Validate arguments.
                if (args.Length < 2)
                {
                    Console.Error.WriteLine("Usage: <program> <msgFilePath> <outputFolderPath>");
                    return;
                }

                string msgFilePath = args[0];
                string outputFolderPath = args[1];

                // Guard file input.
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

                    Console.Error.WriteLine($"Message file not found: {msgFilePath}");
                    return;
                }

                // Ensure output directory exists.
                if (!Directory.Exists(outputFolderPath))
                {
                    Directory.CreateDirectory(outputFolderPath);
                }

                // Load the MSG file and extract attachments.
                using (MapiMessage message = MapiMessage.Load(msgFilePath))
                {
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        string destinationPath = Path.Combine(outputFolderPath, attachment.FileName);
                        attachment.Save(destinationPath);
                        Console.WriteLine($"Saved attachment: {destinationPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
