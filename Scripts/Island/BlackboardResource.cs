using System.Collections.Generic;

namespace Barterta.Island
{
    public class BlackboardResource
    {
        public static List<string> IDList = new List<string>()
        {
            "food",
            "water"
        };

        public Dictionary<string, float> IDToValueDict = new Dictionary<string, float>();

        public BlackboardResource()
        {
            foreach (var id in IDList)
            {
                IDToValueDict.Add(id, 0);
            }
        }

        //Define Multiply with Ratio
        public static BlackboardResource operator *(BlackboardResource a, BlackboardResourceRatio b)
        {
            var result = new BlackboardResource();
            foreach (var id in a.IDToValueDict.Keys)
            {
                result.IDToValueDict[id] = a.IDToValueDict[id] * b.idToValueDict[id];
            }
            return result;
        }
    }
}