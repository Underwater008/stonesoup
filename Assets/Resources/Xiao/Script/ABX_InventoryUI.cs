using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ABX_InventoryUI : MonoBehaviour
{
  private ABX_Inventory inventory;    //Reference to our inventory
  public Transform itemSlotContainer;
  public  Transform itemSlotTemplate;

  private void Awake() {
    //itemSlotContainer = transform.Find("itemSlotContainer");
    //itemSlotTemplate = itemSlotTemplate.Find("itemSlotTemplate");
  }
  //set a function to recive our incentory
  public void SetInventory(ABX_Inventory inventory) {
    this.inventory = inventory;
    RefreshInventoryItems();
  }

  private void RefreshInventoryItems() {
    int x = 0;
    int y = 0;
    float itemSlotCellSize = 75f;
    //generate items in backpack
    foreach (ABX_Item item in inventory.GetItemList()) {
      RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
      itemSlotRectTransform.gameObject.SetActive(true);
      itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
      Image image = itemSlotRectTransform.GetComponent<Image>();
      image.sprite = item.GetSprite();
      x++; //place the next item to the right.
      
    }
  }
}
