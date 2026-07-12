using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestSlotUI : SlotUI
{
    [SerializeField] private GameObject questStatusIcon;
    [SerializeField] private TextMeshProUGUI questNameText;
    private QuestSO questSO;
    public void SetQuestSO(QuestSO questSO)
    {
        this.questSO = questSO;
    }
    public void SetIcon(Sprite sprite)
    {
        questStatusIcon.GetComponent<Image>().sprite = sprite;
    }
    public void SetQuestName(string name)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = name;
    }
    public QuestSO GetQuestSO()
    {
        return questSO;
    }
    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        
    }
    public override void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        
    }
    public override void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        
    }
}
