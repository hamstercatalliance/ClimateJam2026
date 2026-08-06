using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BoardEntryManager : MonoBehaviour
{
    [Header("questInstance should not come from a prefab, its a component in a unique instance under quests in SceneBasics")]
    [SerializeField] private Quest questInstance; 
    [Header("Day is the day the quest will be available on the board")]
    [SerializeField] private int day; 
    public Quest GetQuest { get {  return questInstance; } }
    public int GetDay { get { return day; } }
    private void OnEnable()
    {
        if (questInstance == null)
        {
            Debug.LogWarning("questInstance is not assigned in the editor.");
            return;
        }
        TMP_Text nameText = transform.Find("Title").GetComponent<TMP_Text>();
        nameText.text = questInstance.questSO.questName;
        Button takeButton = transform.Find("Accept").GetComponent<Button>();

        takeButton.onClick.AddListener(() => {
            questInstance.InitiateQuest();
            BoardManager.Instance.RenderBoard();
        });
    }
}