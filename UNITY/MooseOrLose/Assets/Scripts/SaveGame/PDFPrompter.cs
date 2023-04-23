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
            PDFInfo info = CollectData();
            info.savePath = path;
            json = JsonConvert.SerializeObject(info);
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
        string exePath = Path.GetDirectoryName(Application.dataPath);
        exePath = Path.Join(exePath, "Assets\\.PDFGen\\net6.0\\PDFGen.exe");
        
        string tempFilePath = Path.GetDirectoryName(Application.dataPath);
        tempFilePath = Path.Join(tempFilePath, "Assets\\.PDFGen\\net6.0\\tempdata.json");
        GenerateJSONFile(tempFilePath, json);

        try {
            Process myProcess = new Process();
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = exePath;
            //myProcess.StartInfo.Arguments = json;
            myProcess.StartInfo.Arguments = "";
            myProcess.EnableRaisingEvents = true;
            myProcess.Start();
            myProcess.WaitForExit();
            int exitCode = myProcess.ExitCode;

            if (exitCode == 0) return true;
            path = Path.ChangeExtension(path, "json");
            GenerateJSONFile(path, json);
            return false;
            
        } catch (Exception e) {
            // Notify user that something went wrong, and log the exception for debug logging via dev contact. Let user choose to save data straight to JSON.
            // Save the JSON file just in case
            path = Path.ChangeExtension(path, "json");
            GenerateJSONFile(path, json);
            print(e);
            print("catch!");
            return false;
        }
    }
    
    private static bool GenerateJSONFile(string path, string json)
    {
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
