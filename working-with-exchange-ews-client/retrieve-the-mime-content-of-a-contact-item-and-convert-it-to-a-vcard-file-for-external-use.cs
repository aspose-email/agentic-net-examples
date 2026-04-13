using Aspose.Email.Clients.Exchange.WebService;
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
            // Exchange server connection details
            string mailboxUri = "https://mail.example.com/exchange";
            string username = "username";
            string password = "password";

            // URI of the contact item on the server
            string contactUri = "/contacts/12345";

            // Destination vCard file path
            string vcardPath = "contact.vcf";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure the output directory exists
            string vcardDir = Path.GetDirectoryName(vcardPath);
            if (!string.IsNullOrEmpty(vcardDir) && !Directory.Exists(vcardDir))
            {
                Directory.CreateDirectory(vcardDir);
            }

            // Create and use the Exchange client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Fetch the contact as a MIME message
                    using (MailMessage mimeMessage = client.FetchMessage(contactUri))
                    {
                        // Convert MIME to MAPI message
                        MapiMessage mapiMessage = MapiMessage.FromMailMessage(mimeMessage);

                        // Verify the item is a contact
                        if (mapiMessage.SupportedType == MapiItemType.Contact)
                        {
                            // Convert to MapiContact
                            MapiContact contact = (MapiContact)mapiMessage.ToMapiMessageItem();

                            // Save the contact as a vCard file
                            contact.Save(vcardPath, ContactSaveFormat.VCard);
                            Console.WriteLine("Contact saved as vCard to: " + vcardPath);
                        }
                        else
                        {
                            Console.Error.WriteLine("The fetched item is not a contact.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during contact retrieval or conversion: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
            return;
        }
    }
}
