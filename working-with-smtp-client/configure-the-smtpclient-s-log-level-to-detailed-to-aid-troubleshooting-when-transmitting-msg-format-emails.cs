using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

namespace SmtpClientLoggingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths for the MSG file and the log file
                string msgFilePath = "email.msg";
                string logFilePath = "smtp.log";

                // Verify the MSG file exists
                if (!File.Exists(msgFilePath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgFilePath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


                // Ensure the directory for the log file exists
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(logDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create log directory: {dirEx.Message}");
                        return;
                    }
                }

                // Load the MSG-format email
                MailMessage message;
                try
                {
                    message = MailMessage.Load(msgFilePath);
                }
                catch (Exception loadEx)
                {
                    Console.Error.WriteLine($"Failed to load message: {loadEx.Message}");
                    return;
                }

                // Configure and use the SMTP client
                try
                {
                    using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password", SecurityOptions.Auto))
                    {
                        // Enable detailed logging
                        client.EnableLogger = true;
                        client.LogFileName = logFilePath;
                        // Assuming LogLevel property exists; set to Detailed
                        client.GetType().GetProperty("LogLevel")?.SetValue(client, Enum.Parse(client.GetType().Assembly.GetType("Aspose.Email.Logging.LogLevel"), "Detailed"));

                        // Send the message
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (SmtpException smtpEx)
                {
                    Console.Error.WriteLine($"SMTP error: {smtpEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
                }
                finally
                {
                    // Dispose the MailMessage explicitly
                    if (message != null)
                    {
                        message.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Unexpected error: {e.Message}");
            }
        }
    }
}
