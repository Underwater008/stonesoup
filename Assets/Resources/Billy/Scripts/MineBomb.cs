using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MineBomb : ABX_Tile
{
    public TextMeshPro tmp;
    public GameObject bombPrefab;
    void OnTriggerEnter2D(Collider2D collision)
    {
        tmp.text = "For Emergencies";
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        tmp.text = "";
    }
    public override void useAsItem(Tile tileUsingUs)
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
        takeDamage(this, 9999);
    }


}
