using System; using System.IO; using System.Collections.Generic;
using Aspose.Email; using Aspose.Email.Mapi; using Aspose.Email.Storage.Pst;
class Program {
 static void Main() {
  try {
   string sourceDir = "LegalMails";
   if (!Directory.Exists(sourceDir)) { Console.Error.WriteLine("Source directory missing: " + sourceDir); Environment.ExitCode=1; return; }
   string pstPath = "LegalMails.pst";
   string outputDir = Path.GetDirectoryName(Path.GetFullPath(pstPath));
   if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
   using (PersonalStorage pst = File.Exists(pstPath) ? PersonalStorage.FromFile(pstPath) : PersonalStorage.Create(pstPath, FileFormatVersion.Unicode)) {
    FolderInfo legalFolder = pst.RootFolder.GetSubFolder("Legal") ?? pst.RootFolder.AddSubFolder("Legal");
    foreach (MailMessage message in GetMessages(sourceDir))
     using (message) using (MapiMessage mapi = MapiMessage.FromMailMessage(message)) legalFolder.AddMessage(mapi);
   }
  } catch (Exception ex) { Console.Error.WriteLine(ex.Message); Environment.ExitCode=1; }
 }
 private static IEnumerable<MailMessage> GetMessages(string directoryPath) {
  if (!Directory.Exists(directoryPath)) yield break;
  foreach (string path in Directory.GetFiles(directoryPath, "*.eml", SearchOption.TopDirectoryOnly)) {
   if (!File.Exists(path)) continue;
   MailMessage message;
   try { message = MailMessage.Load(path); } catch (Exception ex) { Console.Error.WriteLine("Cannot load " + path + ": " + ex.Message); Environment.ExitCode=1; continue; }
   yield return message;
  }
 }
}
