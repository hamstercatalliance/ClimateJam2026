using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class QuestSlotUI : SlotUI
{
    [SerializeField] private float hoverScale = 1.1f;
    private Vector3 textOriginalScale;
    private Vector3 iconOriginalScale;
    private void Start()
    {
        textOriginalScale = questNameObject.transform.localScale;
        iconOriginalScale = questStatusIcon.transform.localScale;
    }
    [SerializeField] private GameObject questStatusIcon;
    [SerializeField] private GameObject questNameObject;
    [SerializeField] private GameObject questDisplayObject;
    private QuestSO questSO;
    public static event EventHandler<OnSlotHoveredEventArgs> OnQuestSlotHovered;
    public class OnSlotHoveredEventArgs : EventArgs
    {
        public QuestSO questSO;
        public bool isCompleted;
    }
    public static event EventHandler OnQuestSlotHoverExit;
    public static event EventHandler OnQuestSlotClicked;
    private bool isCompleted = false;
    public void SetCompleted(bool completed)
    {
        isCompleted = completed;
    }
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
        questNameObject.GetComponent<TextMeshProUGUI>().text = name;
    }
    public QuestSO GetQuestSO()
    {
        return questSO;
    }
    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        OnQuestSlotClicked?.Invoke(this, EventArgs.Empty);
    }
    public override void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (questSO != null)
        {
            questNameObject.transform.localScale = textOriginalScale * hoverScale;
            questStatusIcon.transform.localScale = iconOriginalScale * hoverScale;
            OnQuestSlotHovered?.Invoke(this, new OnSlotHoveredEventArgs
            {
                questSO = questSO,
                isCompleted = isCompleted
            });
        }
    }
    public override void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        questNameObject.transform.localScale = textOriginalScale;
        questStatusIcon.transform.localScale = iconOriginalScale;
        OnQuestSlotHoverExit?.Invoke(this, EventArgs.Empty);
    }
}
