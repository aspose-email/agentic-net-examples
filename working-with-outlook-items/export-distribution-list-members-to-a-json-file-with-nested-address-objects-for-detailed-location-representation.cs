using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip execution in CI environments.
            string host = "exchange.example.com";
            string username = "user@example.com";
            string password = "password";
            string distributionListAddress = "dl@example.com";

            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder Exchange credentials detected. Skipping execution.");
                return;
            }

            // Ensure output directory exists.
            string outputPath = "dl_members.json";
            try
            {
                string? directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
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

            // Connect to Exchange using EWS.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
                {
                    // Expand the public distribution list to get its members.
                    MailAddressCollection members = client.ExpandDistributionList(new MailAddress(distributionListAddress));

                    // Prepare a serializable list of member objects.
                    var memberList = new List<object>();
                    foreach (MailAddress address in members)
                    {
                        memberList.Add(new
                        {
                            DisplayName = address.DisplayName,
                            Email = address.Address
                        });
                    }

                    // Serialize to JSON.
                    string json = JsonSerializer.Serialize(memberList, new JsonSerializerOptions { WriteIndented = true });

                    // Write JSON to file.
                    try
                    {
                        using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(json);
                        }
                        Console.WriteLine($"Distribution list members exported to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write JSON file: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange or retrieve members: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
