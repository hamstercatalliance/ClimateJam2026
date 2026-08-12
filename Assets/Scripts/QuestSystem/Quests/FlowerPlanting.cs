using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FlowerPlanting : Quest
{
    [SerializeField] private GameItemSO flowerSeeds;
    [SerializeField] private GameObject flowers;

    private const string DOWNTOWN_SCENE = "Downtown";

    public bool CheckFlowerSeeds()
    {
        int items = InventoryManager.Instance.GetItemCount(flowerSeeds);
        if (1 <= items)
        {
            Debug.Log("PlantingFlowers: Flower seeds count sufficient");
            return true;
        }

        Debug.Log("PlantingFlowers: Flower seeds count insufficient");
        return false;
    }

    protected override void Start()
    {
        base.Start();
        if (gameObject.scene.name == DOWNTOWN_SCENE)
        {
            if (isCompleted)
            {
                flowers.SetActive(true);
            }
            else
            {
                flowers.SetActive(false);
            }
        }
    }
}
