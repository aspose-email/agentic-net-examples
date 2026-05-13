using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";

            // Ensure directory exists
            string directory = Path.GetDirectoryName(emlPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            MailMessage message;

            if (File.Exists(emlPath))
            {
                try
                {
                    message = MailMessage.Load(emlPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load email file: {ex.Message}");
                    return;
                }
            }
            else
            {
                // Create a minimal placeholder email with a linked resource
                using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder", "This is a placeholder"))
                {
                    placeholder.IsBodyHtml = true;
                    placeholder.HtmlBody = "<html><body><img src='cid:placeholder'></body></html>";

                    Aspose.Email.LinkedResource lr = Aspose.Email.LinkedResource.CreateLinkedResourceFromString("dummy content");
                    lr.ContentId = "placeholder";
                    placeholder.LinkedResources.Add(lr);

                    try
                    {
                        placeholder.Save(emlPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save placeholder email: {ex.Message}");
                        return;
                    }

                    message = placeholder;
                }
            }

            using (message)
            {
                List<string> contentIds = GetLinkedResourceContentIds(message);
                foreach (string id in contentIds)
                {
                    Console.WriteLine($"LinkedResource Content-ID: {id}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static List<string> GetLinkedResourceContentIds(MailMessage message)
    {
        List<string> ids = new List<string>();

        // Linked resources directly attached to the message
        foreach (Aspose.Email.LinkedResource lr in message.LinkedResources)
        {
            if (!string.IsNullOrEmpty(lr.ContentId))
            {
                ids.Add(lr.ContentId);
            }
        }

        // Linked resources that belong to each alternate view (e.g., HTML view)
        foreach (AlternateView view in message.AlternateViews)
        {
            foreach (Aspose.Email.LinkedResource lr in view.LinkedResources)
            {
                if (!string.IsNullOrEmpty(lr.ContentId))
                {
                    ids.Add(lr.ContentId);
                }
            }
        }

        return ids;
    }
}
