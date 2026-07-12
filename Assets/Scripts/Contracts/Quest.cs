using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Quest : MonoBehaviour
{
    public QuestSO questSO;
    public bool isInitiated;
    public bool isCompleted;
    public static event EventHandler OnQuestInitiated;
    public static event EventHandler OnQuestCompleted;
    
    public virtual void InitiateQuest()
    {
        if (!isInitiated)
        {
            isInitiated = true;
            isCompleted = false;
            OnQuestInitiated?.Invoke(this, EventArgs.Empty);
        }
    }
    public virtual void CompleteQuest()
    {
        isCompleted = true;
        isCompleted = true;
        OnQuestCompleted?.Invoke(this, EventArgs.Empty);
    }
}
