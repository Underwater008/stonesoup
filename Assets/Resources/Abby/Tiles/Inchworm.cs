using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using a lot of ideas from "BasicAICreature" class
public class Inchworm : BasicAICreature
{

    protected override void takeStep()
    {
        // Here's the function you can override to figure out how your AI object moves.
        // takeStep will USUALLY set _targetGridPos to do this. 
        // _targetGridPos = Tile.toGridCoord(globalX, globalY);
        _targetGridPos = Player.toGridCoord(globalX, globalY);
        Debug.Log(_targetGridPos);
    }


}


