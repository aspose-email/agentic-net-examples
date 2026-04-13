using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aspose.Email;

namespace DomainAnalysisSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Sample list of contact email addresses.
                List<string> emailAddresses = new List<string>
                {
                    "alice@example.com",
                    "bob@subdomain.example.org",
                    "carol@test.co.uk",
                    "dave@example.com",
                    "eve@anotherdomain.net"
                };

                // Regular expression to capture the domain part after '@'.
                Regex domainRegex = new Regex(@"@(?<domain>[^@\s]+)", RegexOptions.Compiled);

                // Dictionary to hold domain occurrence counts.
                Dictionary<string, int> domainCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                foreach (string email in emailAddresses)
                {
                    Match match = domainRegex.Match(email);
                    if (match.Success)
                    {
                        string domain = match.Groups["domain"].Value;
                        if (domainCounts.ContainsKey(domain))
                        {
                            domainCounts[domain] += 1;
                        }
                        else
                        {
                            domainCounts[domain] = 1;
                        }
                    }
                }

                // Output the domain analysis results.
                Console.WriteLine("Domain occurrence analysis:");
                foreach (KeyValuePair<string, int> entry in domainCounts)
                {
                    Console.WriteLine($"{entry.Key}: {entry.Value}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
