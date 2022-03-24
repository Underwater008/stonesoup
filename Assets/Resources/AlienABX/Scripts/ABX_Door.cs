using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Door : ABX_Tile
{


    Transform canvas;

    public Tile keyTile;
    public GameObject pickUpEUI;
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
    }




    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Tile>().tileName.Equals(keyTile.tileName) && IsInHand(collision.transform))
        {
            var prefab = pickUpEUI;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                collision.GetComponent<Tile>().useAsItem(this);
                collision.GetComponent<ABX_Tile>().ABX_die();
                die();
            }
        }
    }



    bool IsInHand(Transform key)
    {
        return key.GetComponent<Tile>().isBeingHeld;
    }
}
