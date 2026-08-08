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
        foreach (GameObject model in DataCenterModels)
        {
            model.SetActive(false);
        }
        dayCount = DayManager.Instance.dayCount;
        DataCenterModels[dayCount].SetActive(true);
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     if(DayManager.Instance.dayCount != dayCount)
    //     {
    //        DataCenterModels[dayCount].SetActive(false);
    //        dayCount = DayManager.Instance.dayCount;
    //        DataCenterModels[dayCount].SetActive(true);
    //     }
    // }
}
