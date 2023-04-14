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
    public void SaveSimulationResults(SaveType type)
    {
        string json = JsonConvert.SerializeObject(CollectData());

        switch (type)
        {
            case SaveType.PDF:
                GeneratePDFFile(json);
                break;
            case SaveType.JSON:
                GenerateJSONFile(json);
                break;
        }
    }

    private void GeneratePDFFile(string json)
    {
        string path = System.IO.Path.GetDirectoryName(Application.dataPath);
        path = System.IO.Path.Join(path, "Assets\\.PDFGen\\PDFGen.exe");
        
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
            int ExitCode = myProcess.ExitCode;
            print(ExitCode);
        } catch (Exception e) {
            // Notify user that something went wrong, and log the exception for debug logging via dev contact. Let user choose to save data straight to JSON.
            // Save the JSON file just in case
            GenerateJSONFile(json);
            print(e);        
        }
    }
    
    private void GenerateJSONFile(string json)
    {
        string path = System.IO.Path.GetDirectoryName(Application.dataPath);
        path = System.IO.Path.Join(path, "Assets\\.SavedDocuments");
        
        File.WriteAllText(path, json);
    }

    private PDFInfo CollectData()
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
