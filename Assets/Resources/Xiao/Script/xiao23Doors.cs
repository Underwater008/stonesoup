using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xiao23Doors : Tile
{
    //public override void pickUp(Tile tilePickingUsUp) {

    //Debug.Log("Picked up BronzeKey");

    //base.pickUp(tilePickingUsUp);

    //tilePickingUsUp.restoreAllHealth();
    //die();

    //}
    // Start is called before the first frame update


    Transform canvas;

    GameObject EPressed;
    Tile keyTile;
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
    }

    // Update is called once per frame
    void Update()
    {
       if(keyTile != null &&!keyTile.GetComponent<Tile>().isBeingHeld)
        {
            if (EPressed != null)
                Destroy(EPressed);
        }
            
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
         if (collision.name.Contains( "BronzeKey") && IsInHand(collision.transform))
        {
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(gameObject);
                collision.GetComponent<Tile>().useAsItem(this);
            }
        }
    }
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("BronzeKey") && IsInHand(collision.transform))
        {
            keyTile = collision.GetComponent<Tile>();
            var prefab = Resources.Load<GameObject>("Xiao/UI/PickUpE");
            EPressed= Instantiate(prefab, canvas);
            EPressed.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Contains("BronzeKey"))
        {
            if (EPressed != null)
                Destroy(EPressed);
        }
    }
    public void OnDisable()
    {
        if (EPressed != null)
            Destroy(EPressed);
    }

    bool IsInHand(Transform key)
    {
        return key.GetComponent<Tile>().isBeingHeld;
    }

}
