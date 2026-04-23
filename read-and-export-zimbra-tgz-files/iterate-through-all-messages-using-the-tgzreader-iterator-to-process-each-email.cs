using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string tgzPath = "mailbox.tgz";

            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Input file not found: {tgzPath}");
                return;
            }

            string outputDirectory = "output";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            using (TgzReader reader = new TgzReader(tgzPath))
            {
                int totalItems;
                try
                {
                    totalItems = reader.GetTotalItemsCount();
                }
                catch (Exception countEx)
                {
                    Console.Error.WriteLine($"Failed to get total items count: {countEx.Message}");
                    return;
                }

                for (int i = 0; i < totalItems; i++)
                {
                    try
                    {
                        reader.ReadNextMessage();
                    }
                    catch (Exception readEx)
                    {
                        Console.Error.WriteLine($"Error reading message #{i}: {readEx.Message}");
                        continue;
                    }

                    MailMessage currentMessage = reader.CurrentMessage;
                    if (currentMessage == null)
                    {
                        continue;
                    }

                    Console.WriteLine($"Subject: {currentMessage.Subject}");
                    string safeSubject = string.IsNullOrWhiteSpace(currentMessage.Subject) ? $"Message_{i}" : currentMessage.Subject;
                    // Replace invalid filename characters
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        safeSubject = safeSubject.Replace(c, '_');
                    }
                    string outputPath = Path.Combine(outputDirectory, $"{safeSubject}.eml");

                    try
                    {
                        currentMessage.Save(outputPath);
                        Console.WriteLine($"Saved to {outputPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message #{i}: {saveEx.Message}");
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
