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
            string tempFilePath = Path.Combine(Path.GetTempPath(), "temp_contact.vcf");

            // Ensure the directory exists
            try
            {
                string directory = Path.GetDirectoryName(tempFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Directory creation failed: {dirEx.Message}");
                return;
            }

            // Create a contact and populate some fields
            Contact contact = new Contact
            {
                GivenName = "John",
                Surname = "Doe",
                EmailAddresses = { new EmailAddress("john.doe@example.com", "John Doe") },
                PhoneNumbers = { new PhoneNumber { Number = "+1-555-1234", Category = PhoneNumberCategory.Company } }
            };

            // Save the contact to the temporary file
            try
            {
                contact.Save(tempFilePath);
                Console.WriteLine($"Contact saved to temporary file: {tempFilePath}");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save contact: {saveEx.Message}");
                return;
            }

            // Process the file (example: read its contents)
            try
            {
                if (File.Exists(tempFilePath))
                {
                    string content = File.ReadAllText(tempFilePath);
                    Console.WriteLine("Contact file content:");
                    Console.WriteLine(content);
                }
                else
                {
                    Console.Error.WriteLine("Temporary contact file does not exist.");
                }
            }
            catch (Exception readEx)
            {
                Console.Error.WriteLine($"Failed to read temporary file: {readEx.Message}");
            }

            // Securely delete the temporary file
            try
            {
                if (File.Exists(tempFilePath))
                {
                    // Overwrite the file with zeros
                    using (FileStream fs = new FileStream(tempFilePath, FileMode.Open, FileAccess.Write, FileShare.None))
                    {
                        long length = fs.Length;
                        byte[] zeros = new byte[4096];
                        long written = 0;
                        while (written < length)
                        {
                            int toWrite = (int)Math.Min(zeros.Length, length - written);
                            fs.Write(zeros, 0, toWrite);
                            written += toWrite;
                        }
                        fs.Flush(true);
                    }

                    // Delete the file
                    File.Delete(tempFilePath);
                    Console.WriteLine("Temporary file securely deleted.");
                }
            }
            catch (Exception delEx)
            {
                Console.Error.WriteLine($"Failed to securely delete file: {delEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
