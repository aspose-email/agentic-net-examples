using Aspose.Email;
using System;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a yearly recurrence pattern
            MapiCalendarYearlyAndMonthlyRecurrencePattern yearlyPattern = new MapiCalendarYearlyAndMonthlyRecurrencePattern();
            yearlyPattern.StartDate = new DateTime(2024, 1, 1);
            yearlyPattern.EndDate = new DateTime(2025, 12, 31);
            yearlyPattern.EndType = MapiCalendarRecurrenceEndType.EndAfterDate;

            Console.WriteLine($"Recurrence configured: Start = {yearlyPattern.StartDate:d}, End = {yearlyPattern.EndDate:d}, EndType = {yearlyPattern.EndType}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
