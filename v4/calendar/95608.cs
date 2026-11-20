using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;
class Program {
 static void Main() {
  try {
  string inputPath = Path.Combine(Environment.CurrentDirectory, "input.msg");
  string outputPath = Path.Combine(Environment.CurrentDirectory, "updated.msg");
  if (!File.Exists(inputPath)) throw new FileNotFoundException("Meeting MSG is required", inputPath);
  Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
  MapiMessage message = MapiMessage.Load(inputPath);
  if (message.SupportedType != MapiItemType.Calendar) throw new InvalidDataException("Expected calendar MSG");
  MapiCalendar calendar = (MapiCalendar)message.ToMapiMessageItem();
  calendar.Attendees.AppointmentRecipients.Add("new@example.com", "New Attendee", MapiRecipientType.MAPI_TO);
  calendar.Save(outputPath, AppointmentSaveFormat.Msg);
  } catch (Exception ex) { Console.Error.WriteLine(ex.Message); Environment.ExitCode = 1; }
 }
}
