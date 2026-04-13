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
            string msgPath = "input.msg";
            string emlPath = "output.eml";

            // Verify input file exists
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

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(emlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Set priority to High (value 2)
                msg.SetProperty(new MapiProperty(KnownPropertyList.Priority, 2));

                // Verify the priority was set using the required Tag
                int priorityValue = 0;
                if (msg.TryGetPropertyInt32(KnownPropertyList.Priority.Tag, ref priorityValue))
                {
                    Console.WriteLine($"Priority set to: {priorityValue}");
                }

                // Convert to MailMessage
                MailConversionOptions convOptions = new MailConversionOptions();
                using (MailMessage mail = msg.ToMailMessage(convOptions))
                {
                    // Save as EML
                    mail.Save(emlPath);
                    Console.WriteLine($"Converted MSG to EML: {emlPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
