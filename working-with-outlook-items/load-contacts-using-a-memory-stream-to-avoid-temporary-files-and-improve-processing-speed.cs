using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Sample VCF contact data
            string vcfData = "BEGIN:VCARD\r\nVERSION:3.0\r\nFN:John Doe\r\nEMAIL:john.doe@example.com\r\nEND:VCARD";
            byte[] vcfBytes = Encoding.UTF8.GetBytes(vcfData);

            using (MemoryStream memoryStream = new MemoryStream(vcfBytes))
            {
                Contact contact = Contact.Load(memoryStream, ContactLoadFormat.VCard);
                Console.WriteLine("Contact loaded:");
                Console.WriteLine($"Full Name: {contact.DisplayName}");
                if (contact.EmailAddresses.Count > 0)
                {
                    Console.WriteLine($"Email: {contact.EmailAddresses[0].Address}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
