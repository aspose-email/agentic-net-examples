using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for PST and source email
            string pstPath = "sample.pst";
            string emlPath = "sample.eml";

            // Ensure PST file exists; create if missing
            if (!File.Exists(pstPath))
            {
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            // Open PST for read/write
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get or create an Inbox folder inside the PST
                FolderInfo inbox;
                try
                {
                    inbox = pst.RootFolder.GetSubFolder("Inbox");
                }
                catch
                {
                    inbox = null;
                }

                if (inbox == null)
                {
                    inbox = pst.RootFolder.AddSubFolder("Inbox");
                }

                // Load the source email; create a placeholder if the file is absent
                MailMessage mail;
                if (File.Exists(emlPath))
                {
                    mail = MailMessage.Load(emlPath);
                }
                else
                {
                    mail = new MailMessage("sender@example.com", "receiver@example.com", "Sample Subject", "Sample body");
                }

                // Process attachments: compress those larger than 10 MB using GZIP
                const long tenMb = 10L * 1024 * 1024;
                List<Attachment> processedAttachments = new List<Attachment>();

                foreach (Attachment att in mail.Attachments)
                {
                    using (MemoryStream originalStream = new MemoryStream())
                    {
                        att.ContentStream.CopyTo(originalStream);
                        byte[] data = originalStream.ToArray();

                        if (data.Length > tenMb)
                        {
                            // Compress large attachment
                            using (MemoryStream compressedStream = new MemoryStream())
                            {
                                using (GZipStream gzip = new GZipStream(compressedStream, CompressionMode.Compress, true))
                                {
                                    gzip.Write(data, 0, data.Length);
                                }
                                compressedStream.Position = 0;
                                Attachment compressedAtt = new Attachment(compressedStream, att.Name + ".gz");
                                processedAttachments.Add(compressedAtt);
                            }
                        }
                        else
                        {
                            // Keep small attachment unchanged
                            Attachment originalAtt = new Attachment(new MemoryStream(data), att.Name);
                            processedAttachments.Add(originalAtt);
                        }
                    }
                }

                // Replace original attachments with processed ones
                mail.Attachments.Clear();
                foreach (Attachment a in processedAttachments)
                {
                    mail.Attachments.Add(a);
                }

                // Convert MailMessage to MapiMessage and add to PST
                MapiMessage mapiMsg = MapiMessage.FromMailMessage(mail);
                inbox.AddMessage(mapiMsg);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return;
        }
    }
}
