
using System.IO;
using System.Linq;
using NUnit.Framework;
using MsgReader.Outlook;

namespace MsgReaderTests
{
    [TestFixture]
    public class RemoveAttachmentTests
    {
        [Test]
        public void RemoveAttachments()
        {
            using (var inputStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "SampleFiles", "EmailWith2Attachments.msg")))
            using (var inputMessage = new Storage.Message(inputStream, FileAccess.ReadWrite))
            {
                var attachments = inputMessage.Attachments.ToList();

                foreach (var attachment in attachments)
                    inputMessage.DeleteAttachment(attachment);

                using (var outputStream = new MemoryStream())
                {
                    inputMessage.Save(outputStream);
                    using (var outputMessage = new Storage.Message(outputStream))
                    {
                        var count = outputMessage.Attachments.Count;
                        Assert.IsTrue(count == 0);
                    }
                }
            }
        }
    }
}
