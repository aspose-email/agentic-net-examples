using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            string host = "exchange.example.com";
            string username = "username";
            string password = "password";
            string outputPath = "distributionLists.xml";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Please provide valid Exchange server credentials.");
                return;
            }

            try
            {
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
            {
                ExchangeDistributionList[] allLists = client.ListDistributionLists();

                Dictionary<string, ExchangeDistributionList> dlMap = new Dictionary<string, ExchangeDistributionList>(StringComparer.OrdinalIgnoreCase);
                foreach (ExchangeDistributionList list in allLists)
                {
                    if (!string.IsNullOrEmpty(list.DisplayName))
                    {
                        dlMap[list.DisplayName] = list;
                    }
                }

                XDocument doc = new XDocument(new XElement("DistributionLists"));
                HashSet<string> visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (ExchangeDistributionList list in allLists)
                {
                    ExportDistributionList(list, client, doc.Root, dlMap, visited);
                }

                try
                {
                    doc.Save(outputPath);
                    Console.WriteLine($"Distribution lists exported to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save XML file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ExportDistributionList(ExchangeDistributionList dl, IEWSClient client, XContainer parent, Dictionary<string, ExchangeDistributionList> dlMap, HashSet<string> visited)
    {
        if (dl == null || string.IsNullOrEmpty(dl.DisplayName) || visited.Contains(dl.DisplayName))
            return;

        visited.Add(dl.DisplayName);

        XElement dlElement = new XElement("DistributionList");
        dlElement.SetAttributeValue("Name", dl.DisplayName);
        parent.Add(dlElement);

        MailAddressCollection members;
        try
        {
            members = client.FetchDistributionList(dl);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to fetch members for {dl.DisplayName}: {ex.Message}");
            return;
        }

        foreach (MailAddress member in members)
        {
            XElement memberElement = new XElement("Member");
            memberElement.SetAttributeValue("Email", member.Address);
            if (!string.IsNullOrEmpty(member.DisplayName))
                memberElement.SetAttributeValue("DisplayName", member.DisplayName);
            dlElement.Add(memberElement);

            if (dlMap.TryGetValue(member.DisplayName, out ExchangeDistributionList subDl))
            {
                ExportDistributionList(subDl, client, memberElement, dlMap, visited);
            }
        }
    }
}
