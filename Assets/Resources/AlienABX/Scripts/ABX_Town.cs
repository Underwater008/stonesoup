using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ABX_Town : Tile
{
    int _wood;
    int _clay;
    int _gold;

    int _level;
    int _startLevel = 0;
    int _startGold = 0;

    [SerializeField] TextMeshPro _reqTMP;
    [SerializeField] TextMeshPro _levelTMP;
    List<string> _townRequirements = new List<string>();

    public override void init()
    {
        _townRequirements.Add("Req: 1 wood.");
        _townRequirements.Add("Req: 2 clay.");
        _townRequirements.Add("Req: nothing.");
        _sprite = GetComponentInChildren<SpriteRenderer>();
        updateSpriteSorting();
        _anim = GetComponentInChildren<Animator>();
        if (hasTag(TileTags.Creature) && GetComponent<Rigidbody2D>() == null)
        {
            _body = gameObject.AddComponent<Rigidbody2D>();
        }
        else
        {
            _body = GetComponent<Rigidbody2D>();
        }
        _collider = GetComponent<Collider2D>();
        _startHealth = health;
        _gold = _startGold;
        _level = _startLevel;
        _levelTMP.text = _level.ToString();
        _reqTMP.text = _townRequirements[_level];
    }


    // public override void interact(Tile tileInteracting)
    // {
    //     //unsure like... how one would implement the merchent stuff other than this.
    //     //goal is to have player use resources and put them into this, and then it levels up.


    //     //has nothing to trade

    //     if (tileInteracting.tileWereHolding.Equals(null))
    //         return;
    //     if (!tileInteracting.tileWereHolding.hasTag(TileTags.Money))
    //         return;

    //     Tile t = tileInteracting.tileWereHolding;
    //     t.dropped(tileInteracting);
    //     if (t.tileName.Equals("Clay"))
    //     {
    //         _clay++;
    //         Clay c = (Clay)t;
    //         c.delete();
    //     }
    //     if (t.tileName.Equals("Wood"))
    //     {
    //         _wood++;
    //         Wood w = (Wood)t;
    //         w.delete();
    //     }



    //     if (CheckLevels())
    //         _level++;

    //     _levelTMP.text = _level.ToString();
    //     _reqTMP.text = _townRequirements[_level];
    // }

    bool CheckLevels()
    {
        if (level1Req())
            return true;
        if (level2Req())
            return true;
        return false;
    }
    //Can have a function for each level.
    bool level1Req()
    {
        if (_level >= 1)
            return false;
        if (_wood < 1)
            return false;
        return true;
    }
    bool level2Req()
    {
        if (_level >= 2)
            return false;
        if (_clay < 2)
            return false;
        return true;
    }

    public override void takeDamage(Tile tileDamagingUs, int damageAmount, DamageType damageType)
    {
        tileDamagingUs.takeDamage(this, 3);
    }

    public override void tileDetected(Tile detectedTile)
    {
        if (!detectedTile.hasTag(TileTags.Player))
            return;
        Player p = (Player)detectedTile;
        p.handSymbol.SetActive(true);

    }




}
