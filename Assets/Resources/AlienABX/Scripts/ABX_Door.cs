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




    private void OnTriggerStay2D(Collider2D collision)
    {
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Tile>().tileName.Equals(keyTile.tileName) && IsInHand(collision.transform))
        {


            var prefab = Resources.Load<GameObject>("Xiao/UI/PickUpE");
        }
    }



    bool IsInHand(Transform key)
    {
        return key.GetComponent<Tile>().isBeingHeld;
    }
}
