using Aspose.Email;
using System;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define the date range
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);

            // Initialize the MailQueryBuilder
            MailQueryBuilder builder = new MailQueryBuilder();

            // Add criteria: sender domain contains "example.com"
            builder.From.Contains("example.com");

            // Add criteria: subject contains "Report"
            builder.Subject.Contains("Report");

            // Add criteria: sent date is on or after startDate
            builder.SentDate.Since(startDate);

            // Add criteria: sent date is on or before endDate (using On with the end date)
            builder.SentDate.On(endDate);

            // Build the combined query (AND of all criteria)
            MailQuery query = builder.GetQuery();

            // Output the generated query string
            Console.WriteLine("Generated MailQuery:");
            Console.WriteLine(query.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
