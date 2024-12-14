using Sirenix.Serialization;

namespace Barterta.Skill
{
    public class SkillAttr
    {
        public string Name;
        [OdinSerialize] private float _levelMultiply;

        public float GetValue(int level)
        {
            return (1 + level * _levelMultiply);
        }

        public SkillAttr(string name, float levelMultiply)
        {
            Name = name;
            _levelMultiply = levelMultiply;
        }
    }
}