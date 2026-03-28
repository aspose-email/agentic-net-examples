using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string icsPath = "sample.ics";
            string msgPath = "output.msg";

            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Error: File not found – {icsPath}");
                return;
            }

            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Error loading appointment: {loadEx.Message}");
                return;
            }

            using (MapiMessage mapiMessage = appointment.ToMapiMessage())
            {
                try
                {
                    mapiMessage.Save(msgPath);
                    Console.WriteLine($"MSG file saved to {msgPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error saving MSG: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
