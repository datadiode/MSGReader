using System.Text;
using System.IO;
using NUnit.Framework;

namespace MsgReaderTests
{
    [TestFixture]
    public class CodePagesInit
    {
        //[AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}