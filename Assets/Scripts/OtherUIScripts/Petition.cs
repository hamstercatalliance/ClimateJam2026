using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petition : MonoBehaviour, IHasPersistentData
{
    [SerializeField] private GameObject petitionPost;
    [SerializeField] private GameObject petition;
    [SerializeField] private GameObject signature;
    [SerializeField] private GameObject signButton;
    private bool petitionSigned = false;
    public bool DataSuccessfullyWritten { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if (GameData.Instance.DayManagerDayCount == 2 && GameData.Instance.SympathyPoints > 600)
        {
            petitionPost.SetActive(true);
        }
        else
        {
            petitionPost.SetActive(false);
        }
    }
    public void OpenPetition()
    {
        petition.SetActive(true);
    }
    public void ClosePetition()
    {
        petition.SetActive(false);
    }

    public void SignPetition()
    {
        if (!petitionSigned)
        {
            signature.SetActive(true);
            petitionSigned = true;
            GameData.Instance.PetitionSigned = true;
            signButton.SetActive(false);
            SympathyPointsManager.Instance.addSympathyPoints(100);
        }
    }
    public void LoadGameData()
    {
        petitionSigned = GameData.Instance.PetitionSigned;
        if (petitionSigned)
        {
            signature.SetActive(true);
            signButton.SetActive(false);
        }
    }
    public void WriteToGameData()
    {
        GameData.Instance.PetitionSigned = petitionSigned;
        DataSuccessfullyWritten = true;
    }
}
