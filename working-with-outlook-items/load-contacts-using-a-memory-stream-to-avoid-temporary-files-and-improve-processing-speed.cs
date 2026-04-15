using Aspose.Email;
using System;
using System.IO;
using System.Text;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Sample vCard data
            string vCardContent = "BEGIN:VCARD\r\nVERSION:2.1\r\nFN:John Doe\r\nEMAIL:john.doe@example.com\r\nEND:VCARD";

            // Load contact from a memory stream
            using (MemoryStream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(vCardContent)))
            {
                Contact contact = Contact.Load(inputStream);
                Console.WriteLine("Display Name: " + contact.DisplayName);
                Console.WriteLine("Email: " + (contact.EmailAddresses.Count > 0 ? contact.EmailAddresses[0].Address : "N/A"));

                // Save the contact back to another memory stream (optional)
                using (MemoryStream outputStream = new MemoryStream())
                {
                    contact.Save(outputStream);
                    // Reset position to read the saved data if needed
                    outputStream.Position = 0;
                    using (StreamReader reader = new StreamReader(outputStream, Encoding.UTF8))
                    {
                        string savedVCard = reader.ReadToEnd();
                        Console.WriteLine("Saved vCard content:");
                        Console.WriteLine(savedVCard);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
