using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // List of MBOX files to process (replace with actual paths)
            string[] mboxFiles = new string[] { "mail1.mbox", "mail2.mbox" };

            // Dictionary to group contacts by organization (domain part of email)
            Dictionary<string, List<Contact>> contactsByOrganization = new Dictionary<string, List<Contact>>(StringComparer.OrdinalIgnoreCase);

            foreach (string mboxPath in mboxFiles)
            {
                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    continue;
                }

                try
                {
                    // Convert MBOX to PST in memory
                    using (MemoryStream pstStream = new MemoryStream())
                    {
                        MailStorageConverter.MboxToPst(mboxPath, pstStream);
                        pstStream.Position = 0;

                        // Open PST from the memory stream
                        using (PersonalStorage pst = PersonalStorage.FromStream(pstStream))
                        {
                            // Enumerate all messages in the root folder
                            foreach (MessageInfo messageInfo in pst.RootFolder.EnumerateMessages())
                            {
                                using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                                {
                                    // Process sender
                                    ProcessEmailAddress(mapiMessage.SenderEmailAddress, mapiMessage.SenderName, contactsByOrganization);

                                    // Process recipients
                                    foreach (MapiRecipient recipient in mapiMessage.Recipients)
                                    {
                                        ProcessEmailAddress(recipient.EmailAddress, recipient.DisplayName, contactsByOrganization);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{mboxPath}': {ex.Message}");
                }
            }

            // Save contacts grouped by organization into separate VCF files
            foreach (KeyValuePair<string, List<Contact>> kvp in contactsByOrganization)
            {
                string organization = kvp.Key;
                List<Contact> contacts = kvp.Value;

                string outputDirectory = Path.Combine("output", organization);
                try
                {
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {ex.Message}");
                    continue;
                }

                foreach (Contact contact in contacts)
                {
                    string safeName = string.IsNullOrWhiteSpace(contact.DisplayName) ? "Contact" : contact.DisplayName;
                    // Replace invalid filename characters
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        safeName = safeName.Replace(invalidChar, '_');
                    }

                    string vcfPath = Path.Combine(outputDirectory, $"{safeName}.vcf");
                    try
                    {
                        contact.Save(vcfPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save contact '{safeName}' to '{vcfPath}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }

    // Helper method to create a Contact from an email address and add it to the grouping dictionary
    private static void ProcessEmailAddress(string email, string displayName, Dictionary<string, List<Contact>> contactsByOrganization)
    {
        if (string.IsNullOrWhiteSpace(email))
            return;

        Contact contact = new Contact();
        contact.DisplayName = string.IsNullOrWhiteSpace(displayName) ? email : displayName;
        contact.EmailAddresses.Add(new EmailAddress(email, contact.DisplayName));

        // Derive organization from email domain (e.g., user@company.com -> company.com)
        string organization = "Unknown";
        int atIndex = email.IndexOf('@');
        if (atIndex > -1 && atIndex < email.Length - 1)
        {
            organization = email.Substring(atIndex + 1);
        }

        if (!contactsByOrganization.ContainsKey(organization))
        {
            contactsByOrganization[organization] = new List<Contact>();
        }
        contactsByOrganization[organization].Add(contact);
    }
}
