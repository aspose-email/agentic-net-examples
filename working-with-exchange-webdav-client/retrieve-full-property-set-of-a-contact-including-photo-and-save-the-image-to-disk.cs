using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        // Placeholder connection parameters
        string serviceUrl = "https://exchange.example.com/ews/Exchange.asmx";
        string username = "username";
        string password = "password";
        string contactId = "contact-id";
        string outputImagePath = "contact_photo.jpg";

        // If placeholders are detected, skip real call and create a dummy photo
        if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected – skipping external call.");
            byte[] dummyPhoto = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01 };
            try
            {
                string dir = Path.GetDirectoryName(Path.GetFullPath(outputImagePath));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllBytes(outputImagePath, dummyPhoto);
                Console.WriteLine($"Dummy photo saved to: {outputImagePath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write dummy photo: {ex.Message}");
            }
            return;
        }

        // Ensure output directory exists
        try
        {
            string dir = Path.GetDirectoryName(Path.GetFullPath(outputImagePath));
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
            return;
        }

        try
        {
            using (ExchangeClient client = new ExchangeClient(serviceUrl, username, password))
            {
                // Attempt to retrieve the contact using reflection to handle API variations
                Contact contact = null;
                var getById = typeof(ExchangeClient).GetMethod("GetContactById", new[] { typeof(string) });
                if (getById != null)
                {
                    contact = (Contact)getById.Invoke(client, new object[] { contactId });
                }
                else
                {
                    var getContact = typeof(ExchangeClient).GetMethod("GetContact", new[] { typeof(string) });
                    if (getContact != null)
                    {
                        contact = (Contact)getContact.Invoke(client, new object[] { contactId });
                    }
                }

                if (contact == null)
                {
                    Console.Error.WriteLine("Contact not found or retrieval method unavailable.");
                    return;
                }

                // Save contact photo if present
                byte[] photoData = contact.Photo?.Data;
                if (photoData != null && photoData.Length > 0)
                {
                    File.WriteAllBytes(outputImagePath, photoData);
                    Console.WriteLine($"Photo saved to: {outputImagePath}");
                }
                else
                {
                    Console.WriteLine("Contact does not contain a photo.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exchange client error: {ex.Message}");
        }
    }
}
