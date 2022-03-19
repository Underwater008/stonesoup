using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Item {
    
  public enum ItemType {
    HealthPotion,
    BronzeKey,
    MagicWond,
  }

  public ItemType itemType;
  public int amount;

  //replace with the correct sprite.
  public Sprite GetSprite() {
    switch (itemType) {
    default:
      case ItemType.HealthPotion:     return ABX_ItemAssets.Instance.HealthPotionSprite;
      case ItemType.BronzeKey:        return ABX_ItemAssets.Instance.BronzeKeySprite;
      case ItemType.MagicWond:        return ABX_ItemAssets.Instance.MagicWondSprite;
    }
  }
}
