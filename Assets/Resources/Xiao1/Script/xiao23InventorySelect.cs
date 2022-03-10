using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class xiao23InventorySelect : Tile {

    public static xiao23InventorySelect MainInventory { get; private set; }
    public List<Image> I = new List<Image>();
    public List<GameObject> T = new List<GameObject>();
    int[] count = new int[6];

    int numX = 0;
    public void Awake()
    {
        if (MainInventory != null)
            Destroy(gameObject);
        else
            MainInventory = this;
    }
    void Start() {
        I[0].color = Color.blue;
        for (int i = 0; i < 6; i++)
        {
            count[i] = 0;
        }

    }
    public bool PutItemInBag(Tile tile)
    {
        string tileName = tile.name;
        if (tileName.Contains("(Clone)"))
        {
            tileName = tileName.Replace("(Clone)", "");
        }
        int index = GetItemIndex(tileName);
        if (index == -1)
            return false;

        count[index]++;

        itemChanged = true;
        UpdateCountUI();
        return true;

    }
    public void RemoveItem(Tile tile)
    {
        string tileName = tile.name;
        if (tileName.Contains("(Clone)"))
        {
            tileName = tileName.Replace("(Clone)", "");
        }
        int index = GetItemIndex(tileName);
        if (index == -1)
            return ;

        count[index]--;

        itemChanged = true;
        UpdateCountUI();
    }

    int GetItemIndex(string itemName)
    {
        for (int i = 0; i < T.Count; i++)
        {
            if (T[i]!=null && T[i].name == itemName)
                return i;
        }
        return -1;
    }

    void UpdateCountUI()
    {
        for (int i = 0; i < count.Length; i++)
        {

            SetItemAmountUI(i, count[i]);
        }
    }
    void SetItemAmountUI(int index,int number)
    {
        I[index].transform.GetChild(0).gameObject.SetActive(number > 0);
        I[index].GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();
    }
    bool itemChanged;
    bool CanGet(Tile t)
    {
        foreach(var v in T)
        {
            if (v != null && v.GetComponent<Tile>().GetType() == t.GetType())
                return true;
        }
        return false;
    }
    void ChooseItem()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            I[numX].color = Color.white;
            numX += 1;
            itemChanged = true;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            I[numX].color = Color.white;
            numX -= 1;
            itemChanged = true;
        }
        numX = numX % 6;
        if (numX < 0)
            numX += 6;
        I[numX].color = new Color(255f / 255f, 178f / 255f, 32f / 255f);

        if(itemChanged)
        {
            Tile holdingTile = Player.instance.tileWereHolding;
            if (holdingTile != null && CanGet(holdingTile))
            {
                Destroy(Player.instance.tileWereHolding.gameObject);
            }
           
            if(count[numX]>0)
            {
                var itemGo = Instantiate(T[numX].gameObject);
                itemGo.GetComponent<Tile>().pickUp(this);
                itemChanged = false;
            }
         
        }

     

    }

    void Update() {

        ChooseItem();


        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    Debug.Log("press B");
        //    if (Player.instance.tileWereHolding == null)        // if our hands are empty
        //    {
        //        print("hand Empty");
        //        if (T[numX]!=null)              // if we have something in the selected bag
        //        {
        //            pickUp(T[numX].GetComponent<Tile>());   // we pick up this tile
        //        }
        //    }

        //    if (Player.instance.tileWereHolding != null)        // if we are holding tile
        //    {
        //        Debug.Log("WE ARE HOLDING TILE");
        //        if (count[numX] == 0)            //if the selected bag is empty
        //        {
        //            Debug.Log(2);
        //            if (Player.instance.tileWereHolding.gameObject == T[numX])        // if the tile we are holding 
        //            {
        //                Debug.Log("1");
        //                count[numX] = 1;
        //                GameObject obj = Instantiate(T[numX]);
        //                obj.transform.parent = I[numX].transform;
        //                obj.transform.position = new Vector3(0, 0, 0);
        //            }
        //        }
        //    }
        //}

    }

     

        
        /*if (true) {
          for (int i = 0; i < 5; i++) {
            if (array[i] == 0) {
            
            }
            else {
              continue;
            }
          }
        }*/
  
    }

    //public override void 



