using System; using System.IO; using Aspose.Email; using Aspose.Email.Storage.Mbox;
class Program { static void Main() { try {
string path = "Messages.mbox"; if (File.Exists(path)) { throw new IOException("Output already exists"); } Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path)));
using (MailMessage m = new MailMessage()) { m.From = new MailAddress("sender@example.com"); m.To.Add("to@example.com"); m.CC.Add("cc@example.com"); m.Bcc.Add("bcc@example.com"); m.Subject = "Archive seed"; m.Date = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc); m.Body = "Seed body";
using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write)) using (MboxStorageWriter writer = new MboxrdStorageWriter(stream, new MboxSaveOptions())) { writer.WriteMessage(m); } }
} catch (Exception ex) { Console.Error.WriteLine(ex.Message); throw; } } }
