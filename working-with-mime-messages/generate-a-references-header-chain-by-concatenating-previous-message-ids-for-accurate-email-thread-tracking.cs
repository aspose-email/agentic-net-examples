using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare a list of previous Message-IDs to build the References header.
            string[] previousMessageIds = new string[]
            {
                "<msg1@example.com>",
                "<msg2@example.com>",
                "<msg3@example.com>"
            };

            // Concatenate the IDs separated by a space as per RFC guidelines.
            string referencesHeader = string.Join(" ", previousMessageIds);

            // Create a new mail message.
            using (MailMessage message = new MailMessage())
            {
                // Basic message properties.
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Re: Sample Thread";
                message.Body = "This is a reply with a proper References header.";

                // Set the References header using the concatenated Message-IDs.
                // HeaderType.References implicitly converts to its string name.
                message.Headers[HeaderType.References] = referencesHeader;

                // Output the constructed References header to verify.
                Console.WriteLine("References: " + message.Headers[HeaderType.References]);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
