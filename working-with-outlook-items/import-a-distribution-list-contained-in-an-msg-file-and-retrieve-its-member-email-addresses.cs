using System;
using System.IO;
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

                Console.Error.WriteLine("The MSG file '{0}' was not found.", msgPath);
                return;
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                if (msg.SupportedType != MapiItemType.DistList)
                {
                    Console.WriteLine("The loaded MSG file is not a distribution list.");
                    return;
                }

                MapiDistributionList distributionList = (MapiDistributionList)msg.ToMapiMessageItem();

                Console.WriteLine("Distribution List Name: " + distributionList.DisplayName);
                Console.WriteLine("Member Count: " + distributionList.Members.Count);

                foreach (MapiDistributionListMember member in distributionList.Members)
                {
                    Console.WriteLine("Member Email: " + member.EmailAddress);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
