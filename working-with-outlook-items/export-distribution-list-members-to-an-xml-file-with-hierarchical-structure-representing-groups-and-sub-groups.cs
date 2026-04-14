using System;
using System.IO;
using System.Xml;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder credentials.
            if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and connect the EWS client.
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(serviceUrl, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or connect EWS client: {ex.Message}");
                return;
            }

            // Ensure the client is disposed properly.
            using (client)
            {
                // List all private distribution lists.
                ExchangeDistributionList[] distributionLists = null;
                try
                {
                    distributionLists = client.ListDistributionLists();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list distribution lists: {ex.Message}");
                    return;
                }

                // Prepare output file path.
                string outputPath = "distributionLists.xml";
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                        return;
                    }
                }

                // Write distribution list information to XML.
                try
                {
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                    using (XmlWriter writer = XmlWriter.Create(outputPath, settings))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("DistributionLists");

                        foreach (ExchangeDistributionList dl in distributionLists)
                        {
                            writer.WriteStartElement("DistributionList");
                            writer.WriteAttributeString("DisplayName", dl.DisplayName ?? string.Empty);
                            writer.WriteAttributeString("Id", dl.Id ?? string.Empty);

                            // Fetch members of the current distribution list.
                            MailAddressCollection members = null;
                            try
                            {
                                members = client.FetchDistributionList(dl);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to fetch members for list '{dl.DisplayName}': {ex.Message}");
                                // Continue with next list.
                                writer.WriteEndElement(); // DistributionList
                                continue;
                            }

                            writer.WriteStartElement("Members");
                            foreach (MailAddress address in members)
                            {
                                writer.WriteStartElement("Member");
                                writer.WriteAttributeString("DisplayName", address.DisplayName ?? string.Empty);
                                writer.WriteAttributeString("Address", address.Address ?? string.Empty);
                                writer.WriteEndElement(); // Member
                            }
                            writer.WriteEndElement(); // Members
                            writer.WriteEndElement(); // DistributionList
                        }

                        writer.WriteEndElement(); // DistributionLists
                        writer.WriteEndDocument();
                    }

                    Console.WriteLine($"Distribution lists exported to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write XML file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
