using Aspose.Email;
using System; using System.IO; using Aspose.Email.Storage.Mbox;
class Program { static void Main() { try { string inputPath = "Messages.mbox"; string outputPath = "summary.txt"; if (!File.Exists(inputPath)) { Console.Error.WriteLine("Missing input archive"); return; }
Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath)));
using (MboxStorageReader reader = MboxStorageReader.CreateReader(inputPath, new MboxLoadOptions())) using (StreamWriter output = new StreamWriter(outputPath)) { foreach (MboxMessageInfo info in reader.EnumerateMessageInfo()) { output.WriteLine(info.Subject + " | " + info.Date.ToUniversalTime().ToString("u")); } }
} catch (Exception ex) { Console.Error.WriteLine(ex.Message); throw; } } }
