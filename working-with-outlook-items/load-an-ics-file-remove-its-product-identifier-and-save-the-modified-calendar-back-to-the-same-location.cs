using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            string icsPath = "sample.ics";

            // Guard file existence
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"File not found: {icsPath}");
                return;
            }

            // Load the iCalendar file
            Appointment appointment = Appointment.Load(icsPath);

            // Prepare save options without a product identifier
            AppointmentIcsSaveOptions saveOptions = new AppointmentIcsSaveOptions();
            saveOptions.ProductId = null; // Remove product identifier

            // Save the modified calendar back to the same file
            appointment.Save(icsPath, saveOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
