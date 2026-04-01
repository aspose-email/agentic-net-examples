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
            string inputPath = "encrypted.msg";
            string outputPath = "decrypted.txt";

            // Ensure input file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    using (MapiMessage placeholder = new MapiMessage("Placeholder Subject", "Placeholder Body", "Sender Name", "sender@example.com"))
                    {
                        placeholder.Save(inputPath);
                        Console.WriteLine($"Placeholder MSG created at '{inputPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file.
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Check if the message is encrypted.
                if (!msg.IsEncrypted)
                {
                    Console.WriteLine("The message is not encrypted. No decryption needed.");
                }
                else
                {
                    // Decrypt the message.
                    using (MapiMessage decryptedMsg = msg.Decrypt())
                    {
                        // Retrieve the body content.
                        string body = decryptedMsg.Body ?? string.Empty;

                        // Ensure the output directory exists.
                        try
                        {
                            string outputDir = Path.GetDirectoryName(outputPath);
                            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                            {
                                Directory.CreateDirectory(outputDir);
                            }

                            // Write the decrypted content to a file.
                            File.WriteAllText(outputPath, body);
                            Console.WriteLine($"Decrypted content saved to '{outputPath}'.");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Failed to write decrypted content: {ioEx.Message}");
                            return;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
