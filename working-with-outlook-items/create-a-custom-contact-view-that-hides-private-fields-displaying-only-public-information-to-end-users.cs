using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Create a sample contact with various fields
            Contact contact = new Contact();
            contact.DisplayName = "John Doe";
            contact.CompanyName = "Acme Corp";
            contact.GivenName = "John";
            contact.Surname = "Doe";
            contact.EmailAddresses.Add(new EmailAddress("john.doe@acme.com"));
            contact.PhoneNumbers.Add(new PhoneNumber { Number = "+1-555-1234", Category = PhoneNumberCategory.Company });
            contact.Notes = "Private note that should not be shown to end users.";

            // Simulate a list of contacts (could be retrieved from a server in real scenarios)
            List<Contact> contacts = new List<Contact> { contact };

            // Display only public information
            foreach (Contact c in contacts)
            {
                Console.WriteLine("Display Name: " + c.DisplayName);
                Console.WriteLine("Company: " + c.CompanyName);
                Console.WriteLine("Email: " + (c.EmailAddresses.Count > 0 ? c.EmailAddresses[0].Address : "N/A"));
                Console.WriteLine("Phone: " + (c.PhoneNumbers.Count > 0 ? c.PhoneNumbers[0].Number : "N/A"));
                // Private fields such as Notes are intentionally omitted
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
