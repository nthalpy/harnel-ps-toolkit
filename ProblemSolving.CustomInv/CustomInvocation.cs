using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ProblemSolving.CustomInv
{
    public class CustomInvocation
    {
        [Test]
        public void TC1()
        {
            // copy this and uncomment
            // var input = "";
            // var output = "";

            var input = "4 2\r\n1924\r\n";
            var output = "94\r\n";

            TrySolve(input, output);
        }
        [Test]
        public void TC2()
        {
            // copy this and uncomment
            // var input = "";
            // var output = "";

            var input = "7 3\r\n1231234\r\n";
            var output = "3234\r\n";

            TrySolve(input, output);
        }
        [Test]
        public void TC3()
        {
            // copy this and uncomment
            // var input = "";
            // var output = "";

            var input = "10 4\r\n4177252841\r\n";
            var output = "775841\r\n";

            TrySolve(input, output);
        }
        [Test]
        public void TC4()
        {
            // copy this and uncomment
            // var input = "";
            // var output = "";

            var input = "10 9\r\n4177252841\r\n";
            var output = "8\r\n";

            TrySolve(input, output);
        }

        private void TrySolve(string input, string output)
        {
            using var inms = new MemoryStream(Encoding.UTF8.GetBytes(input));
            using var sr = new StreamReader(inms);

            using var outms = new MemoryStream();
            using var sw = new StreamWriter(outms);

            Program.Solve(sr, sw);
            sw.Flush();

            outms.Seek(0, SeekOrigin.Begin);
            var read = Encoding.UTF8.GetString(outms.GetBuffer(), 0, (int)outms.Length);

            var trimmed = String.Join(Environment.NewLine, read.Trim().Split(Environment.NewLine).Select(l => l.Trim()));
            Assert.That(trimmed, Is.EqualTo(output.Trim()));
        }
    }
}