using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Mime;
using Aspose.Email.PersonalInfo.VCard;

namespace BatchMboxToVcf
{
    class Program
    {
        static void Main()
        {
            string inputFolder = "InputMbox";
            string outputFolder = "OutputVcf";

            try
            {
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);

                if (!Directory.Exists(inputFolder))
                {
                    Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                    return;
                }

                var contactsByOrg = new Dictionary<string, List<VCardContact>>(StringComparer.OrdinalIgnoreCase);

                foreach (string mboxPath in Directory.GetFiles(inputFolder, "*.mbox"))
                {
                    if (!File.Exists(mboxPath))
                    {
                        Console.Error.WriteLine($"MBOX file '{mboxPath}' not found, skipping.");
                        continue;
                    }

                    try
                    {
                        using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                        {
                            MailMessage message;
                            while ((message = reader.ReadNextMessage()) != null)
                            {
                                foreach (Attachment attachment in message.Attachments)
                                {
                                    if (attachment.Name != null && attachment.Name.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase))
                                    {
                                        using (var attachmentStream = attachment.ContentStream)
                                        {
                                            using (var ms = new MemoryStream())
                                            {
                                                attachmentStream.CopyTo(ms);
                                                ms.Position = 0;
                                                VCardContact vcard = VCardContact.Load(ms);
                                                string organization = vcard.Organization?.Organization ?? "Unknown";

                                                if (!contactsByOrg.TryGetValue(organization, out var list))
                                                {
                                                    list = new List<VCardContact>();
                                                    contactsByOrg[organization] = list;
                                                }
                                                list.Add(vcard);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing MBOX file '{mboxPath}': {ex.Message}");
                    }
                }

                foreach (var kvp in contactsByOrg)
                {
                    string orgName = kvp.Key;
                    List<VCardContact> orgContacts = kvp.Value;

                    string orgFolderPath = Path.Combine(outputFolder, orgName);
                    if (!Directory.Exists(orgFolderPath))
                        Directory.CreateDirectory(orgFolderPath);

                    foreach (VCardContact contact in orgContacts)
                    {
                        string vcfFilePath = Path.Combine(orgFolderPath, $"{Guid.NewGuid()}.vcf");
                        try
                        {
                            contact.Save(vcfFilePath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save VCF for organization '{orgName}': {ex.Message}");
                        }
                    }
                }

                Console.WriteLine("Batch conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
