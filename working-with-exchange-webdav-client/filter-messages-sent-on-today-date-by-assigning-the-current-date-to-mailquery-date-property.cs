using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Create a query builder
            MailQueryBuilder builder = new MailQueryBuilder();

            // Build a query that filters messages sent on today's date
            MailQuery mailQuery = builder.SentDate.On(DateTime.Today);

            // Output the generated query string
            Console.WriteLine("Generated MailQuery: " + mailQuery.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
