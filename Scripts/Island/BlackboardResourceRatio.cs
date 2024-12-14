using System.Collections.Generic;

namespace Barterta.Island
{
    public class BlackboardResourceRatio
    {
        public Dictionary<string, float> idToValueDict = new Dictionary<string, float>();
        
        //Constructor
        public BlackboardResourceRatio()
        {
            foreach (var id in BlackboardResource.IDList)
            {
                idToValueDict.Add(id, 0);
            }
        }

        //Define Add which is add value with same id
        public static BlackboardResourceRatio operator +(BlackboardResourceRatio a, BlackboardResourceRatio b)
        {
            var result = new BlackboardResourceRatio();
            foreach (var id in a.idToValueDict.Keys)
            {
                result.idToValueDict[id] = a.idToValueDict[id] + b.idToValueDict[id];
            }
            return result;
        }
    }
}