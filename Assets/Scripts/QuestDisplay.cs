using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuestDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI questDesc;
    private bool stayVisible = false;
    // Start is called before the first frame update
    void Start()
    {
        QuestSlotUI.OnQuestSlotHovered += OnQuestSlotHovered;
        QuestSlotUI.OnQuestSlotHoverExit += OnQuestSlotHoverExit;
        QuestSlotUI.OnQuestSlotClicked += OnQuestSlotClicked;
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
        if (stayVisible == false)
        {
            stayVisible = true;
        }
        else 
        {
            stayVisible = false;
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
    }
}
