using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Door : ABX_Tile
{


    Transform canvas;

    public Tile keyTile;

    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
    }



    // void OnCollisionStay2D(Collision2D collision)
    // {
    //     Debug.Log(collision);
    //     if (collision.gameObject.GetComponent<Tile>().tileName.Equals(keyTile.tileName) && IsInHand(collision.gameObject.transform))
    //     {
    //         if (Input.GetKeyDown(KeyCode.Mouse0))
    //         {
    //             collision.gameObject.GetComponent<Tile>().useAsItem(this);
    //             collision.gameObject.GetComponent<ABX_Tile>().ABX_die();
    //             die();
    //         }
    //     }
    // }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Tile>() == null)
            return;
        if (collision.GetComponent<Tile>().tileName.Equals(keyTile.tileName) && IsInHand(collision.transform))
        {
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
