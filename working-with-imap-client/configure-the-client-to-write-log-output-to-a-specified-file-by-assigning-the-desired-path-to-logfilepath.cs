using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Specify the log file path
            string logFilePath = "logs/email.log";

            // Ensure the directory for the log file exists
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Create and configure the SMTP client
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    // Enable logging and set the log file name
                    client.EnableLogger = true;
                    client.LogFileName = logFilePath;
                    client.UseDateInLogFileName = false; // optional: do not append date

                    Console.WriteLine("Email client logging is configured to: " + client.LogFileName);
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine("Client error: " + clientEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
