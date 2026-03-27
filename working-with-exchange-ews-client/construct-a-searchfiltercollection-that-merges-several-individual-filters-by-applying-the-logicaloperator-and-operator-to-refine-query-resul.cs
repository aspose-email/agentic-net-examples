using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Create an ExchangeQueryBuilder. Conditions added are combined with AND by default.
            ExchangeQueryBuilder builder = new ExchangeQueryBuilder();

            // Individual filters
            builder.From.Contains("alice@example.com");
            builder.Subject.Contains("Invoice");
            builder.HasFlags(ExchangeMessageFlag.IsRead);

            // Build the final query
            MailQuery query = builder.GetQuery();

            // Display the constructed query string
            Console.WriteLine("Constructed query: " + query);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
