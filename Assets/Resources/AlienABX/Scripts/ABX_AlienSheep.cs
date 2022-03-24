using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_AlienSheep : ABX_BasicAICreature
{
    const float BRIGHTNESS = 150f;

    [Header("Alien Sheep")]
    public int damage;

    [Header("Movement")]
    public float moveIntervalMin;
    public float moveINtervalMax;

    [Header("Drops")]
    public GameObject commonDrop;
    public GameObject rareDrop;
    public float commonChance;
    public float rareChance;
    public int minDrop;
    public int maxDrop;
    public float offsetRange;

    public override void useAsItem(Tile tileUsingUs)
    {
        removeTag(TileTags.Water);
        GetComponent<SpriteRenderer>().color = new Color(BRIGHTNESS, BRIGHTNESS, BRIGHTNESS);
        for (int i = 0; i < Random.Range(minDrop, maxDrop); i++)
        {
            Vector3 offsetVector = new Vector3(Random.Range(-offsetRange, offsetRange),
                                               Random.Range(-offsetRange, offsetRange),
                                               0);
            if (Random.Range(0f, 1f) < rareChance)
            {
                Instantiate(rareDrop, transform.position + offsetVector, Quaternion.identity);
            }
            else if (Random.Range(0f, 1f) < commonChance)
            {
                Instantiate(commonDrop, transform.position + offsetVector, Quaternion.identity);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile == null)
            return;
        if (tile.hasTag(TileTags.Player))
        {
            tile.takeDamage(this, damage, DamageType.Explosive);
        }
    }

    public override void init()
    {
        base.init();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(Random.Range(moveIntervalMin, moveINtervalMax));
        takeStep();
        StartCoroutine(Move());
    }

    protected override void takeStep()
    {
        _neighborPositions.Clear();

        Vector2 upGridNeighbor = new Vector2(_targetGridPos.x, _targetGridPos.y + 1);
        if (pathIsClear(toWorldCoord(upGridNeighbor)))
        {
            _neighborPositions.Add(upGridNeighbor);
        }
        Vector2 rightGridNeighbor = new Vector2(_targetGridPos.x + 1, _targetGridPos.y);
        if (pathIsClear(toWorldCoord(rightGridNeighbor)))
        {
            _neighborPositions.Add(rightGridNeighbor);
        }
        Vector2 downGridNeighbor = new Vector2(_targetGridPos.x, _targetGridPos.y - 1);
        if (pathIsClear(toWorldCoord(downGridNeighbor)))
        {
            _neighborPositions.Add(downGridNeighbor);
        }
        Vector2 leftGridNeighbor = new Vector2(_targetGridPos.x - 1, _targetGridPos.y);
        if (pathIsClear(toWorldCoord(leftGridNeighbor)))
        {
            _neighborPositions.Add(leftGridNeighbor);
        }

        if (_neighborPositions.Count > 0)
        {
            _targetGridPos = GlobalFuncs.randElem(_neighborPositions);
        }
    }
}
