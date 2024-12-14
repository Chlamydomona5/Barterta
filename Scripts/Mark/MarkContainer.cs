using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Barterta.Mark
{
    [CreateAssetMenu(fileName = "SignContainer", menuName = "SO/SignContainer")]
    public class MarkContainer : ScriptableObject
    {
        [FormerlySerializedAs("signList")] public List<Mark> markList;

        public void Register(Mark mark)
        {
            if (markList.Contains(mark))
                //Debug.Log("This sign has already been registered");
                return;
            //Register to list and give a id
            markList.Add(mark);
            mark.id = markList.IndexOf(mark);
        }

        public void Unregister(Mark mark)
        {
            if (!markList.Contains(mark))
            {
                Debug.Log("This sign has not been registered");
                return;
            }

            //Register to list and give a id
            markList.Remove(mark);
            mark.id = -1;
        }
    }
}