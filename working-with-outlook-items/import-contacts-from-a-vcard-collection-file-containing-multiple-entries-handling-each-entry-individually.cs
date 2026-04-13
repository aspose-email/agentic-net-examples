using Aspose.Email.PersonalInfo;
using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.PersonalInfo.VCard;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string vcardFilePath = "contacts.vcf";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(vcardFilePath))
            {
                using (StreamWriter writer = new StreamWriter(vcardFilePath))
                {
                    writer.WriteLine("BEGIN:VCARD");
                    writer.WriteLine("VERSION:2.1");
                    writer.WriteLine("N:Doe;John;;;");
                    writer.WriteLine("FN:John Doe");
                    writer.WriteLine("END:VCARD");
                }
                Console.WriteLine($"Placeholder vCard file created at '{vcardFilePath}'.");
            }

            // Load all contacts from the multi‑contact vCard file
            List<VCardContact> contacts;
            using (FileStream fileStream = new FileStream(vcardFilePath, FileMode.Open, FileAccess.Read))
            {
                contacts = VCardContact.LoadAsMultiple(fileStream);
            }

            // Process each contact individually
            int contactIndex = 0;
            foreach (VCardContact vcard in contacts)
            {
                contactIndex++;
                Console.WriteLine($"Contact #{contactIndex} loaded.");
                // Additional processing of each VCardContact can be added here
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
