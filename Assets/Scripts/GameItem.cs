using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameItem : MonoBehaviour
{
    [SerializeField] private GameItemSO gameItemSO;
    public GameItemSO GetGameItemSO()
    {
        return gameItemSO;
    }
    
}
