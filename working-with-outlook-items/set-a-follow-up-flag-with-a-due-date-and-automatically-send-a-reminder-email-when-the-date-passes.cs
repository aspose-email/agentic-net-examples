using System;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP settings
            string smtpHost = "smtp.example.com";
            int smtpPort = 25;
            string smtpUser = "user@example.com";
            string smtpPass = "password";

            // Skip execution when placeholder credentials are detected
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            // Create a simple mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Task with Follow‑up";
                message.Body = "Please complete the task.";

                // Convert to a MAPI message to set follow‑up flags
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(message))
                {
                    // Define due date and reminder time
                    DateTime dueDate = DateTime.Now.AddDays(2);
                    DateTime reminderTime = DateTime.Now.AddDays(1);

                    // Set a follow‑up flag for recipients with a reminder
                    FollowUpManager.SetFlagForRecipients(mapiMessage, "Please complete", reminderTime);

                    // Set additional follow‑up options such as due date
                    FollowUpOptions options = new FollowUpOptions();
                    options.DueDate = dueDate;
                    FollowUpManager.SetOptions(mapiMessage, options);

                    // Convert back to MailMessage for sending
                    using (MailMessage sendMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                    {
                        // Send the message via SMTP
                        using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                        {
                            try
                            {
                                client.Send(sendMessage);
                                Console.WriteLine("Message sent successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                                return;
                            }
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
