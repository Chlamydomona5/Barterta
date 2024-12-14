using System.Collections.Generic;
using Barterta.UI.ScreenUI.Skill;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Skill
{
    public class SkillContainer : SerializedMonoBehaviour
    {
        public List<Skill> Skills;
    
        private void Start()
        {
            Skills = new List<Skill>();
            foreach (var skill in Resources.Load<SkillData>("SkillData").StartSkills)
            {
                var newSkill = new Skill(skill.Name);
                newSkill.AttrList = skill.AttrList;
                Skills.Add(newSkill);
            }
        }

        public float GetAttribute(string attrName)
        {
            foreach (var skill in Skills)
            {
                SkillAttr attr;
                if ((attr = skill.AttrList.Find(x => x.Name.Equals(attrName))) != null)
                {
                    return attr.GetValue(skill.Level);
                }
            }
            Debug.LogAssertion("There is no attr name called " + attrName);
            return 0;
        }

        public void AddXPTo(string skillName, int xp)
        {
            var skill = Skills.Find(x => x.Name == skillName);
            if (skill != null) skill.CurrentXP += xp;
        }
    }
}