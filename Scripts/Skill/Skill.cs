using System.Collections.Generic;
using Barterta.ToolScripts;
using Sirenix.Serialization;

namespace Barterta.Skill
{
    public class Skill
    {
        [OdinSerialize] private int _currentXP;
        public int Level;
        public int MaxLevel;
        public string Name;

        public Skill(string name, int level = 1, int currentXP = 0)
        {
            Name = name;
            Level = level;
            CurrentXP = currentXP;
        }

        public int CurrentXP
        {
            get => _currentXP;
            set
            {
                var tmpValue = value;
                //Level up loop, will triggered until current XP < level need
                while (tmpValue >= MaxXP)
                {
                    tmpValue -= MaxXP;
                    //will change Max XP
                    Level++;
                }

                _currentXP = tmpValue;
            }
        }

        public int MaxXP => Level * Constant.MaxXPConstant;

        public List<SkillAttr> AttrList = new(5);
    }
}