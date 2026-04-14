using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Tools;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "original.msg";
            string outputPath = "reply.msg";

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

            using (MapiMessage original = MapiMessage.Load(inputPath))
            {
                ReplyMessageBuilder builder = new ReplyMessageBuilder();
                builder.ResponseText = "Thank you for your email.";

                using (MapiMessage reply = builder.BuildResponse(original))
                {
                    try
                    {
                        reply.Save(outputPath);
                        Console.WriteLine($"Reply saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save reply: {ex.Message}");
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
