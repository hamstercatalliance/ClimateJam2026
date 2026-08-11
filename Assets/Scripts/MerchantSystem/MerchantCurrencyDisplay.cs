using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MerchantCurrencyDisplay : MonoBehaviour
{
    private TextMeshProUGUI currencyText;
    private void Start()
    {
        currencyText = GetComponent<TextMeshProUGUI>();
        CurrencyManager.Instance.OnCurrencyChanged += OnCurrencyChangedHandler;
        UpdateCurrencyDisplay(CurrencyManager.Instance.GetCurrency());
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
        currencyText.text = "Coins:" + newCurrencyAmount.ToString();
    }
}
