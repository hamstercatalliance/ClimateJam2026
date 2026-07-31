using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCenterCycling : MonoBehaviour
{
    int dayCount = 0;
    [SerializeField]
    GameObject[] DataCenterModels;

    // Start is called before the first frame update
    void Start()
    {
        DataCenterModels[0].SetActive(false);
        DataCenterModels[1].SetActive(false);
        DataCenterModels[2].SetActive(false);
        DataCenterModels[3].SetActive(false);
        dayCount = DayManager.Instance.dayCount;
        DataCenterModels[dayCount].SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if(DayManager.Instance.dayCount != dayCount)
        {
           DataCenterModels[dayCount].SetActive(false);
           dayCount = DayManager.Instance.dayCount;
           DataCenterModels[dayCount].SetActive(true);
        }
    }
}
