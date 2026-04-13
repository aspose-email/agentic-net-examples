using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Output XML file path
            string xmlPath = "FolderHierarchy.xml";

            // Ensure the output directory exists
            string dir = Path.GetDirectoryName(Path.GetFullPath(xmlPath));
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // Create EWS client
            IEWSClient client = null;
            try
            {
                // Replace with actual mailbox URI and credentials
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create/connect EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Get root folder URI (use empty string to get the root)
                ExchangeFolderInfo rootInfo;
                try
                {
                    rootInfo = client.GetFolderInfo(string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve root folder info: {ex.Message}");
                    return;
                }

                // Build XML hierarchy
                XElement rootElement = new XElement("Folder",
                    new XAttribute("DisplayName", rootInfo.DisplayName ?? "Root"),
                    new XAttribute("Uri", rootInfo.Uri ?? string.Empty));

                BuildFolderXml(client, rootInfo.Uri, rootElement);

                XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), rootElement);

                // Save XML file
                try
                {
                    doc.Save(xmlPath);
                    Console.WriteLine($"Folder hierarchy exported to {xmlPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save XML file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void BuildFolderXml(IEWSClient client, string folderUri, XElement parentElement)
    {
        try
        {
            // List subfolders of the current folder
            var subFolders = client.ListSubFolders(folderUri);
            foreach (ExchangeFolderInfo subFolder in subFolders)
            {
                XElement folderElement = new XElement("Folder",
                    new XAttribute("DisplayName", subFolder.DisplayName ?? string.Empty),
                    new XAttribute("Uri", subFolder.Uri ?? string.Empty));

                parentElement.Add(folderElement);

                // Recursively process child folders
                BuildFolderXml(client, subFolder.Uri, folderElement);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing folder '{folderUri}': {ex.Message}");
        }
    }
}
