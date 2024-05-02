using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create/Clicker/NewUpgrade", fileName = "NewUpgrade")]
public class Upgrade : ScriptableObject
{
    public List<int> values;
    public List<int> costs;
}
