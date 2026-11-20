using System;
using System.IO;
using Aspose.Email.Mapi;
class Program {
    static void Main() {
        string input = Path.Combine(Environment.CurrentDirectory, "meeting.msg");
        string output = Path.Combine(Environment.CurrentDirectory, "meeting.ics");
        try {
            if (!File.Exists(input)) { Console.Error.WriteLine("Meeting MSG is missing."); Environment.ExitCode = 1; return; }
            Directory.CreateDirectory(Path.GetDirectoryName(output));
            using (MapiMessage message = MapiMessage.Load(input)) {
                if (message.SupportedType != MapiItemType.Calendar) { Console.Error.WriteLine("Input is not a meeting."); Environment.ExitCode = 1; return; }
                using (MapiCalendar calendar = (MapiCalendar)message.ToMapiMessageItem()) calendar.Save(output);
            }
        } catch (Exception ex) { Console.Error.WriteLine(ex.Message); Environment.ExitCode = 1; }
    }
}
