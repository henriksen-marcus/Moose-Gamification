using System.Collections;
using System.Collections.Generic;

/* Class for storing variable information that
 is used in PDF export of simulator data. Version
 must be the same in the PDF exporter program as 
 in Unity. */
public class PDFInfo
{
    public ArrayList Rules = new ArrayList();

    public int NumMoose;
    public float MooseMaleRatio;
    public float MooseFemaleRatio;
    public int NumWolves;
    public int NumSpruceTrees;
    public int NumBirchTrees;
    public int NumPineTrees;
    public int NumMooseDeaths;
    public int NumWolvesDeaths;
    public int NumHunters;
    public int SimDurationDays;
    public int SimDurationMonths;
    public int SimDurationYears;

    public string savePath;
}

public class Rule<T>
{
    public string Name { get; set; }
    public List<Interval<T>> Intervals { get; set; }
}

public class Interval<T>
{
    public int StartDay { get; set; }
    public T Value { get; set; }
}