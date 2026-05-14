using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare a list of contacts (placeholder data)
            List<MapiContact> contacts = new List<MapiContact>();

            // Contact 1
            MapiContact contact1 = new MapiContact();
            contact1.NameInfo.GivenName = "John";
            contact1.NameInfo.Surname = "Doe";
            contact1.NameInfo.DisplayName = "John Doe";
            contacts.Add(contact1);

            // Contact 2
            MapiContact contact2 = new MapiContact();
            contact2.NameInfo.GivenName = "Alice";
            contact2.NameInfo.Surname = "Smith";
            contact2.NameInfo.DisplayName = "Alice Smith";
            contacts.Add(contact2);

            // Contact 3
            MapiContact contact3 = new MapiContact();
            contact3.NameInfo.GivenName = "Bob";
            contact3.NameInfo.Surname = "Anderson";
            contact3.NameInfo.DisplayName = "Bob Anderson";
            contacts.Add(contact3);

            // Sort contacts by last name (Surname) alphabetically
            List<MapiContact> sortedContacts = contacts
                .OrderBy(c => c.NameInfo != null && c.NameInfo.Surname != null ? c.NameInfo.Surname : string.Empty)
                .ToList();

            // Destination folder for vCard files
            string outputFolder = Path.Combine(Environment.CurrentDirectory, "SortedContacts");

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Save each sorted contact to a separate vCard file
            foreach (MapiContact sortedContact in sortedContacts)
            {
                // Build file path using the contact's display name (fallback to index if empty)
                string safeFileName = string.IsNullOrWhiteSpace(sortedContact.NameInfo?.DisplayName)
                    ? Guid.NewGuid().ToString()
                    : sortedContact.NameInfo.DisplayName.Replace(' ', '_');

                string vcardPath = Path.Combine(outputFolder, safeFileName + ".vcf");

                // Guard file write operation
                try
                {
                    using (MapiContact disposableContact = sortedContact)
                    {
                        disposableContact.Save(vcardPath);
                    }
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save contact '{safeFileName}': {saveEx.Message}");
                }
            }

            Console.WriteLine("Contacts have been sorted and saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
