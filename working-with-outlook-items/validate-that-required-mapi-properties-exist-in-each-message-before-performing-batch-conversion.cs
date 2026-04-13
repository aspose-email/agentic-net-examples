using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolder = "InputMsgs";
            string outputFolder = "ConvertedPst";

            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            string pstPath = Path.Combine(outputFolder, "output.pst");

            if (!File.Exists(pstPath))
            {
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                FolderInfo rootFolder = pst.RootFolder;

                foreach (string msgFile in Directory.GetFiles(inputFolder, "*.msg"))
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

                        Console.Error.WriteLine($"Message file '{msgFile}' missing, skipping.");
                        continue;
                    }

                    using (MapiMessage msg = MapiMessage.Load(msgFile))
                    {
                        const long PR_SUBJECT = 0x0037001F;               // Subject string property
                        const long PR_SENDER_EMAIL_ADDRESS = 0x0C1F001F; // Sender email address string property

                        string subject = null;
                        string senderEmail = null;

                        bool hasSubject = msg.TryGetPropertyString(PR_SUBJECT, ref subject);
                        bool hasSender = msg.TryGetPropertyString(PR_SENDER_EMAIL_ADDRESS, ref senderEmail);

                        if (!hasSubject || !hasSender)
                        {
                            Console.WriteLine($"Message '{msgFile}' missing required properties, skipping.");
                            continue;
                        }

                        rootFolder.AddMessage(msg);
                        Console.WriteLine($"Added '{msgFile}' to PST.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
