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

            var input = "3 5\r\nXPA\r\n1 P 3\r\n1 P 5\r\n2 2 5\r\n1 Y 2\r\n2 1 2\r\n";
            var output = "PPAP\r\nXY\r\n";

            TrySolve(input, output);
        }
        [Test]
        public void TC2()
        {
            // copy this and uncomment
            // var input = "";
            // var output = "";

            var input = "27 7\r\nICPCASIAPACIFICCHAMPIONSHIP\r\n2 5 8\r\n2 5 11\r\n1 A 5\r\n1 P 6\r\n1 A 7\r\n1 C 8\r\n2 1 8\r\n";
            var output = "ASIA\r\nPACIFIC\r\nICPCAPAC\r\n";

            TrySolve(input, output);
        }
        [Test]
        public void TC3()
        {
            // copy this and uncomment
            // var input = "";
            // var output = "";

            var input = "1 3\nA\n2 1 1\n1 B 1\n2 1 1";
            var output = "A\r\nB\r\n";

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

            output = String.Join(Environment.NewLine, output.Trim().Split(Environment.NewLine).Select(l => l.Trim()));
            var trimmed = String.Join(Environment.NewLine, read.Trim().Split(Environment.NewLine).Select(l => l.Trim()));
            Assert.AreEqual(trimmed, output);
        }
    }
}