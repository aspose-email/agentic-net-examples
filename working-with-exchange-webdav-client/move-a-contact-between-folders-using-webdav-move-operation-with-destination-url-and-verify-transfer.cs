using Aspose.Email.PersonalInfo;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string serverUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (serverUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Source and destination folder URIs (WebDAV format).
            string sourceFolderUri = "/exchange/user@example.com/contacts/SourceFolder";
            string destinationFolderUri = "/exchange/user@example.com/contacts/DestinationFolder";

            using (ExchangeClient client = new ExchangeClient(serverUri, username, password))
            {
                // List contacts in the source folder.
                MapiContact[] sourceContacts = client.ListContacts(sourceFolderUri);
                if (sourceContacts == null || sourceContacts.Length == 0)
                {
                    Console.Error.WriteLine("No contacts found in the source folder.");
                    return;
                }

                // Take the first contact for demonstration.
                MapiContact contactToMove = sourceContacts[0];
                string contactItemId = contactToMove.ItemId;

                // Perform the MOVE operation.
                try
                {
                    client.MoveItems(destinationFolderUri, contactItemId);
                    Console.WriteLine($"Contact '{contactToMove.NameInfo.DisplayName}' moved successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to move contact: {ex.Message}");
                    return;
                }

                // Verify the contact now exists in the destination folder.
                MapiContact[] destContacts = client.ListContacts(destinationFolderUri);
                bool found = false;
                foreach (MapiContact c in destContacts)
                {
                    if (c.ItemId == contactItemId)
                    {
                        found = true;
                        break;
                    }
                }

                Console.WriteLine(found
                    ? "Verification succeeded: contact is present in the destination folder."
                    : "Verification failed: contact not found in the destination folder.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
