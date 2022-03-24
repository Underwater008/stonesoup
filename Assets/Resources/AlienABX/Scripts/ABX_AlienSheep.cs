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
    public float moveIntervalMax;

    [Header("Drops")]
    public GameObject commonDrop;
    public GameObject rareDrop;
    public float commonChance;
    public float rareChance;
    public int minDrop;
    public int maxDrop;
    public float offsetRange;

    private int MehhhDelay;
    private bool Mehhhed = false;
    public AudioSource Sheep;
    bool playerNearSheep = false;

    private void FixedUpdate()
    {
        Vector2 targetGlobalPos = Tile.toWorldCoord(_targetGridPos.x, _targetGridPos.y);
        if (Vector2.Distance(transform.position, targetGlobalPos) >= 0.1f)
        {
            // If we're away from our target position, move towards it.
            Vector2 toTargetPos = (targetGlobalPos - (Vector2)transform.position).normalized;
            moveViaVelocity(toTargetPos, moveSpeed, moveAcceleration);
            // Figure out which direction we're going to face. 
            // Prioritize side and down.
            if (_anim != null)
            {
                if (toTargetPos.x >= 0)
                {
                    _sprite.flipX = false;
                }
                else
                {
                    _sprite.flipX = true;
                }
                // Make sure we're marked as walking.
                _anim.SetBool("Walking", true);
                if (Mathf.Abs(toTargetPos.x) > 0 && Mathf.Abs(toTargetPos.x) > Mathf.Abs(toTargetPos.y))
                {
                    _anim.SetInteger("Direction", 1);
                }
                else if (toTargetPos.y > 0 && toTargetPos.y > Mathf.Abs(toTargetPos.x))
                {
                    _anim.SetInteger("Direction", 0);
                }
                else if (toTargetPos.y < 0 && Mathf.Abs(toTargetPos.y) > Mathf.Abs(toTargetPos.x))
                {
                    _anim.SetInteger("Direction", 2);
                }
            }
        }
        else
        {
            moveViaVelocity(Vector2.zero, 0, moveAcceleration);
            if (_anim != null)
            {
                _anim.SetBool("Walking", false);
            }
        }

        if (playerNearSheep)
        {
            StartCoroutine(Mehhh());
        }
        // if (Mehhhed == false)
        // {
        //     StartCoroutine(Mehhh());
        //     Sheep.Play();
        //     print(Mehhhed);
        // }


    }

    IEnumerator Mehhh()
    {
        if (deathSFX == null)
            yield break;

        AudioManager.playAudio(deathSFX);
        yield return new WaitForSeconds(deathSFX.length);

        MehhhDelay = Random.Range(1, 3);
        yield return new WaitForSeconds(MehhhDelay);

        yield break;
        // Mehhhed = true;
    }



    public override void useAsItem(Tile tileUsingUs)
    {
        Debug.Log("Harvested");
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
            playerNearSheep = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile == null)
            return;
        if (tile.hasTag(TileTags.Player))
        {
            playerNearSheep = false;
        }
    }

    public override void init()
    {
        StartCoroutine(Move());
        base.init();
    }

    IEnumerator Move()
    {
        Debug.Log("Moving");
        yield return new WaitForSeconds(Random.Range(moveIntervalMin, moveIntervalMax));
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
