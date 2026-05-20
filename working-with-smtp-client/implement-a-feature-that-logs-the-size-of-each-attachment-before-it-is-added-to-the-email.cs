using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP configuration (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip sending when placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send.");
                return;
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("receiver@example.com");
                message.Subject = "Test email with attachment size logging";
                message.Body = "Please see attached files.";

                // Define attachment file paths
                string[] attachmentPaths = { "file1.txt", "file2.jpg" };

                foreach (string path in attachmentPaths)
                {
                    // Ensure the directory exists
                    string fullPath = Path.GetFullPath(path);
                    string dir = Path.GetDirectoryName(fullPath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    // Ensure the file exists; create a minimal placeholder if it does not
                    if (!File.Exists(fullPath))
                    {
                        try
                        {
                            File.WriteAllText(fullPath, "Placeholder content");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create placeholder for '{path}': {ex.Message}");
                            continue;
                        }
                    }

                    // Log the size of the attachment before adding
                    long size = new FileInfo(fullPath).Length;
                    Console.WriteLine($"Adding attachment '{Path.GetFileName(path)}' – {size} bytes");

                    // Create and add the attachment
                    Attachment attachment = new Attachment(fullPath);
                    message.Attachments.Add(attachment);
                }

                // Send the message using SMTP
                using (SmtpClient client = new SmtpClient(host, port, username, password))
                {
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
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
