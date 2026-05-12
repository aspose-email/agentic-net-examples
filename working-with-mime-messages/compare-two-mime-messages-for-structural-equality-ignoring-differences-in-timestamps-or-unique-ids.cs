using Aspose.Email.Clients;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string messagePath1 = "message1.eml";
            string messagePath2 = "message2.eml";

            // Verify input files exist
            if (!File.Exists(messagePath1))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath1, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {messagePath1}");
                return;
            }

            if (!File.Exists(messagePath2))
            {
                Console.Error.WriteLine($"Input file not found: {messagePath2}");
                return;
            }

            try
            {
                using (MailMessage message1 = MailMessage.Load(messagePath1))
                using (MailMessage message2 = MailMessage.Load(messagePath2))
                {
                    bool areEqual = AreMessagesStructurallyEqual(message1, message2);
                    Console.WriteLine(areEqual
                        ? "Messages are structurally equal (ignoring timestamps and IDs)."
                        : "Messages differ in structure.");
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Error processing messages: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static bool AreMessagesStructurallyEqual(MailMessage msg1, MailMessage msg2)
    {
        // Compare simple scalar properties
        if (!string.Equals(msg1.Subject, msg2.Subject, StringComparison.Ordinal))
            return false;

        if (msg1.From != null && msg2.From != null)
        {
            if (!string.Equals(msg1.From.Address, msg2.From.Address, StringComparison.OrdinalIgnoreCase))
                return false;
        }
        else if (msg1.From != null || msg2.From != null)
        {
            return false;
        }

        // Compare body content
        if (!string.Equals(msg1.Body, msg2.Body, StringComparison.Ordinal))
            return false;

        if (!string.Equals(msg1.HtmlBody, msg2.HtmlBody, StringComparison.Ordinal))
            return false;

        // Compare recipient collections (To, CC, BCC)
        if (!AreAddressCollectionsEqual(msg1.To, msg2.To))
            return false;

        if (!AreAddressCollectionsEqual(msg1.CC, msg2.CC))
            return false;

        if (!AreAddressCollectionsEqual(msg1.Bcc, msg2.Bcc))
            return false;

        // Compare attachments (by file name)
        if (msg1.Attachments.Count != msg2.Attachments.Count)
            return false;

        for (int i = 0; i < msg1.Attachments.Count; i++)
        {
            if (!string.Equals(msg1.Attachments[i].Name, msg2.Attachments[i].Name, StringComparison.OrdinalIgnoreCase))
                return false;
        }

        // All compared aspects are equal
        return true;
    }

    static bool AreAddressCollectionsEqual(MailAddressCollection col1, MailAddressCollection col2)
    {
        if (col1 == null && col2 == null)
            return true;

        if (col1 == null || col2 == null)
            return false;

        if (col1.Count != col2.Count)
            return false;

        List<string> list1 = new List<string>();
        foreach (MailAddress address in col1)
            list1.Add(address.Address.ToLowerInvariant());

        List<string> list2 = new List<string>();
        foreach (MailAddress address in col2)
            list2.Add(address.Address.ToLowerInvariant());

        list1.Sort();
        list2.Sort();

        for (int i = 0; i < list1.Count; i++)
        {
            if (!string.Equals(list1[i], list2[i], StringComparison.Ordinal))
                return false;
        }

        return true;
    }
}
