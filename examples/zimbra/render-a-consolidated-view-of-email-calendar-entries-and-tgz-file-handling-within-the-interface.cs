using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace AsposeEmailZimbraSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // ---------- TGZ File Handling ----------
                string tgzFilePath = "sample.tgz";

                // Ensure the TGZ file exists; create a minimal placeholder if missing
                if (!File.Exists(tgzFilePath))
                {
                    using (FileStream placeholder = File.Create(tgzFilePath))
                    {
                        // Write an empty GZIP header to make it a valid (though empty) TGZ
                        byte[] emptyGzipHeader = new byte[] { 0x1F, 0x8B, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF };
                        placeholder.Write(emptyGzipHeader, 0, emptyGzipHeader.Length);
                    }
                    Console.WriteLine($"Created placeholder TGZ file at '{tgzFilePath}'.");
                }

                // Open the TGZ file and read a few bytes from the GZIP stream
                using (FileStream tgzStream = File.OpenRead(tgzFilePath))
                using (GZipStream gzipStream = new GZipStream(tgzStream, CompressionMode.Decompress))
                {
                    byte[] buffer = new byte[128];
                    int bytesRead = gzipStream.Read(buffer, 0, buffer.Length);
                    Console.WriteLine($"Read {bytesRead} bytes from TGZ (gzip) stream.");
                }

                // ---------- Gmail (Email & Calendar) Interaction ----------
                // NOTE: Replace the placeholder values with real OAuth access token and email.
                string accessToken = "YOUR_ACCESS_TOKEN";
                string defaultEmail = "user@example.com";

                // Create Gmail client instance inside a using block to ensure disposal
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    try
                    {
                        // List calendars
                        Aspose.Email.Clients.Google.Calendar[] calendars = gmailClient.ListCalendars();
                        Console.WriteLine($"Found {calendars.Length} calendar(s).");
                        foreach (Aspose.Email.Clients.Google.Calendar calendar in calendars)
                        {
                            Console.WriteLine($"Calendar Summary: {calendar.Summary}");
                        }

                        // List contacts (as a simple email-related view)
                        Aspose.Email.PersonalInfo.Contact[] contacts = gmailClient.GetAllContacts();
                        Console.WriteLine($"Found {contacts.Length} contact(s).");
                        foreach (Aspose.Email.PersonalInfo.Contact contact in contacts)
                        {
                            Console.WriteLine($"Contact Display Name: {contact.DisplayName}");
                        }
                    }
                    catch (Exception gmailEx)
                    {
                        Console.Error.WriteLine($"Gmail operation failed: {gmailEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}