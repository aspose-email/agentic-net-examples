using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            string vcfPath = "distributionList.vcf";

            if (!File.Exists(vcfPath))
            {
                Console.Error.WriteLine($"File not found: {vcfPath}");
                return;
            }

            // Load the distribution list from the VCF file
            using (MapiDistributionList distributionList = MapiDistributionList.FromVCF(vcfPath))
            {
                List<Contact> contacts = new List<Contact>();

                foreach (MapiDistributionListMember member in distributionList.Members)
                {
                    Contact contact = new Contact
                    {
                        DisplayName = member.DisplayName
                    };
                    contact.EmailAddresses.Add(new EmailAddress(member.EmailAddress));
                    contacts.Add(contact);
                }

                // Output the created contacts
                foreach (Contact contact in contacts)
                {
                    string email = contact.EmailAddresses.Count > 0 ? contact.EmailAddresses[0].Address : "N/A";
                    Console.WriteLine($"Name: {contact.DisplayName}, Email: {email}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
