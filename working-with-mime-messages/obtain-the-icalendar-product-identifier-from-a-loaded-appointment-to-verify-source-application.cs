using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            string icsPath = "appointment.ics";

            // Ensure the directory for the iCalendar file exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(icsPath));
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    string placeholder = "BEGIN:VCALENDAR\r\nPRODID:-//Aspose Ltd//iCalendar Builder (v3.0)//EN\r\nEND:VCALENDAR";
                    File.WriteAllText(icsPath, placeholder, Encoding.UTF8);
                    Console.WriteLine($"Placeholder iCalendar file created at '{icsPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the iCalendar file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            // Serialize the appointment to obtain the raw iCalendar content
            string icsContent;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    appointment.Save(ms, AppointmentSaveFormat.Ics);
                    ms.Position = 0;
                    using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        icsContent = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to serialize appointment: {ex.Message}");
                return;
            }

            // Extract the PRODID line
            string productId = null;
            using (StringReader sr = new StringReader(icsContent))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("PRODID:", StringComparison.OrdinalIgnoreCase))
                    {
                        productId = line.Substring("PRODID:".Length).Trim();
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(productId))
            {
                Console.WriteLine($"iCalendar Product Identifier: {productId}");
            }
            else
            {
                Console.WriteLine("Product identifier not found in the iCalendar content.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
