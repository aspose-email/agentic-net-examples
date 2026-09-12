using System;
using System.IO;
using System.Text;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace MboxReflectionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                const string mboxPath = "storage.mbox";

                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    return;
                }

                // Discover MboxLoadOptions properties via reflection
                Type loadOptionsType = typeof(MboxLoadOptions);
                PropertyInfo[] properties = loadOptionsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                Console.WriteLine("Available MboxLoadOptions properties:");
                foreach (PropertyInfo prop in properties)
                {
                    Console.WriteLine($"- {prop.Name} ({prop.PropertyType.Name})");
                }

                // Create and configure MboxLoadOptions dynamically
                MboxLoadOptions loadOptions = new MboxLoadOptions();

                foreach (PropertyInfo prop in properties)
                {
                    if (prop.Name == "PreferredTextEncoding" && prop.CanWrite)
                    {
                        prop.SetValue(loadOptions, Encoding.UTF8);
                        Console.WriteLine("Set PreferredTextEncoding to UTF8.");
                    }
                }

                // Ensure output directory exists
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                // Read messages sequentially using MboxStorageReader
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
                {
                    int messageIndex = 0;
                    while (true)
                    {
                        MailMessage message = mboxReader.ReadNextMessage();
                        if (message == null)
                            break;

                        messageIndex++;
                        Console.WriteLine($"Message {messageIndex}:");
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");

                        string safeSubject = string.IsNullOrWhiteSpace(message.Subject)
                            ? $"Message_{messageIndex}"
                            : string.Join("_", message.Subject.Split(Path.GetInvalidFileNameChars()));

                        string outputFile = Path.Combine(outputDir, $"{safeSubject}.eml");
                        message.Save(outputFile);
                        Console.WriteLine($"Saved message to {outputFile}");
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
