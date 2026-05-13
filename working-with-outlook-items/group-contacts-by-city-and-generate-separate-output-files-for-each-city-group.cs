using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // ----- Configuration -----
            string pstPath = "YOUR_PST_PATH.pst";               // Path to the PST file
            string outputFolder = "YOUR_OUTPUT_FOLDER";         // Folder where city files will be created

            // Guard against placeholder values
            if (string.IsNullOrWhiteSpace(pstPath) || pstPath.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(outputFolder) || outputFolder.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Please replace placeholder paths with actual values.");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Ensure PST file exists; if not, create an empty one
            try
            {
                if (!File.Exists(pstPath))
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode)) { }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare PST file: {ex.Message}");
                return;
            }

            // ----- Process PST -----
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Collect contacts grouped by city
                    var contactsByCity = new Dictionary<string, List<MapiContact>>(StringComparer.OrdinalIgnoreCase);

                    // Iterate all folders recursively
                    Queue<FolderInfo> folders = new Queue<FolderInfo>();
                    folders.Enqueue(pst.RootFolder);

                    while (folders.Count > 0)
                    {
                        FolderInfo folder = folders.Dequeue();

                        // Enqueue subfolders
                        foreach (FolderInfo sub in folder.GetSubFolders())
                            folders.Enqueue(sub);

                        // Enumerate messages in the current folder
                        foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                        {
                            using (MapiMessage msg = pst.ExtractMessage(msgInfo))
                            {
                                if (msg.SupportedType != MapiItemType.Contact)
                                    continue;

                                // Convert to MapiContact
                                MapiContact contact = (MapiContact)msg.ToMapiMessageItem();

                                // Determine city (prefer Work, then Home, then Other)
                                string city = contact.PhysicalAddresses?.WorkAddress?.City ??
                                              contact.PhysicalAddresses?.HomeAddress?.City ??
                                              contact.PhysicalAddresses?.OtherAddress?.City ??
                                              "Unknown";

                                if (!contactsByCity.TryGetValue(city, out List<MapiContact> list))
                                {
                                    list = new List<MapiContact>();
                                    contactsByCity[city] = list;
                                }
                                list.Add(contact);
                            }
                        }
                    }

                    // ----- Write output files per city -----
                    foreach (var kvp in contactsByCity)
                    {
                        string cityName = string.IsNullOrWhiteSpace(kvp.Key) ? "Unknown" : kvp.Key;
                        string safeCityName = string.Concat(cityName.Split(Path.GetInvalidFileNameChars()));
                        string filePath = Path.Combine(outputFolder, $"{safeCityName}.csv");

                        try
                        {
                            using (StreamWriter writer = new StreamWriter(filePath, false))
                            {
                                writer.WriteLine("DisplayName,Email");
                                foreach (MapiContact c in kvp.Value)
                                {
                                    string email = c.ElectronicAddresses?.Email1?.EmailAddress ?? "";
                                    string name = c.NameInfo?.DisplayName ?? "";
                                    writer.WriteLine($"\"{name}\",\"{email}\"");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to write file for city '{cityName}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
