using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string msgPath = "input.msg";

            // Output directory for extracted vCard files
            string outputDir = "output";

            // Guard input file existence
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Iterate through attachments looking for .vcf files
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    if (attachment.FileName != null &&
                        attachment.FileName.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase))
                    {
                        // Save attachment to a memory stream
                        using (MemoryStream vcfStream = new MemoryStream())
                        {
                            attachment.Save(vcfStream);
                            vcfStream.Position = 0;

                            // Load the vCard as a MapiContact
                            MapiContact contact = MapiContact.FromVCard(vcfStream);

                            // Display some contact information
                            Console.WriteLine($"Contact Name: {contact.NameInfo.DisplayName}");
                            if (contact.ElectronicAddresses.Email1 != null)
                            {
                                Console.WriteLine($"Email: {contact.ElectronicAddresses.Email1.EmailAddress}");
                            }

                            // Save the vCard to the output directory
                            string outPath = Path.Combine(outputDir, attachment.FileName);
                            contact.Save(outPath);
                            Console.WriteLine($"Saved vCard to: {outPath}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
