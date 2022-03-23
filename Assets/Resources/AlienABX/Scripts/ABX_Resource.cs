using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Resource : ABX_Tile
{
    public enum ResourceTags {AlienMineral = 0, AlienPlant, AlienTissue};
    public ResourceTags resourceTag;

    public bool hasResourceTag (ResourceTags tag)
    {
        if (resourceTag != tag)
            return false;
        return true;
    }
}
