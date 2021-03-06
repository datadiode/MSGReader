using System.IO;
using NUnit.Framework;
using MsgReader;

namespace MsgReaderTests
{
    [TestFixture]
    public class LoadTest
    {
        private DirectoryInfo _tempDirectory;

        [SetUp]
        public void Initialize()
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            _tempDirectory = Directory.CreateDirectory(tempDirectory);
        }

        [TearDown]
        public void Cleanup()
        {
            _tempDirectory.Delete(true);
        }

        [Test]
        public void Extract_100_Times()
        {
            for (var i = 0; i < 100; i++)
            {
                var msgReader = new Reader();
                var tempDirectory =
                    Directory.CreateDirectory(Path.Combine(_tempDirectory.FullName, Path.GetRandomFileName()));
                msgReader.ExtractToFolder(Path.Combine(TestContext.CurrentContext.TestDirectory, "SampleFiles", "EmailWithAttachments.msg"), tempDirectory.FullName);
            }
        }
    }
}