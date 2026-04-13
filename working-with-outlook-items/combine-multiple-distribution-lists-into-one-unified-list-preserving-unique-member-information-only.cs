using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the source distribution list files and the output file
            string list1Path = "list1.msg";
            string list2Path = "list2.msg";
            string outputPath = "combined.msg";

            // Ensure the first input file exists; create a minimal placeholder if missing
            if (!File.Exists(list1Path))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(list1Path);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                using (MapiDistributionList placeholder1 = new MapiDistributionList())
                {
                    placeholder1.DisplayName = "Placeholder List 1";
                    placeholder1.Save(list1Path);
                }
            }

            // Ensure the second input file exists; create a minimal placeholder if missing
            if (!File.Exists(list2Path))
            {
                using (MapiDistributionList placeholder2 = new MapiDistributionList())
                {
                    placeholder2.DisplayName = "Placeholder List 2";
                    placeholder2.Save(list2Path);
                }
            }

            // Load the first distribution list
            MapiDistributionList distList1;
            using (MapiMessage msg1 = MapiMessage.Load(list1Path))
            {
                if (msg1.SupportedType != MapiItemType.DistList)
                {
                    Console.Error.WriteLine("File '{0}' is not a distribution list. Creating an empty placeholder.", list1Path);
                    distList1 = new MapiDistributionList();
                }
                else
                {
                    distList1 = (MapiDistributionList)msg1.ToMapiMessageItem();
                }
            }

            // Load the second distribution list
            MapiDistributionList distList2;
            using (MapiMessage msg2 = MapiMessage.Load(list2Path))
            {
                if (msg2.SupportedType != MapiItemType.DistList)
                {
                    Console.Error.WriteLine("File '{0}' is not a distribution list. Creating an empty placeholder.", list2Path);
                    distList2 = new MapiDistributionList();
                }
                else
                {
                    distList2 = (MapiDistributionList)msg2.ToMapiMessageItem();
                }
            }

            // Create a new distribution list to hold the combined unique members
            using (MapiDistributionList combinedList = new MapiDistributionList())
            {
                combinedList.DisplayName = "Combined Distribution List";

                // Use a HashSet to track unique email addresses
                HashSet<string> uniqueEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Helper action to add members from a source list
                Action<MapiDistributionList> addMembers = sourceList =>
                {
                    if (sourceList != null && sourceList.Members != null)
                    {
                        foreach (MapiDistributionListMember member in sourceList.Members)
                        {
                            if (member != null && !string.IsNullOrEmpty(member.EmailAddress))
                            {
                                if (uniqueEmails.Add(member.EmailAddress))
                                {
                                    // Add a copy of the member to the combined list
                                    MapiDistributionListMember newMember = new MapiDistributionListMember(member.DisplayName, member.EmailAddress);
                                    combinedList.Members.Add(newMember);
                                }
                            }
                        }
                    }
                };

                // Add members from both source lists
                addMembers(distList1);
                addMembers(distList2);

                // Save the combined distribution list
                combinedList.Save(outputPath);
                Console.WriteLine("Combined distribution list saved to '{0}'.", outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
