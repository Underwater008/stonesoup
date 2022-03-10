using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xiao23EnemySkull : BasicAICreature
{
    public float timeBetweenMove = 1;
    float _moveTimer;
    public GameObject projectilePrefab;
    Vector2 _initPosition;
    Vector2 _randomPosition;
    bool _targetIsInitPositon;

    Transform _shootTarget;
    float _shootTimer;

    public override void init()
    {
        base.init();
        _initPosition =toGridCoord(globalX,globalY);
      FindRandomPosition();

    }
    public void Update()
    {

        Movement();
        Shoot();
    }
    void Movement()
    {
        _moveTimer += Time.deltaTime;
        if (_moveTimer > timeBetweenMove)
        {
            _moveTimer = 0;
            takeStep();
        }
    }
    void Shoot()
    {
        if (_shootTarget == null)
            return;
        _shootTimer += Time.deltaTime;
        if(_shootTimer >1)
        {
            var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector2 dir = _shootTarget.position - transform.position;

            dir.Normalize();
            projectile.transform.right = dir;
            _shootTimer = 0;
        }
    }
    protected override void takeStep()
    {
        _targetIsInitPositon = !_targetIsInitPositon;

        _targetGridPos = _targetIsInitPositon ? _initPosition : _randomPosition;
    }
    void FindRandomPosition()
    {
        List<Vector2> possibleDir = new List<Vector2>() { Vector2.left, Vector2.right, Vector2.up, Vector2.down };

        bool find = false;
        while (!find && possibleDir.Count > 0)
        {
            Vector2 selectedDir = GlobalFuncs.randElem(possibleDir);
            Vector2 tempPos = _initPosition + selectedDir;
            if(pathIsClear(toWorldCoord( tempPos)))
            {
                find = true;
                _randomPosition = tempPos;

            }
            else
            {
                possibleDir.Remove(selectedDir);
            }

        }
        if (!find)
            _randomPosition = _initPosition;


    }

    public override void tileDetected(Tile detectedTile)
    {
        base.tileDetected(detectedTile);
        _shootTarget = detectedTile.transform;
    }
}
