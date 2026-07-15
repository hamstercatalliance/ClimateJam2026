using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMerchant : MonoBehaviour
{
    [SerializeField] private GameObject testStore;
    private void Start()
    {
        testStore.GetComponent<MerchantStore>().LeaveStore();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            testStore.GetComponent<MerchantStore>().EnterStore();
        }
    }
}
