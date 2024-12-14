using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Barterta.UI.ScreenUI.Skill
{
    public class SkillPanel : MonoBehaviour
    {
        private List<SkillProgressUI> _slotsList;

        private void Awake()
        {
            _slotsList = GetComponentsInChildren<SkillProgressUI>().ToList();
        }

        public void UpdateAllSkills(List<global::Barterta.Skill.Skill> skillsdata)
        {
            //TODO: Some skill can't find name
            foreach (var data in skillsdata)
                _slotsList.Find(x => x.skillName == data.Name)?.UpdateSlots(data.Level, data.CurrentXP, data.MaxXP);
        }
    }
}