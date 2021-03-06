using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using MsgReader;
using System.Collections.Generic;

namespace MsgReaderTests
{
    /*
         * Contains some basic content tests to make sure that the msg files are
         * being parsed correctly.  Sample emails contain chapter 1 from book 1
         * of War and Peace taken from Project Gutenberg.
         * 
         * Sample files are set to copy always content so will be accessible to
         * this project build location as relative path of SampleFiles/<file>.msg
         * 
         * MsgReader always returns HTML from .ExtractMsgEmailBody irrespective of
         * starting format, so all tests perform a simple tag cleanup regex.  This
         * will not work for all variations of HTML/XHTML, but will be fine for the simple
         * examples in this test.
         */

    [TestFixture]
    public class BasicContentTests
    {
        private static readonly Regex HtmlSimpleCleanup = new Regex(@"<[^>]*>", RegexOptions.Compiled);
        private const string SampleText = "Heavens! what a virulent attack!";

        [Test]
        public void Html_Content_Test()
        {
            using (Stream fileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "SampleFiles", "HtmlSampleEmail.msg")))
            {
                var msgReader = new Reader();
                var content = msgReader.ExtractMsgEmailBody(fileStream, ReaderHyperLinks.Both, null);
                content = HtmlSimpleCleanup.Replace(content, string.Empty);
                Assert.IsTrue(content.Contains(SampleText));
            }
        }

        [Test]
        public void Rtf_Content_Test()
        {
            using (Stream fileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "SampleFiles", "RtfSampleEmail.msg")))
            {
                var msgReader = new Reader();
                var content = msgReader.ExtractMsgEmailBody(fileStream, ReaderHyperLinks.Both, null);
                content = HtmlSimpleCleanup.Replace(content, string.Empty);
                Assert.IsTrue(content.Contains(SampleText));
            }
        }

        [Test]
        public void PlainText_Content_Test()
        {
            using (Stream fileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "SampleFiles", "TxtSampleEmail.msg")))
            {
                var msgReader = new Reader();
                var content = msgReader.ExtractMsgEmailBody(fileStream, ReaderHyperLinks.Both, null);
                content = HtmlSimpleCleanup.Replace(content, string.Empty);
                Assert.IsTrue(content.Contains(SampleText));
            }
        }

        [Test]
        public void Html_Content_Test_EML()
        {
            using (Stream fileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "SampleFiles", "test.eml")))
            {
                var message = MsgReader.Mime.Message.Load(fileStream);
                var From = message.Headers.From.Raw;
                var To = string.Join(";", message.Headers.To);
                var Cc = string.Join(";", message.Headers.Cc);
                var TextBody = message.TextBody?.GetBodyAsText();
                var HtmlBody = message.HtmlBody?.GetBodyAsText();
                var Subject = message.Headers.Subject;
                Assert.IsNotNull(From);
                Assert.IsNotEmpty(From);
                Assert.AreEqual(1, message.Headers.To.Count);
                Assert.AreEqual("\u62C9\u52FE\u7F51 <service@email.lagou.com>", From);
                Assert.AreEqual("tonyqus@163.com", To);
                Assert.AreEqual("\u4E0A\u6D77\u5206\u4F17\u5FB7\u5CF0\u5E7F\u544A\u4F20\u64AD\u6709\u9650\u516C\u53F8-\u9AD8\u7EA7.NET\u8F6F\u4EF6\u5DE5\u7A0B\u5E08\u62DB\u8058\u53CD\u9988\u901A\u77E5", Subject);
                Assert.IsTrue(HtmlBody.StartsWith("<html>\r\n"));
                Assert.IsNull(TextBody);
            }
        }
    }
}