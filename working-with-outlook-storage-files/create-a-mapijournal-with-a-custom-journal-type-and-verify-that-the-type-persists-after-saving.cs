using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string filePath = "journal.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a MapiJournal with a custom message class and save it
            using (MapiJournal journal = new MapiJournal())
            {
                journal.Subject = "Test Journal";
                journal.Body = "Journal body content.";
                journal.Description = "Test description.";
                journal.BriefDescription = "Brief description.";
                journal.MessageClass = "IPM.Journal.Custom";
                journal.Save(filePath);
            }

            // Verify that the file was created
            if (!File.Exists(filePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine("Journal file was not created.");
                return;
            }

            // Load the saved journal and check the custom message class
            using (MapiMessage loadedMessage = MapiMessage.Load(filePath))
            {
                if (loadedMessage.SupportedType != MapiItemType.Journal)
                {
                    Console.Error.WriteLine("Loaded item is not a journal.");
                    return;
                }

                MapiJournal loadedJournal = (MapiJournal)loadedMessage.ToMapiMessageItem();
                bool typeMatches = string.Equals(loadedJournal.MessageClass, "IPM.Journal.Custom", StringComparison.OrdinalIgnoreCase);
                Console.WriteLine("Custom journal type persisted: " + typeMatches);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
