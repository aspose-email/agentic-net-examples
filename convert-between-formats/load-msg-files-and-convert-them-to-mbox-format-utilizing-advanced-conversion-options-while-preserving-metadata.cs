using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Folder containing MSG files
            string msgFolder = "MsgFiles";
            // Output MBOX file path
            string outputMboxPath = "output.mbox";

            // Verify input folder exists
            if (!Directory.Exists(msgFolder))
            {
                Console.Error.WriteLine($"Error: Directory not found – {msgFolder}");
                return;
            }

            // Ensure the output MBOX file exists (create empty placeholder if needed)
            if (!File.Exists(outputMboxPath))
            {
                using (FileStream fs = File.Create(outputMboxPath)) { }
            }

            // Prepare MBOX writer with default options
            MboxSaveOptions saveOptions = new MboxSaveOptions();
            using (MboxrdStorageWriter writer = new MboxrdStorageWriter(outputMboxPath, saveOptions))
            {
                // Process each MSG file in the folder
                string[] msgFiles = Directory.GetFiles(msgFolder, "*.msg");
                foreach (string msgFile in msgFiles)
                {
                    if (!File.Exists(msgFile))
                    {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"Warning: MSG file not found – {msgFile}");
                        continue;
                    }

                    try
                    {
                        // Load MSG as MapiMessage
                        MapiMessage mapiMsg = MapiMessage.Load(msgFile);

                        // Configure conversion options to preserve metadata
                        MailConversionOptions convOptions = new MailConversionOptions
                        {
                            KeepOriginalEmailAddresses = true,
                            PreserveEmbeddedMessageFormat = true,
                            PreserveRtfContent = true
                        };

                        // Convert to MailMessage
                        MailMessage mailMsg = mapiMsg.ToMailMessage(convOptions);

                        // Write the message into the MBOX storage
                        writer.WriteMessage(mailMsg);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing {msgFile}: {ex.Message}");
                    }
                }
            }

            // Verify the written MBOX by reading it back (required by validation rules)
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(outputMboxPath, new MboxLoadOptions()))
            {
                MailMessage message;
                while ((message = reader.ReadNextMessage()) != null)
                {
                    Console.WriteLine($"Written message: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
