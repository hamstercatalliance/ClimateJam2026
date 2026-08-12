using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FlowerPlanting : Quest
{
    [SerializeField] private GameItemSO flowerSeeds;
    [SerializeField] private GameObject flowerPlantingArea;
    [SerializeField] private GameObject flowers;
    [SerializeField] private GameItem flowerSeedsObject;
    private bool pickedUpSeeds;

    private const string DOWNTOWN_SCENE = "Downtown";


    public override void WriteToGameData()
    {
        GameData.Instance.FlowerSeedsPickedUp = pickedUpSeeds;
        base.WriteToGameData();
    }

    public override void LoadGameData()
    {
        base.LoadGameData();
        if (GameData.Instance != null)
        {
            // setCondition(GameData.Instance.OpenedQuestBoard);
            //HI RANJIt
            //becasreful to ot call complete quest on laod 
            //because it will trigger the quest complete event and play the completed sound on scene laod
            pickedUpSeeds = GameData.Instance.FlowerSeedsPickedUp;
        }
        Debug.Log("isCompleted " + isCompleted + "for flower planting");

    }
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
        StartCoroutine(LateStart());
     
    }

    private IEnumerator LateStart ()
    {
        yield return null;
        if (gameObject.scene.name == DOWNTOWN_SCENE)
        {
            if (!isInitiated)
            {
                flowerSeedsObject.gameObject.SetActive(false);
            }
            if (isInitiated && !isCompleted)
            {
                flowerPlantingArea.SetActive(true);
                flowers.SetActive(false);
                if (!pickedUpSeeds)
                {
                    flowerSeedsObject.gameObject.SetActive(true);
                    InventoryManager.Instance.OnInventoryAddition += InventoryManager_OnInventoryAddition;
                }
            }

            if (isCompleted)
            {
                flowerPlantingArea.SetActive(true);
                flowers.SetActive(true);
                flowerSeedsObject.gameObject.SetActive(false);
                flowerPlantingArea.GetComponent<FlowerPlantingInteractable>().DisableInteraction();
            }
        }
    }

    public override void InitiateQuest()
    {
        base.InitiateQuest();
        flowerSeedsObject.gameObject.SetActive(true);
    }

    public void InventoryManager_OnInventoryAddition (object sender, EventArgs e)
    {
        InventoryManager.InventoryAdditionEventArgs args = e as InventoryManager.InventoryAdditionEventArgs;
        if (args.slot.itemID == flowerSeeds.itemID && !pickedUpSeeds)
        {
            pickedUpSeeds = true;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        InventoryManager.Instance.OnInventoryAddition -= InventoryManager_OnInventoryAddition;


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
