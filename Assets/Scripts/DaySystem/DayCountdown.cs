using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DayCountdown : MonoBehaviour
{
    [SerializeField] private int countdownDays = 3;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject countdownPanel;
    public void ShowCountdown()
    {
        int daysLeft = countdownDays - GameData.Instance.DayManagerDayCount;
        countdownPanel.SetActive(true);
        countdownText.text = daysLeft.ToString() + " days remaining";
    }
}
