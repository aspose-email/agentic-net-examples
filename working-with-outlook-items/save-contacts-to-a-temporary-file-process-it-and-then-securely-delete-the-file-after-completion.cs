using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Define a temporary file path for the contact (vCard format)
            string tempDirectory = Path.GetTempPath();
            string tempFilePath = Path.Combine(tempDirectory, "tempContact.vcf");

            // Ensure the directory exists
            if (!Directory.Exists(tempDirectory))
            {
                Console.Error.WriteLine("Temporary directory does not exist.");
                return;
            }

            // Create a contact and save it to the temporary file
            Contact contact = new Contact();
            contact.GivenName = "John";
            contact.Surname = "Doe";
            contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
            contact.PhoneNumbers.Add(new PhoneNumber { Number = "555-1234", Category = PhoneNumberCategory.Company });

            try
            {
                contact.Save(tempFilePath);
                Console.WriteLine($"Contact saved to temporary file: {tempFilePath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save contact: {ex.Message}");
                return;
            }

            // Process the saved contact (e.g., load and display its details)
            if (File.Exists(tempFilePath))
            {
                Contact loadedContact = null;
                try
                {
                    loadedContact = Contact.Load(tempFilePath);
                    Console.WriteLine("Loaded contact details:");
                    Console.WriteLine($"Name: {loadedContact.GivenName} {loadedContact.Surname}");
                    foreach (EmailAddress email in loadedContact.EmailAddresses)
                    {
                        Console.WriteLine($"Email: {email.Address}");
                    }
                    foreach (PhoneNumber phone in loadedContact.PhoneNumbers)
                    {
                        Console.WriteLine($"Phone ({phone.Category}): {phone.Number}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load contact: {ex.Message}");
                }
                finally
                {
                    // No disposal needed for Contact, but clear reference
                    loadedContact = null;
                }
            }
            else
            {
                Console.Error.WriteLine("Temporary contact file was not found for processing.");
                return;
            }

            // Securely delete the temporary file by overwriting its content before deletion
            if (File.Exists(tempFilePath))
            {
                try
                {
                    using (FileStream fs = new FileStream(tempFilePath, FileMode.Open, FileAccess.Write, FileShare.None))
                    {
                        long length = fs.Length;
                        fs.Position = 0;
                        byte[] zeroBuffer = new byte[4096];
                        long remaining = length;
                        while (remaining > 0)
                        {
                            int writeSize = (int)Math.Min(zeroBuffer.Length, remaining);
                            fs.Write(zeroBuffer, 0, writeSize);
                            remaining -= writeSize;
                        }
                        fs.Flush(true);
                    }

                    File.Delete(tempFilePath);
                    Console.WriteLine("Temporary contact file securely deleted.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to securely delete file: {ex.Message}");
                }
            }
            else
            {
                Console.Error.WriteLine("Temporary contact file not found for deletion.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
