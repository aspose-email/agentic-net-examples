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
            string inputPath = "readonly.msg";

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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    try
                    {
                        FollowUpManager.SetFlag(message, "Follow up");
                    }
                    catch (Exception flagEx)
                    {
                        Console.Error.WriteLine($"Error setting follow‑up flag: {flagEx.Message}");
                        // Continue without rethrowing; the message may remain unchanged.
                    }

                    string outputPath = "updated.msg";
                    string outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        try
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }
                        catch (Exception dirEx)
                        {
                            Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                            return;
                        }
                    }

                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved to {outputPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Error saving message: {saveEx.Message}");
                    }
                }
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {loadEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
