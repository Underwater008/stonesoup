using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Inventory {

  private List<ABX_Item> itemList;

  public ABX_Inventory() {
    itemList = new List<ABX_Item>();

    AddItem(new ABX_Item { itemType = ABX_Item.ItemType.BronzeKey, amount = 1 });
    AddItem(new ABX_Item { itemType = ABX_Item.ItemType.HealthPotion, amount = 1 });
    AddItem(new ABX_Item { itemType = ABX_Item.ItemType.MagicWond, amount = 1 });
    Debug.Log(itemList.Count);
  }
   public void AddItem(ABX_Item item) {
    itemList.Add(item);
  }

  //Expose our item list
  public List<ABX_Item> GetItemList() {
    return itemList;
  }
}
