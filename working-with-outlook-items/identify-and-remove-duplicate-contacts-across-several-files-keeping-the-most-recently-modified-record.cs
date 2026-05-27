using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input contact files (VCard format). Adjust paths as needed.
            string[] inputFiles = new string[]
            {
                "contact1.vcf",
                "contact2.vcf",
                "contact3.vcf"
            };

            // Ensure input files exist; create minimal placeholder if missing.
            foreach (string filePath in inputFiles)
            {
                if (!File.Exists(filePath))
                {
                    string placeholderVCard = "BEGIN:VCARD\r\nVERSION:2.1\r\nFN:Placeholder\r\nEND:VCARD";
                    File.WriteAllText(filePath, placeholderVCard);
                }
            }

            // Load all contacts from the files.
            List<MapiContact> allContacts = new List<MapiContact>();
            foreach (string filePath in inputFiles)
            {
                try
                {
                    MapiContact contact = MapiContact.FromVCard(filePath);
                    allContacts.Add(contact);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load contact from '{filePath}': {ex.Message}");
                }
            }

            // Deduplicate contacts based on primary email (Email1). Keep the most recently modified.
            Dictionary<string, MapiContact> uniqueContacts = new Dictionary<string, MapiContact>(StringComparer.OrdinalIgnoreCase);
            foreach (MapiContact contact in allContacts)
            {
                string email = contact.ElectronicAddresses?.Email1?.EmailAddress ?? string.Empty;
                if (string.IsNullOrWhiteSpace(email))
                    continue;

                // Retrieve the last modification time via PR_LAST_MODIFICATION_TIME (0x3008).
                DateTime contactModTime = DateTime.MinValue;
                try
                {
                    if (contact.TryGetPropertyDateTime(0x3008, ref contactModTime))
                    {
                        // value already assigned to contactModTime
                    }
                }
                catch { }

                if (uniqueContacts.TryGetValue(email, out MapiContact existingContact))
                {
                    DateTime existingModTime = DateTime.MinValue;
                    try
                    {
                        if (existingContact.TryGetPropertyDateTime(0x3008, ref existingModTime))
                        {
                            // value already assigned
                        }
                    }
                    catch { }

                    if (contactModTime > existingModTime)
                    {
                        uniqueContacts[email] = contact;
                    }
                }
                else
                {
                    uniqueContacts[email] = contact;
                }
            }

            // Prepare output directory.
            string outputDir = "DedupedContacts";
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Save deduplicated contacts as VCard files.
            foreach (KeyValuePair<string, MapiContact> kvp in uniqueContacts)
            {
                string email = kvp.Key;
                MapiContact contact = kvp.Value;
                string safeFileName = $"{Guid.NewGuid()}.vcf";
                string outputPath = Path.Combine(outputDir, safeFileName);
                try
                {
                    contact.Save(outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save contact '{email}' to '{outputPath}': {ex.Message}");
                }
            }

            Console.WriteLine($"Deduplication complete. {uniqueContacts.Count} unique contacts saved to '{outputDir}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
