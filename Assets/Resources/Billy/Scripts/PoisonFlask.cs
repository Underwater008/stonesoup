using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFlask : Tile
{
    public GameObject poisonPoolPrefab;
    //How fast should the poison flask move
    public float speed;


    //If the poison flask has been thrown or not
    bool _isThrown = false;
    //Where the posion flask is thrown from
    Vector3 _originPos;
    //Where the poison flask should land
    Vector3 _landingPos;

    //Used for lerping
    float _t = 0;


    public override void useAsItem(Tile tileUsingUs)
    {
        base.useAsItem(tileUsingUs);
        _originPos = transform.position;
        _landingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _isThrown = true;
    }

    void Update()
    {
        if (!_isThrown)
        {
            return;
        }
        //Move the posion flask toward the landing position
        transform.position = new Vector3(Mathf.Lerp(_originPos.x, _landingPos.x, _t),
                                         Mathf.Lerp(_originPos.y, _landingPos.y, _t),
                                         _originPos.z);
        _t += Time.deltaTime * speed;
        if (_t >= 1.0f)
        {
            CreatePoisonPool(transform.position);
        }
    }

    void CreatePoisonPool (Vector3 pos)
    {
        Instantiate(poisonPoolPrefab, pos, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //The poison flask will explode when it hits a wall
        Tile tile = collision.GetComponent<Tile>();
        if (tile != null)
        {
            if (tile.hasTag(TileTags.Wall))
            {
                if (_isThrown && _t > 0.1f)
                {
                    CreatePoisonPool(transform.position);
                }
            }
        }
    }
}
