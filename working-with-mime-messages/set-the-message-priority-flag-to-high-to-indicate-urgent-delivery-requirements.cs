using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "high_priority.eml";
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Urgent: High Priority";
                message.Body = "Please address this issue as soon as possible.";
                message.Priority = MailPriority.High;

                message.Save(outputPath);
            }

            Console.WriteLine("Message saved with high priority.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
