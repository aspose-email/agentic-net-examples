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
            // Path to the Outlook MSG file
            string msgPath = "sample.msg";

            // Verify the input file exists
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file inside a try/catch to handle parsing errors
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(msgPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {loadEx.Message}");
                return;
            }

            // Define prohibited characters for attachment filenames
            char[] prohibitedChars = new char[] { '\\', '/', '<', '>', '|', ':', '*', '?', '\"' };

            // Iterate through each attachment and validate its filename
            foreach (MapiAttachment attachment in message.Attachments)
            {
                string fileName = attachment.FileName ?? string.Empty;

                bool containsProhibited = false;
                foreach (char ch in prohibitedChars)
                {
                    if (fileName.IndexOf(ch) >= 0)
                    {
                        containsProhibited = true;
                        break;
                    }
                }

                if (containsProhibited)
                {
                    Console.WriteLine($"Warning: Attachment \"{fileName}\" contains prohibited characters.");
                }
                else
                {
                    Console.WriteLine($"Attachment \"{fileName}\" is valid.");
                }
            }

            // Optional: Save attachments to a safe directory after validation
            string outputDir = "SafeAttachments";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            foreach (MapiAttachment attachment in message.Attachments)
            {
                string safeFileName = attachment.FileName ?? "unnamed";
                // Replace prohibited characters with underscore for safe saving
                foreach (char ch in prohibitedChars)
                {
                    safeFileName = safeFileName.Replace(ch, '_');
                }

                string savePath = Path.Combine(outputDir, safeFileName);
                try
                {
                    attachment.Save(savePath);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save attachment \"{attachment.FileName}\": {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
