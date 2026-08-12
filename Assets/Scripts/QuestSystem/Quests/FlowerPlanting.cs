using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FlowerPlanting : Quest
{
    [SerializeField] private GameItemSO flowerSeeds;
    [SerializeField] private GameObject flowerPlantingArea;
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
            if (isInitiated && !isCompleted)
            {
                flowerPlantingArea.SetActive(true);
                flowers.SetActive(false);
                return;
            }

            if (isCompleted)
            {
                flowerPlantingArea.SetActive(true);
                flowers.SetActive(true);

                flowerPlantingArea.GetComponent<FlowerPlantingInteractable>().DisableInteraction();
            }
        }
    }

    public void PlantFlowers()
    {
        InventoryManager.Instance.RemoveItemFromInventory(flowerSeeds, 1);
        flowerPlantingArea.SetActive(true);
        flowers.SetActive(true);

        flowerPlantingArea.GetComponent<FlowerPlantingInteractable>().DisableInteraction();

        CompleteQuest();
    }
}
