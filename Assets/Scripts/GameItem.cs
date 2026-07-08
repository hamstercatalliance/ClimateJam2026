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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // public void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         Debug.Log("I AM AN ITEM AND I HAVE COLLIDED WITH THE PLAYER");
    //     }
    // }
}
