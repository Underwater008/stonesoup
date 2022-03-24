using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Resource : ABX_Tile {
    public enum ResourceTags {AlienMineral = 0, AlienPlant, AlienTissue};
    public ResourceTags resourceTag;

    public AudioClip SFPickUpMine;
    public AudioClip SFPickUpPlant;
    public AudioClip SFPickUpTissue;

    public bool hasResourceTag (ResourceTags tag) {
        if (resourceTag != tag)
            return false;
        return true;
	}

    //if the tile is colliding with the generated player. when we press space play each sound effects.
    private void OnTriggerStay2D(Collider2D _player) {

        if (_player.gameObject.name == "ABX_Player(Clone)") {
            if (Input.GetKeyDown(KeyCode.Space)) {

                //if we picked up mineral
                if (resourceTag == ResourceTags.AlienMineral)
                {
                    print("picked up Mineral");
                    AudioManager.playAudio(SFPickUpMine);
                }
                //if we picked up plant
                if (resourceTag == ResourceTags.AlienPlant)
                {
                    print("picked up Plant");
                    AudioManager.playAudio(SFPickUpPlant);
                }
                //if we picked up Tissue
                if (resourceTag == ResourceTags.AlienMineral)
                {
                    print("picked up Tissue");
                    AudioManager.playAudio(SFPickUpTissue);
                }
            }
        }
    }
}
