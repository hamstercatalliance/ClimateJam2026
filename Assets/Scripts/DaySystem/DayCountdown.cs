using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DayCountdown : MonoBehaviour
{
    [SerializeField] private int countdownDays = 3;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject countdownPanel;
    private float countdownDuration = 2f;
    public float GetCountdownDays()
    {
        return countdownDays;
    }
    private void Start()
    {
        // if (countdownPanel != null)
        // {
        //     countdownPanel.SetActive(false);
        // }
    }
    public IEnumerator ShowCountdownCoroutine()
    {
        int daysLeft = countdownDays - GameData.Instance.DayManagerDayCount;
        Debug.Log($"[DayCountdown] DayManagerDayCount={GameData.Instance.DayManagerDayCount}, daysLeft={daysLeft}");
        countdownPanel.SetActive(true);
        Debug.Log(countdownPanel.activeInHierarchy);
        if (daysLeft == 1)
        {
            countdownText.text = $"{daysLeft} day remaining";
        }
        else if (daysLeft == 0)
        {
            countdownText.text = "Today is The Day";
        }
        else if (daysLeft < countdownDays)
        {
            countdownText.text = $"{daysLeft} days remaining";
        }
        
        yield return new WaitForSeconds(countdownDuration);
        countdownPanel.SetActive(false);
    }
}
