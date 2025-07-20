using UnityEngine;

[CreateAssetMenu(fileName = "UIScriptableObject", menuName = "Scriptable Objects/UIScriptableObject")]
public class UIScriptableObject : ScriptableObject
{
    public int NumLights;
    public int NumGen;
    public int NumMetal;

    public void Reset()
    {
        NumLights = 0;
        NumGen = 0;
        NumMetal = 0;
    }
}
