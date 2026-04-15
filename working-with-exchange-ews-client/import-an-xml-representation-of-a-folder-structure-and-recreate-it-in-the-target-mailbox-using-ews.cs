using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using System.Xml.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input XML file path
            string xmlPath = "FolderStructure.xml";

            // Verify the XML file exists
            if (!File.Exists(xmlPath))
            {
                Console.Error.WriteLine($"Input file not found: {xmlPath}");
                return;
            }

            // Load the XML safely
            XDocument doc;
            try
            {
                doc = XDocument.Load(xmlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load XML: {ex.Message}");
                return;
            }

            // EWS connection parameters
            string serviceUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUri, username, password))
                {
                    // Determine a parent folder for top‑level folders (use Inbox as example)
                    string rootFolderUri = client.MailboxInfo.InboxUri;

                    // Process each top‑level <Folder> element
                    foreach (XElement folderElement in doc.Root.Elements("Folder"))
                    {
                        CreateFolderRecursive(client, rootFolderUri, folderElement);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively creates folders based on XML definition
    private static void CreateFolderRecursive(IEWSClient client, string parentFolderUri, XElement folderElement)
    {
        string folderName = (string)folderElement.Attribute("Name");
        if (string.IsNullOrEmpty(folderName))
        {
            Console.Error.WriteLine("Folder element missing Name attribute.");
            return;
        }

        // Check if the folder already exists
        bool exists = client.FolderExists(parentFolderUri, folderName);
        string newFolderUri = parentFolderUri;

        if (!exists)
        {
            try
            {
                ExchangeFolderInfo createdInfo = client.CreateFolder(parentFolderUri, folderName);
                newFolderUri = createdInfo.Uri;
                Console.WriteLine($"Created folder: {folderName}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create folder '{folderName}': {ex.Message}");
                return;
            }
        }
        else
        {
            // Retrieve existing folder info to obtain its Uri
            try
            {
                ExchangeFolderInfo existingInfo = client.GetFolderInfo($"{parentFolderUri}/{folderName}");
                newFolderUri = existingInfo.Uri;
                Console.WriteLine($"Folder already exists: {folderName}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to retrieve existing folder '{folderName}': {ex.Message}");
                return;
            }
        }

        // Recurse into child folders
        foreach (XElement childFolder in folderElement.Elements("Folder"))
        {
            CreateFolderRecursive(client, newFolderUri, childFolder);
        }
    }
}
