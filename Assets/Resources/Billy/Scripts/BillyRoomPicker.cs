using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillyRoomPicker : Room
{
    public BillyValidatedRoom[] roomsPrefab;
    public override Room createRoom(ExitConstraint requiredExits)
    {
        List<BillyValidatedRoom> validatedRooms = new List<BillyValidatedRoom>();
        foreach (BillyValidatedRoom room in roomsPrefab)
        {
            if (room.MeetsConstraints(requiredExits))
            {
                validatedRooms.Add(room);
            }
        }
        return GlobalFuncs.randElem(validatedRooms).createRoom(requiredExits);
    }
}
