using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CurrencyDisplayManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    private void Start()
    {
        CurrencyManager.Instance.OnCurrencyChanged += OnCurrencyChangedHandler;
    }
    private void OnDestroy()
    {
        CurrencyManager.Instance.OnCurrencyChanged -= OnCurrencyChangedHandler;
    }
    private void OnCurrencyChangedHandler(object sender, CurrencyManager.OnCurrencyChangedEventArgs e)
    {
        UpdateCurrencyDisplay(e.newCurrencyAmount);
    }
    private void UpdateCurrencyDisplay(int newCurrencyAmount)
    {
        Debug.Log("Updating currency display to: " + newCurrencyAmount);
        currencyText.text = "Coins:" + newCurrencyAmount.ToString();
    }
}
