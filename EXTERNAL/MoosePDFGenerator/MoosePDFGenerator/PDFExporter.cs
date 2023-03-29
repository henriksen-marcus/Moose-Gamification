using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Text.Json;

public sealed class PDFExporter
{
    public static PDFExporter Instance { get; }

    /* The current y value that we have written to. Used for keeping
     track of where we have written data so that we don't overwrite. */
    private int _currentY;
    private int _pageMargin = 50;

    /* The name of the pdf file. */
    private string _fileName = "MooseSimulatorReport.pdf";
    /* The name displayed in the browser tab in your internet browser. */
    private string _browserTitle = "Simulator Report";



    public void GeneratePDF(string args)
    {
        if (string.IsNullOrEmpty(args)) return;

        // Needs to be done for PDFSharp to work.
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Parse JSON argument data
        var jsonElement = JsonDocument.Parse(args).RootElement;


        // Create a PDF document
        var document = new PdfDocument();
        document.Info.Title = _browserTitle;

        // Create an empty page
        var page = document.AddPage();

        // Get an XGraphics object for drawing
        var gfx = XGraphics.FromPdfPage(page);

        var defaultFont = new XFont("Arial", 14, XFontStyle.Regular);

        // Top banner "Moose Simulator"
        gfx.DrawImage(XImage.FromFile("banner.jpg"), 20, _currentY += 20, page.Width*0.93, page.Width*0.93/5.8090909);

        // Timestamp
        var currentDateTime = DateTime.Now.ToString("dd:MM:yy HH:mm");
        
        gfx.DrawString(currentDateTime, defaultFont, XBrushes.Black, new XRect(_pageMargin, _currentY += 20, page.Width, 50), XStringFormats.TopRight);
        
        // Title
        gfx.DrawString("Moose Simulator Report", 
            new XFont("Verdana", 24, XFontStyle.Regular), 
            XBrushes.Black, 
            new XRect(0, 0, page.Width, page.Height));

        var subheadingFont = new XFont("Arial", 16, XFontStyle.Bold);
        gfx.DrawString("Subheading", subheadingFont, XBrushes.Black, new XRect(_pageMargin, _currentY += 20, page.Width, 50), XStringFormats.TopLeft);

        XFont listFont = new XFont("Arial", 12, XFontStyle.Regular);
        string[] points = { "Point 1", "Point 2", "Point 3" };
        _currentY += 30;
        for (var i = 0; i < points.Length; i++)
        {
            gfx.DrawString("• " + points[i], listFont, XBrushes.Black, new XRect(_pageMargin + 15, _currentY + (i * 20), page.Width - 100, 20), XStringFormats.TopLeft);
        }

        // Save the document
        var filename = GetSafeFilename("C:/bin", _fileName);
        document.Save(filename);
    }

    private string GetSafeFilename(string path, string filename)
    {
        string[] files = System.IO.Directory.GetFiles(path, filename);

        int count = 0;
        foreach (string file in files)
        {
            string[] parts = file.Split('.');
            if (parts.Length >= 2 && int.TryParse(parts[parts.Length - 2].Substring(filename.Length), out int fileCount))
            {
                count = Math.Max(count, fileCount + 1);
            }
        }

        string newfilename = filename.Insert(filename.LastIndexOf('.'), count.ToString());

        return newfilename;
    }
}