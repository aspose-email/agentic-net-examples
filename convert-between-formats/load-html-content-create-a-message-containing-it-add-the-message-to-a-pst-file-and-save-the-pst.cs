using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the HTML content and the PST file
            string htmlPath = "content.html";
            string pstPath = "output.pst";

            // Ensure the HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(htmlPath))
            {
                try
                {
                    File.WriteAllText(htmlPath, "<html><body><p>Hello World</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Load HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read HTML file: {ex.Message}");
                return;
            }

            // Create or open the PST file
            PersonalStorage pst;
            try
            {
                if (File.Exists(pstPath))
                {
                    pst = PersonalStorage.FromFile(pstPath);
                }
                else
                {
                    pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to open or create PST file: {ex.Message}");
                return;
            }

            using (pst)
            {
                // Build the email message with HTML body
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("sender@example.com");
                    mail.To = new MailAddressCollection();
                    mail.To.Add(new MailAddress("recipient@example.com"));
                    mail.Subject = "Sample HTML Email";
                    mail.HtmlBody = htmlContent;

                    // Convert MailMessage to MapiMessage
                    using (MapiMessage mapi = MapiMessage.FromMailMessage(mail))
                    {
                        // Add the message to the root folder of the PST
                        try
                        {
                            string entryId = pst.RootFolder.AddMessage(mapi);
                            Console.WriteLine($"Message added to PST. EntryId: {entryId}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to add message to PST: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
