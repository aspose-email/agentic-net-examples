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
            string inputPath = "input.msg";
            string outputPath = "draft.msg";

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

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            using (MapiMessage originalMessage = MapiMessage.Load(inputPath))
            {
                using (MapiMessage draftMessage = originalMessage.Clone() as MapiMessage)
                {
                    if (draftMessage == null)
                    {
                        Console.Error.WriteLine("Failed to clone the message.");
                        return;
                    }

                    draftMessage.Subject = "Draft: " + draftMessage.Subject;

                    try
                    {
                        draftMessage.Save(outputPath);
                        Console.WriteLine($"Draft message saved to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save draft: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
