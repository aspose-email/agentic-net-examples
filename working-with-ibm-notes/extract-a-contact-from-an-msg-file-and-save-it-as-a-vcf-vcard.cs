using System;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo;

class ExtractContactFromMsg
{
    static void Main()
    {
        try
        {
            string msgFilePath = "contact.msg";
            string vcardPath = "extracted_contact.vcf";

            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
                Console.Error.WriteLine($"File not found: {msgFilePath}");
                return;
            }

            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                if (msg.SupportedType != MapiItemType.Contact)
                {
                    Console.WriteLine("The provided MSG file does not contain a contact.");
                    return;
                }

                using (MapiContact mapiContact = (MapiContact)msg.ToMapiMessageItem())
                {
                    Contact contact = new Contact();
                    contact.DisplayName = mapiContact.NameInfo?.DisplayName;
                    contact.CompanyName = mapiContact.Companies?.FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(mapiContact.ElectronicAddresses?.Email1?.EmailAddress))
                    {
                        contact.EmailAddresses.Add(new EmailAddress(mapiContact.ElectronicAddresses.Email1.EmailAddress));
                    }
                    if (!string.IsNullOrWhiteSpace(mapiContact.ElectronicAddresses?.Email2?.EmailAddress))
                    {
                        contact.EmailAddresses.Add(new EmailAddress(mapiContact.ElectronicAddresses.Email2.EmailAddress));
                    }
                    if (!string.IsNullOrWhiteSpace(mapiContact.ElectronicAddresses?.Email3?.EmailAddress))
                    {
                        contact.EmailAddresses.Add(new EmailAddress(mapiContact.ElectronicAddresses.Email3.EmailAddress));
                    }

                    if (!string.IsNullOrWhiteSpace(mapiContact.Telephones?.PrimaryTelephoneNumber))
                    {
                        contact.PhoneNumbers.Add(new PhoneNumber
                        {
                            Number = mapiContact.Telephones.PrimaryTelephoneNumber,
                            Category = PhoneNumberCategory.Primary
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(mapiContact.Telephones?.BusinessTelephoneNumber))
                    {
                        contact.PhoneNumbers.Add(new PhoneNumber
                        {
                            Number = mapiContact.Telephones.BusinessTelephoneNumber,
                            Category = PhoneNumberCategory.Company
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(mapiContact.Telephones?.HomeTelephoneNumber))
                    {
                        contact.PhoneNumbers.Add(new PhoneNumber
                        {
                            Number = mapiContact.Telephones.HomeTelephoneNumber,
                            Category = PhoneNumberCategory.Home
                        });
                    }

                    contact.Save(vcardPath);
                    Console.WriteLine($"Contact extracted and saved to '{vcardPath}'.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
