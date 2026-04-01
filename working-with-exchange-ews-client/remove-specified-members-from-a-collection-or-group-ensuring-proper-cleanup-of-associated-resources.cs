using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a collection of mail addresses (e.g., a distribution list)
            MailAddressCollection members = new MailAddressCollection();

            // Add some members to the collection
            members.Add(new MailAddress("alice@example.com"));
            members.Add(new MailAddress("bob@example.com"));
            members.Add(new MailAddress("carol@example.com"));

            Console.WriteLine("Original members:");
            foreach (MailAddress address in members)
            {
                Console.WriteLine(address.Address);
            }

            // Specify the member to remove
            string addressToRemove = "bob@example.com";

            // Remove the specified member
            for (int i = members.Count - 1; i >= 0; i--)
            {
                if (string.Equals(members[i].Address, addressToRemove, StringComparison.OrdinalIgnoreCase))
                {
                    members.RemoveAt(i);
                }
            }

            Console.WriteLine("\nMembers after removal of '{0}':", addressToRemove);
            foreach (MailAddress address in members)
            {
                Console.WriteLine(address.Address);
            }

            // No external resources to dispose; collection will be cleaned up by GC
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
