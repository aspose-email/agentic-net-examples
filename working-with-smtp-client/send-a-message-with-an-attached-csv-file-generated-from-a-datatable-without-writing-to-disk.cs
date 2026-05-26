using Aspose.Email.Clients;
using System;
using System.Data;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP credentials
            string smtpHost = "YOUR_SMTP_HOST";
            int smtpPort = 587; // typical port
            string smtpUser = "YOUR_SMTP_USERNAME";
            string smtpPass = "YOUR_SMTP_PASSWORD";

            // Guard against placeholder credentials
            if (smtpHost.StartsWith("YOUR_") || smtpUser.StartsWith("YOUR_") || smtpPass.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder SMTP credentials detected. Skipping send operation.");
                return;
            }

            // Create a sample DataTable
            DataTable table = new DataTable("Sample");
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Rows.Add(1, "Alice");
            table.Rows.Add(2, "Bob");

            // Convert DataTable to CSV string
            StringBuilder csvBuilder = new StringBuilder();
            // Header
            for (int i = 0; i < table.Columns.Count; i++)
            {
                csvBuilder.Append(table.Columns[i].ColumnName);
                if (i < table.Columns.Count - 1) csvBuilder.Append(",");
            }
            csvBuilder.AppendLine();
            // Rows
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    csvBuilder.Append(row[i].ToString());
                    if (i < table.Columns.Count - 1) csvBuilder.Append(",");
                }
                csvBuilder.AppendLine();
            }

            // Create attachment from CSV data in memory
            byte[] csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            using (MemoryStream csvStream = new MemoryStream(csvBytes))
            using (Attachment csvAttachment = new Attachment(csvStream, "data.csv", "text/csv"))
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "sender@example.com";
                mailMessage.To.Add("recipient@example.com");
                mailMessage.Subject = "DataTable CSV Attachment";
                mailMessage.Body = "Please find the CSV attachment generated from a DataTable.";
                mailMessage.Attachments.Add(csvAttachment);

                // Send using SMTP client
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                {
                    client.SecurityOptions = SecurityOptions.Auto;
                    client.Send(mailMessage);
                    Console.WriteLine("Message sent successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
