using System; using System.IO; using Aspose.Email; using Aspose.Email.Storage.Mbox;
class Program { static void Main() { try { string path = "Messages.mbox"; if (!File.Exists(path)) { Console.Error.WriteLine("Missing input archive"); return; }
Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path)));
using (MailMessage m = new MailMessage()) { m.From = new MailAddress("sender@example.com"); m.To.Add("to@example.com"); m.Subject = "Appended message"; m.Body = "Appended body"; m.IsDraft = false;
using (MboxStorageWriter writer = new MboxrdStorageWriter(path, new MboxSaveOptions())) { writer.WriteMessage(m); } }
} catch (Exception ex) { Console.Error.WriteLine(ex.Message); throw; } } }
