using System.Collections;
using System.Diagnostics;
using System.IO;

namespace PDFGen;

internal class Program
{
    static void Main(string[] args)
    {
        //File.WriteAllText("wtf.txt", "Current directory: " + Directory.GetCurrentDirectory());
        PDFExporter.GetInstance().GeneratePDF();
    }
}