using System.Collections;
using System.Diagnostics;
using System.IO;

namespace PDFGen;

internal class Program
{
    static void Main(string[] args)
    {
        PDFExporter.GetInstance().GeneratePDF();
    }
}