using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Random Choice room is a simple room that just 
// Chooses from a list of other rooms when it's created. 
// Think of it as the "pick a card from this hand of rooms" option
public class ABX_xiao23Room : Room {

	public GameObject[] roomChoices;
	//public GameObject backPackRoom;
    public bool iscreated;
    float timeToFalse = 0;

     private void Awake()
     {
         iscreated = false;
       
     }

    public void Update()
    {
        Debug.Log(iscreated);
        timeToFalse += Time.deltaTime;
        if (timeToFalse >= 1f)
        {
            iscreated = false;

            timeToFalse = 0;
        }
    }

    public override Room createRoom(ExitConstraint requiredExits)
     {
        //Debug.Log(iscreated);
        GameObject roomPrefab = GlobalFuncs.randElem(roomChoices);
        if (iscreated == false)
        {
            Debug.Log("Back Pack Created");
            iscreated = true;
            roomPrefab = roomChoices[0];
            
            return roomPrefab.GetComponent<Room>().createRoom(requiredExits);
            
        }
        else
        {
            
            roomPrefab = roomChoices[Random.Range(1,roomChoices.Length)];
            return roomPrefab.GetComponent<Room>().createRoom(requiredExits);
        }
    }
}

