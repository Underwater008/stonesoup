using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class ABX_xiao23BackPack : Tile
{
    public GameObject InventoryUI;
    //public GameObject Parent;


    public override void pickUp(Tile tilePickingUsUp)
    {

        //Debug.Log("Picked up BackPack");
        Instantiate(InventoryUI, tilePickingUsUp.gameObject.transform);
        die();

        //base.pickUp(tilePickingUsUp);

        //tilePickingUsUp.restoreAllHealth();


    }
    // Start is called before the first frame update
    void Start()
    {
        //InventoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
