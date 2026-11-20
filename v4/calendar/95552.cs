using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
class Program {
 static void Main() {
  try {
   string output = Path.Combine(Environment.CurrentDirectory, "appointment.msg");
   Directory.CreateDirectory(Path.GetDirectoryName(output)!);
   var attendees = new MailAddressCollection();
   attendees.Add(new MailAddress("attendee@example.com"));
   var appointment = new Appointment("Room A", new DateTime(2027,1,2,10,0,0,DateTimeKind.Utc), new DateTime(2027,1,2,11,0,0,DateTimeKind.Utc), new MailAddress("organizer@example.com"), attendees);
   appointment.Summary = "HTML meeting";
   appointment.HtmlDescription = "<html><body><p>Meeting <b>details</b></p></body></html>";
   var options = new AppointmentMsgSaveOptions();
   appointment.Save(output, options);
  } catch (Exception ex) { Console.Error.WriteLine(ex.Message); Environment.ExitCode = 1; }
 }
}
