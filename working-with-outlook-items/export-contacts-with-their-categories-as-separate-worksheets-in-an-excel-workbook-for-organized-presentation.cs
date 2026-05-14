using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "ExportedContacts");
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Create sample contacts with categories
            List<MapiContact> contacts = new List<MapiContact>();

            var contact1 = new MapiContact
            {
                NameInfo = { DisplayName = "Alice Johnson" },
                ElectronicAddresses = { Email1 = { EmailAddress = "alice@example.com" } },
                Categories = new[] { "Friends", "Work" }
            };
            contacts.Add(contact1);

            var contact2 = new MapiContact
            {
                NameInfo = { DisplayName = "Bob Smith" },
                ElectronicAddresses = { Email1 = { EmailAddress = "bob@example.com" } },
                Categories = new[] { "Family" }
            };
            contacts.Add(contact2);

            var contact3 = new MapiContact
            {
                NameInfo = { DisplayName = "Carol Davis" },
                ElectronicAddresses = { Email1 = { EmailAddress = "carol@example.com" } },
                Categories = new[] { "Work" }
            };
            contacts.Add(contact3);

            // Group contacts by category
            var categoryMap = new Dictionary<string, List<MapiContact>>(StringComparer.OrdinalIgnoreCase);
            foreach (var c in contacts)
            {
                if (c.Categories == null) continue;
                foreach (var cat in c.Categories)
                {
                    if (!categoryMap.TryGetValue(cat, out var list))
                    {
                        list = new List<MapiContact>();
                        categoryMap[cat] = list;
                    }
                    list.Add(c);
                }
            }

            // Export each category to its own CSV file (as a stand‑in for separate worksheets)
            foreach (var kvp in categoryMap)
            {
                string category = kvp.Key;
                var catContacts = kvp.Value;

                var sb = new StringBuilder();
                sb.AppendLine("Display Name,Email");

                foreach (var c in catContacts)
                {
                    string name = c.NameInfo?.DisplayName ?? string.Empty;
                    string email = c.ElectronicAddresses?.Email1?.EmailAddress ?? string.Empty;
                    sb.AppendLine($"\"{name}\",\"{email}\"");
                }

                string safeFileName = string.Concat(category.Split(Path.GetInvalidFileNameChars()));
                string filePath = Path.Combine(outputDir, $"{safeFileName}.csv");
                File.WriteAllText(filePath, sb.ToString());
                Console.WriteLine($"Category '{category}' exported to '{filePath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
