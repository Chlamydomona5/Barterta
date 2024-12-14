using UnityEngine;

public class TipController : MonoBehaviour
{
    [SerializeField] private Transform tipParent;
    [SerializeField] private TipUI tipPrefab;
    
    public void Tip(string str)
    {
        Instantiate(tipPrefab, tipParent).Init(str);
    }
}