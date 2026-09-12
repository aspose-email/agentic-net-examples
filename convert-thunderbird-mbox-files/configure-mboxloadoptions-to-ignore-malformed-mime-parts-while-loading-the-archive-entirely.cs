using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxPath = "storage.mbox";

            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Create load options (default behavior loads the entire MBOX and ignores malformed MIME parts).
            MboxLoadOptions loadOptions = new MboxLoadOptions();

            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                int messageIndex = 0;
                while (true)
                {
                    MailMessage eml;
                    try
                    {
                        eml = mboxReader.ReadNextMessage();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error reading message #{messageIndex}: {ex.Message}");
                        break;
                    }

                    if (eml == null)
                        break;

                    using (eml)
                    {
                        string safeSubject = string.IsNullOrWhiteSpace(eml.Subject) ? "Untitled" : eml.Subject;
                        string outputFileName = $"{SanitizeFileName(safeSubject)}.eml";

                        string outputDirectory = Path.GetDirectoryName(outputFileName);
                        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }

                        try
                        {
                            eml.Save(outputFileName);
                            Console.WriteLine($"Saved: {outputFileName}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving message #{messageIndex}: {ex.Message}");
                        }
                    }

                    messageIndex++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static string SanitizeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}
