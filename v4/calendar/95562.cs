using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
class Program {
 static void Main() {
  try {
   string output = Path.Combine(Environment.CurrentDirectory, "appointments.ics");
   Directory.CreateDirectory(Path.GetDirectoryName(output)!);
   var attendees = new MailAddressCollection();
   attendees.Add(new MailAddress("attendee@example.com"));
   var appointments = new List<Appointment> {
    new Appointment("Room A", new DateTime(2027,1,2,10,0,0,DateTimeKind.Utc), new DateTime(2027,1,2,11,0,0,DateTimeKind.Utc), new MailAddress("organizer@example.com"), attendees) { Summary = "First meeting" },
    new Appointment("Room B", new DateTime(2027,1,3,10,0,0,DateTimeKind.Utc), new DateTime(2027,1,3,11,0,0,DateTimeKind.Utc), new MailAddress("organizer@example.com"), attendees) { Summary = "Second meeting" }
   };
   using (var writer = new CalendarWriter(output)) {
    foreach (Appointment appointment in appointments) writer.Write(appointment);
   }
  } catch (Exception ex) { Console.Error.WriteLine(ex.Message); Environment.ExitCode = 1; }
 }
}
