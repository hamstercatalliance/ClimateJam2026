using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class QuestSO : ScriptableObject
{
    public string questName;
    public string questDescription;
    public string questID;
    public int currencyAward;
    public int sympathyAward;
}
