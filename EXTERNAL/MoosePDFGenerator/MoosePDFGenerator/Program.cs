using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoosePDFGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PDFExporter.Instance.GeneratePDF(args[0]);
        }
    }
}
