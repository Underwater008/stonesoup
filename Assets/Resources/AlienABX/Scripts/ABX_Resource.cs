using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Resource : ABX_Tile
{
    public enum ResourceTags {AlienMineral = 0, AlienPlant, AlienTissue};
    public ResourceTags resourceTag;

    public AudioClip SFPickUpMine;

    public bool hasResourceTag (ResourceTags tag)
    {
        if (resourceTag != tag)
            return false;
        return true;
	}

    private void OnTriggerStay2D(Collider2D _player)
    {
        if (_player.gameObject.name == "ABX_Player(Clone)")
        {
            Debug.Log("MineTouchingPlayer");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioManager.playAudio(SFPickUpMine);
            }
        }
    }
}
