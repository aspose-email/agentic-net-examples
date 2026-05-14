using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "contacts.pst";
            string outputCsv = "exported_contacts.csv";

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputCsv));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Verify PST file existence; if missing, skip gracefully
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found at path: {pstPath}");
                return;
            }

            // Open PST and export contacts
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Assume contacts are stored in the Contacts folder
                    FolderInfo contactsFolder = pst.RootFolder.GetSubFolder("Contacts");
                    List<string> lines = new List<string>();
                    lines.Add("DisplayName,BirthdayISO");

                    foreach (MessageInfo info in contactsFolder.EnumerateMessages())
                    {
                        using (MapiMessage msg = pst.ExtractMessage(info))
                        {
                            // Convert to MapiContact if possible
                            if (msg.SupportedType == MapiItemType.Contact)
                            {
                                MapiContact contact = (MapiContact)msg.ToMapiMessageItem();

                                string displayName = contact.NameInfo?.DisplayName ?? string.Empty;

                                DateTime birthday = DateTime.MinValue;
                                bool hasBirthday = contact.TryGetPropertyDateTime(KnownPropertyList.Birthday.Id, ref birthday);
                                string birthdayIso = hasBirthday ? birthday.ToString("yyyy-MM-dd") : string.Empty;

                                lines.Add($"{EscapeCsv(displayName)},{birthdayIso}");
                            }
                        }
                    }

                    // Write CSV
                    try
                    {
                        File.WriteAllLines(outputCsv, lines);
                        Console.WriteLine($"Export completed. CSV saved to: {outputCsv}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write CSV file: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper to escape CSV fields
    private static string EscapeCsv(string field)
    {
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            field = field.Replace("\"", "\"\"");
            return $"\"{field}\"";
        }
        return field;
    }
}
