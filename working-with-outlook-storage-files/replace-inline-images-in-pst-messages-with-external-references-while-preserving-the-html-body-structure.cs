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
            string pstPath = "sample.pst";
            string outputFolder = "output";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                    return;
                }
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                ProcessFolder(pst, pst.RootFolder, outputFolder);
                foreach (FolderInfo subFolder in pst.RootFolder.GetSubFolders())
                {
                    ProcessFolder(pst, subFolder, outputFolder);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputFolder)
    {
        try
        {
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                {
                    if (string.IsNullOrEmpty(mapiMessage.BodyHtml))
                        continue;

                    string htmlBody = mapiMessage.BodyHtml;
                    bool modified = false;

                    foreach (MapiAttachment attachment in mapiMessage.Attachments)
                    {
                        // Use FileName as identifier; if not present, skip
                        string identifier = attachment.FileName;
                        if (string.IsNullOrEmpty(identifier))
                            continue;

                        string safeFileName = identifier;
                        string externalPath = Path.Combine(outputFolder, safeFileName);

                        try
                        {
                            using (FileStream fs = new FileStream(externalPath, FileMode.Create, FileAccess.Write))
                            {
                                attachment.Save(fs);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{safeFileName}': {ex.Message}");
                            continue;
                        }

                        // Replace CID reference (cid:filename) with external file path
                        string cidReference = $"cid:{identifier}";
                        if (htmlBody.Contains(cidReference))
                        {
                            htmlBody = htmlBody.Replace(cidReference, externalPath);
                            modified = true;
                        }
                    }

                    if (modified)
                    {
                        mapiMessage.SetBodyContent(htmlBody, BodyContentType.Html);
                        folder.UpdateMessage(messageInfo.EntryIdString, mapiMessage);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to process folder '{folder.DisplayName}': {ex.Message}");
        }
    }
}
