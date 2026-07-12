using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject hasProgressGameObject;
    private IHasProgress hasProgress;
    
    public void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("hasProgress is null!");
        }
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        barImage.fillAmount = 0f;

        //Hide();
    }
    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        Debug.Log("Progress: " + e.progressNormalized);
        // if (e.progressNormalized == 1f || e.progressNormalized == 0f)
        // {
        //     Hide();
        // }
        // else
        // {
        //     Show();
        // }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
