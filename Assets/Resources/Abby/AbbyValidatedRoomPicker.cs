using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbbyValidatedRoomPicker : Room
{
    public Room[] RoomChoices;
    public override Room createRoom(ExitConstraint requiredExits)
    {
        List<Room> roomsThatMeetConstraints = new List<Room>();

        foreach (Room room in RoomChoices)
        {
            AbbyValidatedRoom validatedRoom = room.GetComponent<AbbyValidatedRoom>();

            if (validatedRoom.MeetsConstraints(requiredExits))
                roomsThatMeetConstraints.Add(validatedRoom);
        }

        Room randomRoom = GlobalFuncs.randElem(roomsThatMeetConstraints);
        return randomRoom.createRoom(requiredExits);
    }
}
