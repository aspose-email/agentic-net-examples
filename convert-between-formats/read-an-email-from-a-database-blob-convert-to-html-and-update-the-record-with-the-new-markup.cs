using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Dictionary<int, byte[]> database = new Dictionary<int, byte[]>();
            int recordId = 1;

            if (!database.ContainsKey(recordId))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder",
                        "This is a placeholder email."))
                    using (MemoryStream placeholderStream = new MemoryStream())
                    {
                        placeholder.Save(placeholderStream);
                        database[recordId] = placeholderStream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder blob: {ex.Message}");
                    return;
                }
            }

            byte[] emailBlob = database[recordId];
            using (MemoryStream inputStream = new MemoryStream(emailBlob))
            using (MailMessage message = MailMessage.Load(inputStream))
            {
                Aspose.Email.HtmlSaveOptions htmlOptions = new Aspose.Email.HtmlSaveOptions()
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };

                using (MemoryStream htmlStream = new MemoryStream())
                {
                    message.Save(htmlStream, htmlOptions);
                    string htmlContent = Encoding.UTF8.GetString(htmlStream.ToArray());
                    database[recordId] = Encoding.UTF8.GetBytes(htmlContent);
                    Console.WriteLine("Email converted to HTML and database record updated successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
