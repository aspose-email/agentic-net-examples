using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
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

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            try
            {
                using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
                {
                    if (mapiMessage.SupportedType != MapiItemType.DistList)
                    {
                        Console.WriteLine("The MSG file does not contain a distribution list.");
                        return;
                    }

                    using (MapiDistributionList distributionList = (MapiDistributionList)mapiMessage.ToMapiMessageItem())
                    {
                        Console.WriteLine($"Distribution List: {distributionList.DisplayName}");
                        Console.WriteLine($"Member count: {distributionList.Members.Count}");

                        foreach (MapiDistributionListMember member in distributionList.Members)
                        {
                            Console.WriteLine($"Member: {member.DisplayName} <{member.EmailAddress}>");
                        }
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"IO error: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
