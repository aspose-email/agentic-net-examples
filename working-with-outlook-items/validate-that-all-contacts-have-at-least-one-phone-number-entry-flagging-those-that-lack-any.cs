using System;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Create sample contacts
            Contact contactWithPhone = new Contact();
            contactWithPhone.DisplayName = "Alice Johnson";
            PhoneNumber phoneAlice = new PhoneNumber
            {
                Number = "+1-555-0100",
                Category = PhoneNumberCategory.Company
            };
            contactWithPhone.PhoneNumbers.Add(phoneAlice);

            Contact contactWithoutPhone = new Contact();
            contactWithoutPhone.DisplayName = "Bob Smith";

            // Store contacts in an array
            Contact[] contacts = new Contact[] { contactWithPhone, contactWithoutPhone };

            // Validate that each contact has at least one phone number
            foreach (Contact currentContact in contacts)
            {
                if (currentContact.PhoneNumbers.Count == 0)
                {
                    Console.WriteLine($"Contact \"{currentContact.DisplayName}\" does not have any phone numbers.");
                }
                else
                {
                    Console.WriteLine($"Contact \"{currentContact.DisplayName}\" has {currentContact.PhoneNumbers.Count} phone number(s).");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
