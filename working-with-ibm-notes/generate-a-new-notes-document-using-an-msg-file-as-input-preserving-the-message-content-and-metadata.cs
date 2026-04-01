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
            string outputPath = "output.msg";

            // Ensure input MSG exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    MailMessage placeholder = new MailMessage
                    {
                        Subject = "Placeholder Subject",
                        Body = "This is a placeholder message."
                    };
                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                    placeholder.Save(inputPath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file into a MapiMessage
            MapiMessage mapMessage;
            try
            {
                mapMessage = MapiMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            // Preserve content and metadata by saving to a new file
            try
            {
                using (mapMessage)
                {
                    mapMessage.Save(outputPath);
                }
                Console.WriteLine($"Notes document created successfully at '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving Notes document: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
