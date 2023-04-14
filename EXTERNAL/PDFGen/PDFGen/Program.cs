using System.Collections;
using System.Diagnostics;
using System.IO;

namespace PDFGen;

internal class Program
{
    static void Main(string[] args)
    {
        //PDFExporter.GetInstance().GeneratePDF(System.IO.File.ReadAllText("file.txt"));
        if (args.Length != 0)
            PDFExporter.GetInstance().GeneratePDF(args[0]);
        else 
            throw new Exception("No arguments passed to PDFGenerator.exe");
    }
}