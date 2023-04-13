using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Newtonsoft.Json;

public class PDFPrompter : MonoBehaviour
{
    public void GeneratePDF()
    {
        string path = System.IO.Path.GetDirectoryName(Application.dataPath);
        path = System.IO.Path.Join(path, "Assets\\.PDFGen\\PDFGen.exe");

        string json = JsonConvert.SerializeObject(CollectData());
        
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
        } catch (Exception e){
            print(e);        
        }
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
