using System;
using System.Collections.Generic;
using Barterta.Skill;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.UI.ScreenUI.Skill
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "SO/SkillData")]
    public class SkillData : SerializedScriptableObject
    {
        public List<global::Barterta.Skill.Skill> StartSkills;

        [Button]
        private void LoadFromCSV()
        {
            String fileData = System.IO.File.ReadAllText(Application.dataPath + "\\Settings\\Skill\\SkillData.csv");
            String[] lines = fileData.Split("\n"[0]);
            lines = lines[Range.StartAt(1)];

            StartSkills = new List<global::Barterta.Skill.Skill>();
            foreach (var line in lines)
            {
                String[] lineData = (line.Trim()).Split(",");
                //name
                var skill = new global::Barterta.Skill.Skill(lineData[0]);
                //attr
                int i = 1;
                while (i < lineData.Length && lineData[i] != String.Empty)
                {
                    //attr name
                    var attr = new SkillAttr(lineData[i], float.Parse(lineData[i + 1]));
                    i += 2;
                    skill.AttrList.Add(attr);
                }

                StartSkills.Add(skill);
            }
        }
    }
}