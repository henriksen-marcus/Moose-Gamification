using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public enum SaveType
{
    PDF,
    JSON
}

public class PDFPrompter : MonoBehaviour
{
    public static bool SaveSimulationResults(string path, SaveType type)
    {
        string json;
        
        try
        {
            json = JsonConvert.SerializeObject(CollectData());
        }
        catch (Exception e)
        {
            return false;
        }
        
        switch (type)
        {
            case SaveType.PDF:
                return GeneratePDFFile(path, json);
                break;
            case SaveType.JSON:
                return GenerateJSONFile(path, json);
                break;
        }

        return false;
    }

    private static bool GeneratePDFFile(string path, string json)
    {
        //string path = System.IO.Path.GetDirectoryName(Application.dataPath);
        //path = System.IO.Path.Join(path, "Assets\\.PDFGen\\PDFGen.exe");
        
        try {
            Process myProcess = new Process();
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = path;
            myProcess.StartInfo.Arguments = json;
            myProcess.EnableRaisingEvents = true;
            myProcess.Start();
            myProcess.WaitForExit();
            int exitCode = myProcess.ExitCode;
            return exitCode == 0;
        } catch (Exception e) {
            // Notify user that something went wrong, and log the exception for debug logging via dev contact. Let user choose to save data straight to JSON.
            // Save the JSON file just in case
            GenerateJSONFile(path, json);
            print(e);
            return false;
        }
    }
    
    private static bool GenerateJSONFile(string path, string json)
    {
        //string path = Path.GetDirectoryName(Application.dataPath);
        //path = Path.Join(path, "Assets\\.SavedDocuments");

        try
        {
            File.WriteAllText(path, json);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private static PDFInfo CollectData()
    {
        int birch = 0, spruce = 0, pine = 0;
        ForestManager.instance.GetNumTrees(ref birch, ref spruce, ref pine);
        
        var info = new PDFInfo
        {
            Rules = RuleManager.Instance.Rules,
            NumMoose = ElgManager.instance.elg_list.Count,
            MooseMaleRatio = ElgManager.instance.GetMaleRatio(),
            NumWolves = UlvManager.instance.ulv_list.Count,
            NumBirchTrees = birch,
            NumSpruceTrees = spruce,
            NumPineTrees = pine,
            SimDurationDays = TimeManager.instance.GetDay(),
            SimDurationMonths = TimeManager.instance.GetMonth(),
            SimDurationYears = TimeManager.instance.GetYear(),
            NumHunters = JegerManager.instance.jeger_list.Count,
        };
        info.MooseFemaleRatio = 1 - info.MooseMaleRatio;
        return info;
    }
}
