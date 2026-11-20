using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
class Program {
 static void Main() {
  try {
   string outputPath="amp_carousel_form.eml";
   string outputDir=Path.GetDirectoryName(Path.GetFullPath(outputPath));
   if(!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
   for(int i=1;i<=3;i++) if(!File.Exists("image"+i+".png")) throw new FileNotFoundException("image"+i+".png");
   using(AmpMessage message=new AmpMessage()) {
    message.From=new MailAddress("sender@example.com"); message.To.Add("reader@example.com");
    message.Subject="Carousel followed by form"; message.Body="Three image carousel and subscription form";
    message.HtmlBody="<html><body><img src=\"cid:image1\"><img src=\"cid:image2\"><img src=\"cid:image3\"><p>Subscribe using the AMP form.</p></body></html>";
    AmpCarousel carousel=new AmpCarousel(800,300); carousel.Attributes.Layout=LayoutType.Responsive;
    for(int i=1;i<=3;i++) {
     AmpImage image=new AmpImage(800,300) {Src="https://example.com/image"+i+".png", Alt="Sample image "+i};
     if(!image.IsValid) throw new Exception("Invalid AMP image");
     carousel.Images.Add(image);
     Attachment embedded=new Attachment("image"+i+".png", "image/png"); embedded.ContentId="image"+i; embedded.ContentDisposition.Inline=true;
     message.Attachments.Add(embedded);
    }
    message.AddAmpComponent(carousel);
    AmpForm form=new AmpForm {Method=FormMethod.Post, Target=FormTarget.Top, ActionXhr="https://example.com/subscribe"};
    form.Fieldset.Add(new FormField {InputType="email",Name="email",Label="Email",IsRequired=true});
    form.Fieldset.Add(new FormField {InputType="submit",Value="Subscribe"});
    message.AddAmpComponent(form);
    using(FileStream stream=new FileStream(outputPath,FileMode.Create,FileAccess.Write)) message.Save(stream,SaveOptions.DefaultEml);
   }
  } catch(Exception ex) {Console.Error.WriteLine(ex);Environment.ExitCode=1;}
 }
}
