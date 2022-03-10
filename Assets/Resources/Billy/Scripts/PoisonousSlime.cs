using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousSlime : BasicAICreature
{
	public GameObject slimePrefab;
	public GameObject poisonPoolPrefab;
	[Header("Movement")]
	public float moveIntervalMin;
	public float moveINtervalMax;

	[Header("Multiply")]
	public float multiplyeIntervalMin;
	public float multiplyINtervalMax;
	public float scaleMultiplier;

	[Header("Damage")]
	public int damage;

	public override void init()
    {
        base.init();
		StartCoroutine(Move());
		if (health > 1)
			StartCoroutine(Multiply());
	}

	IEnumerator Move ()
    {
		yield return new WaitForSeconds(Random.Range(moveIntervalMin, moveINtervalMax));
		takeStep();
		StartCoroutine(Move());
    }

	IEnumerator Multiply ()
    {
		yield return new WaitForSeconds(Random.Range(multiplyeIntervalMin, multiplyINtervalMax));
		SpawnNewSlime();
		StartCoroutine(Multiply());
	}

    //This function is modified from AP's basic enemy script
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

	void SpawnNewSlime ()
    {
		GameObject slime = Instantiate(slimePrefab, transform.position, Quaternion.identity);
		slime.transform.localScale = transform.localScale * scaleMultiplier;
		slime.GetComponent<PoisonousSlime>().health = health / 2;
		slime.GetComponent<PoisonousSlime>().init();
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
		Tile tile = collision.gameObject.GetComponent<Tile>();
		if (tile != null)
        {
			if (tile.hasTag(TileTags.Friendly))
            {
				tile.takeDamage(this, damage);
            }
        }
    }

    protected override void die()
    {
		Instantiate(poisonPoolPrefab, transform.position, Quaternion.identity);
		base.die();
	}

    void OnEnable()
    {
		init();
    }
}
