using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ABX_ResearchStation : ABX_Tile
{
    [Header("Research")]
    public ABX_Tile requiredObject;
    public int requiredAmount;
    public GameObject dropItemPrefab;
    public int dropAmount;
    public Vector3 dropOffset;
    public bool isOneTime;

    [Header("UI")]
    public TMP_Text textUI;

    int _current = 0;
    bool _finished = false;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (_finished)
            return;
        ABX_Tile tile = collision.gameObject.GetComponent<ABX_Tile>();
        if (tile == null)
        {
            return;
        }

        if (tile.tileName.Equals(requiredObject.tileName) && !tile.isBeingHeld)
        {
            _current++;
            Destroy(tile.gameObject);
            if (_current >= requiredAmount)
            {
                for (int i = 0; i < dropAmount;)
                {
                    Instantiate(dropItemPrefab, transform.position + dropOffset * ++i, Quaternion.identity);
                }
                if (isOneTime)
                    _finished = true;
                _current = 0;
            }
            UpdateUI();
        }
    }

    public override void takeDamage(Tile tileDamagingUs, int damageAmount, DamageType damageType)
    {
        if (damageType == DamageType.Normal)
            return;
        base.takeDamage(tileDamagingUs, damageAmount, damageType);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile == null)
        {
            return;
        }
        if (tile.hasTag(TileTags.Player))
        {
            UpdateUI();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile == null)
        {
            return;
        }
        if (tile.hasTag(TileTags.Player))
        {
            textUI.text = "";
        }
    }

    void UpdateUI ()
    {
        if (_finished)
        {
            textUI.text = "Research Complete";
            return;
        }
        textUI.text = "" + requiredAmount + " " + requiredObject.tileName + " ---> " + " " +
                          dropAmount + " " + dropItemPrefab.GetComponent<ABX_Tile>().tileName + "\n" +
                          "Need " + (requiredAmount - _current) + " more";
    }
}
