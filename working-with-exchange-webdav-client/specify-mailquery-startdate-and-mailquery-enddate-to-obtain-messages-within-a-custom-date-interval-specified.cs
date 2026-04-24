using Aspose.Email;
using System;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define the custom date interval
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate   = new DateTime(2023, 1, 31);

            // Build the query using MailQueryBuilder
            MailQueryBuilder builder = new MailQueryBuilder();

            // SentDate >= startDate
            MailQuery startQuery = builder.SentDate.Since(startDate);

            // SentDate <= endDate
            MailQuery endQuery = builder.SentDate.On(endDate, DateComparisonType.ByDate);

            // Combine the two criteria with AND
            string combined = $"({startQuery} & {endQuery})";
            string query = combined;

            Console.WriteLine("Generated MailQuery:");
            Console.WriteLine(query.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
