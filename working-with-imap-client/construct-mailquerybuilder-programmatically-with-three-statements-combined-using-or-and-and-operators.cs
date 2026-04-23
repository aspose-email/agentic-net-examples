using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the query builder.
            MailQueryBuilder builder = new MailQueryBuilder();

            // First two simple queries.
            MailQuery fromQuery = builder.From.Contains("alice@example.com");
            MailQuery subjectQuery = builder.Subject.Contains("Quarterly Report");

            // Combine the first two queries with OR.
            MailQuery orCombined = builder.Or(fromQuery, subjectQuery);

            // Third simple query.
            MailQuery bodyQuery = builder.Body.Contains("Confidential");

            // Build the final query: (first OR second) AND third.
            // The AND operation is the default intersection, so we compose it manually.
            string finalQueryString = $"({orCombined} & {bodyQuery})";
            MailQuery finalQuery = new MailQuery(finalQueryString);

            Console.WriteLine("Constructed MailQuery:");
            Console.WriteLine(finalQuery.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
