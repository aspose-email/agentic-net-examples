using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Calendar;

namespace AsposeEmailAppointmentComparison
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // First appointment
                MailAddressCollection attendees1 = new MailAddressCollection();
                attendees1.Add(new MailAddress("alice@example.com"));
                attendees1.Add(new MailAddress("bob@example.com"));

                Appointment appointment1 = new Appointment(
                    location: "Conference Room",
                    startDate: new DateTime(2023, 10, 1, 9, 0, 0),
                    endDate: new DateTime(2023, 10, 1, 10, 0, 0),
                    organizer: new MailAddress("organizer@example.com"),
                    attendees: attendees1);
                appointment1.Summary = "Project Meeting";
                appointment1.Description = "Discuss project status";

                // Second appointment (potential duplicate)
                MailAddressCollection attendees2 = new MailAddressCollection();
                attendees2.Add(new MailAddress("bob@example.com"));
                attendees2.Add(new MailAddress("alice@example.com")); // different order

                Appointment appointment2 = new Appointment(
                    location: "Conference Room",
                    startDate: new DateTime(2023, 10, 1, 9, 0, 0),
                    endDate: new DateTime(2023, 10, 1, 10, 0, 0),
                    organizer: new MailAddress("organizer@example.com"),
                    attendees: attendees2);
                appointment2.Summary = "Project Meeting";
                appointment2.Description = "Discuss project status";

                bool duplicates = AreDuplicates(appointment1, appointment2);
                Console.WriteLine(duplicates ? "Appointments are duplicates." : "Appointments are not duplicates.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static bool AreDuplicates(Appointment a, Appointment b)
        {
            if (a == null || b == null)
                return false;

            bool subjectEqual = string.Equals(a.Summary, b.Summary, StringComparison.Ordinal);
            bool startEqual = a.StartDate == b.StartDate;

            List<string> listA = a.Attendees.Select(addr => addr.Address).OrderBy(addr => addr).ToList();
            List<string> listB = b.Attendees.Select(addr => addr.Address).OrderBy(addr => addr).ToList();

            bool attendeesEqual = listA.SequenceEqual(listB, StringComparer.Ordinal);

            return subjectEqual && startEqual && attendeesEqual;
        }
    }
}
