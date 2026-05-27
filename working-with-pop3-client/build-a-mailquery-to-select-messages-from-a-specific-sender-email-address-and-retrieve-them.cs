using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Build a query to find messages from a specific sender
            MailQueryBuilder builder = new MailQueryBuilder();
            // Use Contains with ignoreCase = true for case‑insensitive match
            builder.From.Contains("sender@example.com", true);
            MailQuery query = builder.GetQuery();

            // Output the generated query string
            Console.WriteLine("Generated MailQuery: " + query.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
