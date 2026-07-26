using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardEntryManager : MonoBehaviour
{
    [SerializeField] private Quest quest;
    [SerializeField] private int day; 
    public Quest getQuest { get {  return quest; } }
    public int getDay { get { return day; } }

    private void OnEnable()
    {
        TMP_Text nameText = transform.Find("Title").GetComponent<TMP_Text>();
        nameText.text = quest.name;
        Button takeButton = transform.Find("Accept").GetComponent<Button>();

        takeButton.onClick.AddListener(() => {
            quest.InitiateQuest();
            BoardManager.Instance.RenderBoard();
        });
    }
}