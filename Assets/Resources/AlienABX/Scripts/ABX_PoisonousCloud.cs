using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_PoisonousCloud : ABX_Tile
{
    [Header("Posionous Cloud")]
    public int damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile == null)
            return;

    }
}
