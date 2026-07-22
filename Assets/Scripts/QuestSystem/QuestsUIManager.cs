using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestsUIManager : MonoBehaviour
{
    public static QuestsUIManager Instance { get; private set; }
    [SerializeField] private GameObject questUITemplate;
    [SerializeField] private Sprite activeQuestIcon;
    [SerializeField] private Sprite finishedQuestIcon;
    [SerializeField] private Transform contentParent;
    [SerializeField] private Sprite[] questSlotBkgs;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void CreateQuestSlot(QuestManager.QuestData questData)
    {
        GameObject questSlot = Instantiate(questUITemplate, contentParent);
        questSlot.SetActive(true);
        questSlot.GetComponent<Image>().sprite = questSlotBkgs[Random.Range(0, questSlotBkgs.Length)];
        QuestSlotUI questSlotUI = questSlot.GetComponent<QuestSlotUI>();
        questSlotUI.SetQuestName(questData.questSO.questName);
        if (questData.isCompleted)
        {
            questSlotUI.SetIcon(finishedQuestIcon);
            questSlotUI.SetCompleted(true);
        }
        else
        {
            questSlotUI.SetIcon(activeQuestIcon);
        }
        questSlotUI.SetQuestSO(questData.questSO);
    }
    public void LoadQuestUIData()
    {
        List<QuestManager.QuestData> questDataList = QuestManager.Instance.questDataList;
        foreach (QuestManager.QuestData questData in questDataList)
        {
            if (questData.isInitiated)
            {
                CreateQuestSlot(questData);
            }
        }
    }
    public void UpdateQuestUI(QuestManager.QuestData questData)
    {
        Debug.Log("Updating quest UI for quest: " + questData.questSO.questName);
        QuestSlotUI[] questSlotUIs = contentParent.GetComponentsInChildren<QuestSlotUI>();
        QuestSO questSO = questData.questSO;
        foreach (QuestSlotUI questSlotUI in questSlotUIs)
        {
            if (questSlotUI.GetQuestSO() == questSO)
            {
                if (questData.isCompleted)
                {
                    questSlotUI.SetIcon(finishedQuestIcon);
                    questSlotUI.SetCompleted(true);
                }
                else
                {
                    questSlotUI.SetIcon(activeQuestIcon);
                }
                Debug.Log("Update quest called for quest: " + questData.questSO.questName);
                break;
            }
        }
    }
}
