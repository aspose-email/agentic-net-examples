using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Prepare sample contacts
            MapiContact[] contacts = new MapiContact[]
            {
                new MapiContact("John Doe", "john.doe@example.com"),
                new MapiContact("Jane Smith", "jane.smith@contoso.com"),
                new MapiContact("Bob Johnson", "bob.johnson@example.com")
            };

            // Dictionary to hold domain counts
            Dictionary<string, int> domainCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            // Regular expression to capture domain part after '@'
            Regex domainRegex = new Regex(@"@(?<domain>[^>]+)$", RegexOptions.Compiled);

            foreach (MapiContact contact in contacts)
            {
                using (contact)
                {
                    // Access the first email address via ElectronicAddresses.Email1.EmailAddress
                    string emailAddress = contact.ElectronicAddresses?.Email1?.EmailAddress;
                    if (string.IsNullOrEmpty(emailAddress))
                        continue;

                    Match match = domainRegex.Match(emailAddress);
                    if (match.Success)
                    {
                        string domain = match.Groups["domain"].Value;
                        if (domainCounts.ContainsKey(domain))
                            domainCounts[domain] = domainCounts[domain] + 1;
                        else
                            domainCounts[domain] = 1;
                    }
                }
            }

            // Output domain analysis results
            Console.WriteLine("Domain analysis of contact email addresses:");
            foreach (KeyValuePair<string, int> kvp in domainCounts)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
