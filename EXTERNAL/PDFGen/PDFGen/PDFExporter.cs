﻿using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using PDFGen;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Visitors;
using MigraDoc.Rendering;


public sealed class PDFExporter
{
    private static PDFExporter? _instance = null;
    
    // The name of the Unity application
    string simTitle = "Moose Simulator";

    private PDFExporter()
    {
        // Needs to be done for PDFSharp and MigraDoc to work.
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    }
    
    public static PDFExporter GetInstance() => _instance ??= new PDFExporter();
    
    public void GeneratePDF(string args)
    {
        // Create a new PDF document
        var doc = new Document();
        doc.Info.Title = "Simulation Report";
        doc.Info.Author = simTitle;

        var page1 = doc.AddSection();
        page1.PageSetup.TopMargin = 7;
        
        // Image section
        var imgParagraph = page1.AddParagraph();
        imgParagraph.Format.Alignment = ParagraphAlignment.Center;
        var bannerimg = imgParagraph.AddImage("banner.png");
        bannerimg.LockAspectRatio = true;
        bannerimg.Width = doc.DefaultPageSetup.PageWidth.Point - 15;

        
        // Title
        var title = page1.AddParagraph("Simulator Report");
        title.Format.SpaceBefore = 0;
        title.Format.Alignment = ParagraphAlignment.Center;
        title.Format.Font = new Font("Arial")
        {
            Size = 24,
            Bold = true
        };

        // Add a small text for the current date on the right side of the page
        var date = page1.AddParagraph(DateTime.Now.ToString("dd.MM.yyyy"));
        date.Format.Alignment = ParagraphAlignment.Right;
        date.Format.SpaceBefore = 2;
        date.Format.SpaceAfter = 5;

        // Add a subheading on the left side
        var subheading = page1.AddParagraph("Rules");
        subheading.Format.Font.Size = 16;
        subheading.Format.Font.Bold = true;
        
        var rulesDesc = page1.AddParagraph("The following table shows the rules that were set and on which day they were changed.");
        rulesDesc.Format.Font.Size = 10;
        rulesDesc.Format.SpaceBefore = 2;
        rulesDesc.Format.SpaceAfter = 8;
        
        // Read JSON data
        var info = JsonConvert.DeserializeObject<PDFInfo>(args);
        if (info == null) throw new Exception("Error: Could not parse JSON data.");
        
        // Rules table
        var ruleTable = new Table();
        ruleTable.Borders.Width = 1;
        const float cWidth = 6;
        const float cHWidth = cWidth / 2;
    
        var column1 = ruleTable.AddColumn(Unit.FromCentimeter(cWidth));
        var column2 = ruleTable.AddColumn(Unit.FromCentimeter(cWidth));
        //var column3 = mainTable.AddColumn(Unit.FromCentimeter(colWidth));

        var titleRow = ruleTable.AddRow();
        titleRow.Format.Font.Bold = true;
        titleRow.Cells[0].AddParagraph("Rule");

        // I am a genius
        var dCell = titleRow.Cells[1];
        var t = new Table();
        t.Borders.Width = 0;
        t.AddColumn(Unit.FromCentimeter(cHWidth));
        t.AddColumn(Unit.FromCentimeter(cHWidth));
        var r = t.AddRow();
        r.Cells[0].AddParagraph("Value").Format.Font.Bold = true;
        r.Cells[1].AddParagraph("Day set").Format.Font.Bold = true;
        dCell.Elements.Add(t);
        
        foreach (dynamic rule in info.Rules)
        {
            var row = ruleTable.AddRow();
        
            // Rule title
            Paragraph p = new Paragraph();
            p.AddText((string)rule.Name);
            p.Format.Font.Bold = true;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].Add(p);
            
            // Value and day changed
            var table = new Table();
            var col1 = table.AddColumn(Unit.FromCentimeter(cWidth / 2));
            var col2 = table.AddColumn(Unit.FromCentimeter(cWidth / 2));
        
            foreach(dynamic interval in rule.Intervals)
            {
                var row2 = table.AddRow();
                row2.Cells[0].AddParagraph((string)interval.Value);
                row2.Cells[1].AddParagraph("Day " + (string)interval.StartDay);
            }
        
            row.Cells[1].Elements.Add(table);
        }
        page1.Add(ruleTable);

            // Add a subheading on the left side: Statistics
        var statHeading = page1.AddParagraph("Statistics on simulation end");
        statHeading.Format.Font.Bold = true;
        statHeading.Format.Font.Size = 16;
        statHeading.Format.SpaceBefore = 15;
        statHeading.Format.SpaceAfter = 6;

        var para = new Paragraph();
        para.AddFormattedText("Number of moose: ", TextFormat.Bold);
        para.AddText(info.NumMoose.ToString());
        page1.Add(para);
        
        para = new Paragraph();
        para.AddFormattedText("Number of wolves: ", TextFormat.Bold);
        para.AddText(info.NumWolves.ToString());
        page1.Add(para);
        
        para = new Paragraph();
        para.AddFormattedText("Number of hunters: ", TextFormat.Bold);
        para.AddText(info.NumHunters.ToString());
        page1.Add(para);
        
        // <br>
        page1.AddParagraph();
        
        para = new Paragraph();
        para.AddFormattedText("Moose % Males: ", TextFormat.Bold);
        para.AddText((int)info.MooseMaleRatio * 100 + "%");
        page1.Add(para);
        
        para = new Paragraph();
        para.AddFormattedText("Moose % Females: ", TextFormat.Bold);
        para.AddText((int)info.MooseFemaleRatio * 100 + "%");
        page1.Add(para);
        
        // <br>
        page1.AddParagraph();
        
        para = new Paragraph();
        para.AddFormattedText("Number of trees:", TextFormat.Bold);
        page1.Add(para);
        page1.AddParagraph("   - Birch: " + info.NumBirchTrees);
        page1.AddParagraph("   - Spruce: " + info.NumSpruceTrees);
        page1.AddParagraph("   - Pine: " + info.NumPineTrees);
        
        // <br>
        page1.AddParagraph();
        
        para = new Paragraph();
        para.AddFormattedText("Simulation duration: ", TextFormat.Bold);
        
        string yearFix = info.SimDurationYears == 1 ? " year" : " years";
        string monthFix = info.SimDurationMonths == 1 ? " month" : " months";
        string dayFix = info.SimDurationDays == 1 ? " day" : " days";
        
        string year = info.SimDurationYears > 0 ? info.SimDurationYears +  yearFix : "";
        string months = info.SimDurationMonths > 0 ? info.SimDurationMonths + monthFix : "";
        string days = info.SimDurationDays + dayFix;
        
        para.AddText(year + " " + months + " " + days);
        page1.Add(para);
        
        // Set the position of the text frame to the lower left corner of the page
        TextFrame footer = page1.AddTextFrame();
        footer.RelativeVertical = RelativeVertical.Page;
        footer.RelativeHorizontal = RelativeHorizontal.Page;
        footer.Top = ShapePosition.Bottom;
        footer.Left = ShapePosition.Left;
        footer.Width = Unit.FromCentimeter(10);
        footer.Height = Unit.FromCentimeter(1);
        
        para = new Paragraph();
        para.AddText("Generated by " + simTitle);
        para.Format.Alignment = ParagraphAlignment.Left;
        para.Format.Font.Size = 8;
        para.Format.Font.Italic = true;
        footer.Add(para);

        SavePDF(doc);
    }

    private void SavePDF(Document doc)
    {
        // Save the document to a file
        var renderer = new PdfDocumentRenderer(true);
        renderer.Document = doc;
        renderer.RenderDocument();
        renderer.PdfDocument.Save("Assets\\.SavedDocuments\\output.pdf");
        //var filename = GetSafeFilename("Documents", _fileName);
    }

    private static string GetSafeFilename(string path, string filename)
    {
        path = string.IsNullOrEmpty(path) ? Directory.GetCurrentDirectory() : Path.GetFullPath(path);

        string baseFileName = Path.GetFileNameWithoutExtension(filename);
        string extension = Path.GetExtension(filename);
        string newFilename = filename;
        int count = 0;

        while (File.Exists(Path.Combine(path, newFilename)))
        {
            count++;
            newFilename = $"{baseFileName}{count}{extension}";
        }

        return Path.Combine(path, newFilename);
    }
}