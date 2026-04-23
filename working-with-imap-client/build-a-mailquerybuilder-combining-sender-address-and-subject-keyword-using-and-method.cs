using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the query builder
            MailQueryBuilder builder = new MailQueryBuilder();

            // Add condition for sender address
            builder.From.Contains("sender@example.com");

            // Add condition for subject keyword
            builder.Subject.Contains("Important");

            // Get the combined query (AND is the default behavior)
            MailQuery query = builder.GetQuery();

            Console.WriteLine("Generated query: " + query);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
