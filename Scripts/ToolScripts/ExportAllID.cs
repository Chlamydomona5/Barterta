using System.Collections.Generic;
using System.IO;
using Barterta.Core;
using Barterta.Island.SO;
using Barterta.Island.SO.UpgradeModule;
using Barterta.ItemGrid;
using UnityEditor;
using UnityEngine;

namespace Barterta.ToolScripts
{
    #if UNITY_EDITOR
    public class ExportAllID : MonoBehaviour
    {
        [MenuItem("Barter/ExportID")]
        public static void Export()
        {
            List<string> alreadyExists = new List<string>();
            StreamReader sr = new StreamReader(Application.dataPath + "\\Settings\\Language\\IDSum.csv");

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                alreadyExists.Add(line.Trim('\n','\r').Split(',')[0]);
            }
            sr.Close();
            sr.Dispose();

            StreamWriter sw = new StreamWriter(Application.dataPath + "\\Settings\\Language\\IDSum.csv",true);
            foreach (var groundable in Resources.LoadAll<IDBase>(""))
            {
                if(alreadyExists.Contains(groundable.ID)) continue;
                sw.Write(groundable.ID + "," + "\n");
                sw.Write(groundable.ID + "_introduction" + "," + "\n");
            }
            
            //Export IslandPreset
            foreach (var islandPreset in Resources.LoadAll<IslandPreset>(""))
            {
                if (alreadyExists.Contains(islandPreset.name)) continue;
                sw.Write(islandPreset.name + "," + "\n");
            }

            sw.Flush();
            sw.Close();
        }
    }
    #endif
}