using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuestDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI questDesc;
    private QuestSlotUI currentlyPinnedSlot;
    public QuestSlotUI GetCurrentlyPinnedSlot()
    {
        return currentlyPinnedSlot;
    }
    private bool stayVisible = false;
    public static QuestDisplay Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        QuestSlotUI.OnQuestSlotHovered += OnQuestSlotHovered;
        QuestSlotUI.OnQuestSlotHoverExit += OnQuestSlotHoverExit;
        QuestSlotUI.OnQuestSlotClicked += OnQuestSlotClicked;
        MenuManager.Instance.OnMenuClosed += OnMenuClosedHandler;
        HideDisplay();
    }
    private void OnDestroy()
    {
        QuestSlotUI.OnQuestSlotHovered -= OnQuestSlotHovered;
        QuestSlotUI.OnQuestSlotHoverExit -= OnQuestSlotHoverExit;
        QuestSlotUI.OnQuestSlotClicked -= OnQuestSlotClicked;
        MenuManager.Instance.OnMenuClosed -= OnMenuClosedHandler;
    }
    private void OnMenuClosedHandler(object sender, System.EventArgs e)
    {
        stayVisible = false;
        currentlyPinnedSlot = null;
        HideDisplay();
    }
    private void OnQuestSlotHovered(object sender, QuestSlotUI.OnSlotHoveredEventArgs e)
    {
        ShowDisplay(e.questSO, e.isCompleted);
    }
    private void OnQuestSlotHoverExit(object sender, System.EventArgs e)
    {
        if (stayVisible == false)
        {
            HideDisplay();
        }
    }
    private void OnQuestSlotClicked(object sender, System.EventArgs e)
    {
        QuestSlotUI clickedSlot = sender as QuestSlotUI;

        if (currentlyPinnedSlot == clickedSlot)
        {
            //unpin pinned slot
            currentlyPinnedSlot = null;
            stayVisible = false;
        }
        else
        {
            //pin new slot
            currentlyPinnedSlot = clickedSlot;
            stayVisible = true;
        }
    }
    private void HideDisplay()
    {
        statusText.text = "";
        questDesc.text = "";
    }
    private void ShowDisplay(QuestSO questSO, bool isCompleted)
    {
        if (isCompleted)
        {
            statusText.text = "Status:\nCOMPLETED";
        }
        else
        {
            statusText.text = "Status:\nIN\nPROGRESS";
        }
        questDesc.text = questSO.questDescription;
        if (questSO.currencyAward > 0)
        {
            questDesc.text += "\n\nReward:\n" + questSO.currencyAward + " Coins";
        }
        
    }
}
