using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Create an ExchangeQueryBuilder instance
            ExchangeQueryBuilder builder = new ExchangeQueryBuilder();

            // Add individual filters (AND is the default logical operator)
            builder.Subject.Contains("Quarterly Report");
            builder.From.Contains("john.doe@example.com");
            builder.Body.Contains("confidential");

            // Build the combined query
            MailQuery query = builder.GetQuery();

            // Output the resulting query string
            Console.WriteLine(query.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}