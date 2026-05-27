using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using System.Text;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example") || username.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            string outputPath = "ContactDirectory.pdf";

            // Connect to Exchange and retrieve contacts.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                MapiContact[] contacts = client.ListContacts("contacts");

                if (contacts == null || contacts.Length == 0)
                {
                    Console.Error.WriteLine("No contacts found.");
                    return;
                }

                // Build HTML representation of contacts.
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<html><head><style>");
                htmlBuilder.AppendLine("table { width: 100%; border-collapse: collapse; }");
                htmlBuilder.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; vertical-align: top; }");
                htmlBuilder.AppendLine("th { background-color: #f2f2f2; }");
                htmlBuilder.AppendLine("img { max-width: 100px; max-height: 100px; }");
                htmlBuilder.AppendLine("</style></head><body>");
                htmlBuilder.AppendLine("<h2>Contact Directory</h2>");
                htmlBuilder.AppendLine("<table>");
                htmlBuilder.AppendLine("<tr><th>Photo</th><th>Name</th><th>Email</th><th>Phone</th></tr>");

                foreach (MapiContact contact in contacts)
                {
                    // Retrieve photo via reflection (property may not exist in some versions)
                    string photoDataUri = "";
                    PropertyInfo photoProp = contact.GetType().GetProperty("ContactPhoto");
                    if (photoProp != null)
                    {
                        var photoBytes = photoProp.GetValue(contact) as byte[];
                        if (photoBytes != null && photoBytes.Length > 0)
                        {
                            string base64 = Convert.ToBase64String(photoBytes);
                            photoDataUri = $"data:image/jpeg;base64,{base64}";
                        }
                    }

                    string name = GetStringProperty(contact, "DisplayName") ??
                                  $"{GetStringProperty(contact, "Name") ?? ""} {GetStringProperty(contact, "Surname") ?? ""}".Trim();

                    string email = GetStringProperty(contact, "Email1Address") ?? "";
                    string phone = GetStringProperty(contact, "BusinessTelephoneNumber") ??
                                   GetStringProperty(contact, "HomeTelephoneNumber") ??
                                   GetStringProperty(contact, "MobileTelephoneNumber") ?? "";

                    htmlBuilder.AppendLine("<tr>");
                    htmlBuilder.AppendLine($"<td>{(string.IsNullOrEmpty(photoDataUri) ? "" : $"<img src=\"{photoDataUri}\"/>")}</td>");
                    htmlBuilder.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(name)}</td>");
                    htmlBuilder.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(email)}</td>");
                    htmlBuilder.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(phone)}</td>");
                    htmlBuilder.AppendLine("</tr>");
                }

                htmlBuilder.AppendLine("</table>");
                htmlBuilder.AppendLine("</body></html>");

                // Convert HTML to PDF using Aspose.Words.
                byte[] htmlBytes = Encoding.UTF8.GetBytes(htmlBuilder.ToString());
                using (MemoryStream htmlStream = new MemoryStream(htmlBytes))
                {
                    Document doc = new Document(htmlStream, new Aspose.Words.LoadOptions());
                    doc.Save(outputPath, new Aspose.Words.Saving.PdfSaveOptions());
                }

                Console.WriteLine($"PDF directory saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static string GetStringProperty(object obj, string propertyName)
    {
        PropertyInfo prop = obj.GetType().GetProperty(propertyName);
        if (prop != null)
        {
            var value = prop.GetValue(obj) as string;
            return value;
        }
        return null;
    }
}
