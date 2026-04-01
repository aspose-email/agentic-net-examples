using System;
using System.Collections.Generic;
using Aspose.Email;

namespace AsposeEmailRuleSample
{
    // Simple rule class with condition and action
    public class Rule
    {
        public Func<MailMessage, bool> Condition { get; set; }
        public Action<MailMessage> Action { get; set; }

        public void Apply(MailMessage message)
        {
            if (Condition != null && Condition(message))
            {
                Action?.Invoke(message);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create a sample mail message
                MailMessage sampleMessage = new MailMessage();
                sampleMessage.From = "sender@example.com";
                sampleMessage.To.Add("recipient@example.com");
                sampleMessage.Subject = "Test Email";
                sampleMessage.Body = "This is a test email.";

                // Create a new rule instance
                Rule moveToFolderRule = new Rule();

                // Configure condition: subject contains "Test"
                moveToFolderRule.Condition = (MailMessage msg) => msg.Subject != null && msg.Subject.Contains("Test");

                // Configure action: add a custom header
                moveToFolderRule.Action = (MailMessage msg) => msg.Headers.Add("X-Processed", "True");

                // Apply rule to the message
                moveToFolderRule.Apply(sampleMessage);

                // Output result
                Console.WriteLine("Rule applied. Header X-Processed: " + sampleMessage.Headers["X-Processed"]);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }
        }
    }
}
