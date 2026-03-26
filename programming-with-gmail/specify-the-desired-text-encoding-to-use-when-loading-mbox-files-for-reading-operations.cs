using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "sample.mbox";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Error: File not found – {mboxPath}");
                return;
            }

            // Set global load options for MBOX parsing
            MailStorageConverter.MboxMessageOptions = new EmlLoadOptions
            {
                PreferredTextEncoding = Encoding.UTF8
            };

            // Create a reader with specific load options
            using (FileStream fileStream = File.OpenRead(mboxPath))
            {
                MboxLoadOptions loadOptions = new MboxLoadOptions
                {
                    PreferredTextEncoding = Encoding.UTF8
                };

                using (MboxStorageReader reader = MboxStorageReader.CreateReader(fileStream, loadOptions))
                {
                    // Reader is ready to read messages with UTF-8 encoding
                    Console.WriteLine("MBOX file opened with UTF-8 encoding.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}