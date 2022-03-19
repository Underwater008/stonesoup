using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_ItemAssets : MonoBehaviour
{
  // making a singleton
  public static ABX_ItemAssets Instance { get; private set; }

  private void Awake() {
    Instance = this;
  }

  //our items' sprites, also add them to ABX_Item.

  //Abby

  //Billy

  //Xiao
  public Sprite BronzeKeySprite;
  public Sprite HealthPotionSprite;
  public Sprite MagicWondSprite;
}
