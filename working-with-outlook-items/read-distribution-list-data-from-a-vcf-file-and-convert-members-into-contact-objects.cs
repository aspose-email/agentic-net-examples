using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            string vcfPath = "distributionlist.vcf";

            if (!File.Exists(vcfPath))
            {
                Console.Error.WriteLine($"VCF file not found: {vcfPath}");
                return;
            }

            using (MapiDistributionList distributionList = MapiDistributionList.FromVCF(vcfPath))
            {
                Console.WriteLine($"Distribution List: {distributionList.DisplayName}");

                List<Contact> contacts = new List<Contact>();

                foreach (MapiDistributionListMember member in distributionList.Members)
                {
                    Contact contact = new Contact();
                    contact.DisplayName = string.IsNullOrEmpty(member.DisplayName) ? member.EmailAddress : member.DisplayName;
                    contact.EmailAddresses.Add(new EmailAddress(member.EmailAddress));
                    contacts.Add(contact);
                }

                foreach (Contact contact in contacts)
                {
                    string email = contact.EmailAddresses.Count > 0 ? contact.EmailAddresses[0].Address : "N/A";
                    Console.WriteLine($"Contact: {contact.DisplayName}, Email: {email}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
