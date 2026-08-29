using System;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            MailQueryBuilder builder = new MailQueryBuilder();
            // Case-sensitive match: ignoreCase = false
            builder.From.Equals("John.Doe@Example.com", false);
            MailQuery query = builder.GetQuery();
            Console.WriteLine("Generated MailQuery:");
            Console.WriteLine(query.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
