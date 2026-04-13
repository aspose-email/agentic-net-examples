using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "distributionList.msg";

            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                if (msg.SupportedType != MapiItemType.DistList)
                {
                    Console.Error.WriteLine("The provided MSG file is not a distribution list.");
                    return;
                }

                MapiDistributionList distributionList = (MapiDistributionList)msg.ToMapiMessageItem();

                // Simple regex for a 10‑digit phone number (adjust as needed)
                Regex phoneRegex = new Regex(@"\b\d{10}\b", RegexOptions.Compiled);

                foreach (MapiDistributionListMember member in distributionList.Members)
                {
                    // Example assumes the phone number may be embedded in the DisplayName.
                    // Adjust the extraction logic based on actual data source.
                    Match match = phoneRegex.Match(member.DisplayName ?? string.Empty);
                    if (!match.Success)
                    {
                        Console.WriteLine($"Member '{member.DisplayName}' has an invalid or missing phone number.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
