using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ABX_xiao23InventorySelect : MonoBehaviour
{
    const int SIZE = 6;

    public static ABX_xiao23InventorySelect MainInventory { get; private set; }

    public TMP_Text[] numberUI = new TMP_Text[SIZE];
    public Image[] backgroundUI = new Image[SIZE];
    public Image[] iconUI = new Image[SIZE];

    GameObject[] _items = new GameObject[SIZE];
    int[] _count = new int[SIZE];
    int _currentIndex = 0;

    ABX_Player player;
    public void Awake()
    {
        if (MainInventory != null)
            Destroy(gameObject);
        else
            MainInventory = this;

    }
    void Start()
    {
        for (int i = 0; i < SIZE; i++)
        {
            _items[i] = null;
            _count[i] = 0;
            iconUI[i].sprite = null;
            numberUI[i].text = null;
        }
        player = GetComponentInParent<ABX_Player>();
    }
    /*
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
    */
    /*
    public void RemoveItem(Tile tile)
    {
        string tileName = tile.name;
        if (tileName.Contains("(Clone)"))
        {
            tileName = tileName.Replace("(Clone)", "");
        }
        int index = GetItemIndex(tileName);
        if (index == -1)
            return;

        _count[index]--;

        itemChanged = true;
        UpdateUI();
    }

    int GetItemIndex(string itemName)
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] != null && _items[i].GetComponent<ABX_Tile>().name.Equals(itemName))
                return i;
        }
        return -1;
    }

    void UpdateUI()
    {
        for (int i = 0; i < _count.Length; i++)
        {

            SetItemUI(i, _count[i]);
        }
    }
    void SetItemUI(int index, int number)
    {
        I[index].transform.GetChild(0).gameObject.SetActive(number > 0);
        if (number == 0)
            I[index].GetComponentInChildren<TextMeshProUGUI>().text = "";
        else
            I[index].GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();
    }
    bool itemChanged;
    bool CanGet(Tile t)
    {
        foreach (var v in T)
        {
            if (v != null && v.GetComponent<Tile>().GetType() == t.GetType())
                return true;
        }
        return false;
    }
    */

    void ChooseItem()
    {
        /*
        if (Input.mouseScrollDelta.y > 0)
        {
            I[currentIndex].color = Color.white;
            currentIndex += 1;
            itemChanged = true;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            I[currentIndex].color = Color.white;
            currentIndex -= 1;
            itemChanged = true;
        }
        currentIndex = currentIndex % 6;
        if (currentIndex < 0)
            currentIndex += 6;

        I[currentIndex].color = new Color(255f / 255f, 178f / 255f, 32f / 255f);

        if (itemChanged)
        {
            Tile holdingTile = Player.instance.tileWereHolding;
            if (holdingTile != null && CanGet(holdingTile))
            {
                Destroy(Player.instance.tileWereHolding.gameObject);
            }

            if (count[currentIndex] > 0)
            {
                var itemGo = Instantiate(T[currentIndex].gameObject);
                itemGo.GetComponent<Tile>().pickUp(this);
                itemChanged = false;
            }

        }
        */
        //Get the current item index
        if (Input.mouseScrollDelta.y < 0)
        {
            _currentIndex += 1;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            _currentIndex -= 1;
        }
        _currentIndex = _currentIndex % 6;
        if (_currentIndex < 0)
            _currentIndex += 6;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            _currentIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            _currentIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            _currentIndex = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            _currentIndex = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            _currentIndex = 4;
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            _currentIndex = 5;

        if (_items[_currentIndex] == null)
            player.tileWereHolding = null;
        else
            player.tileWereHolding = _items[_currentIndex].GetComponent<ABX_Tile>();

        //Initialize and clear out items that no longer exist
        for (int i = 0; i < SIZE; i++)
        {
            if (_items[i] == null)
            {
                //iconUI[_currentIndex].sprite = null;
                iconUI[i].gameObject.SetActive(false);
                numberUI[i].text = "";
                if (i == _currentIndex)
                    backgroundUI[i].GetComponent<Image>().color = new Color(255f / 255f, 178f / 255f, 32f / 255f);
                else
                    backgroundUI[i].GetComponent<Image>().color = Color.white;
                continue;
            }

            //iconUI[i].gameObject.SetActive(true);
            //iconUI[i].sprite = _items[_currentIndex].GetComponent<SpriteRenderer>().sprite;

            numberUI[i].text = "" + _count[i];
            if (i == _currentIndex)
            {

                _items[i].SetActive(true);
                backgroundUI[i].color = new Color(255f / 255f, 178f / 255f, 32f / 255f);
            }
            else
            {
                _items[i].SetActive(false);
                backgroundUI[i].color = Color.white;
            }
        }
    }

    //return 0 if the item is a duplicate
    //return 1 if the item is not a duplicate
    //return -1 if the item is not added
    public int AddItem (ABX_Tile tile)
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (_items[i] == null)
                continue;
            if (tile.tileName.Equals(_items[i].GetComponent<ABX_Tile>().tileName))
            {
                _count[i]++;
                return 0;
            }
        }

        //If the object is not a duplicate
        for (int i = 0; i < SIZE; i++)
        {
            if (_items[i] == null)
            {
                _items[i] = tile.gameObject;
                _count[i]++;
                iconUI[i].gameObject.SetActive(true);
                iconUI[i].sprite = tile.gameObject.GetComponent<SpriteRenderer>().sprite;
                return 1;
            }
        }
        return -1;
    }

    void Update()
    {

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

    public GameObject GetCurrentItem ()
    {
        return _items[_currentIndex];
    }

    public int GetCurrentCount ()
    {
        return _count[_currentIndex];
    }

    public int GetItemCount (ABX_Tile tile)
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (_items[i].GetComponent<ABX_Tile>().tileName.Equals(tile.tileName))
            {
                return _count[i];
            }
        }
        return 0;
    }

    public int ConsumeCurrentItem (int cost)
    {
        if (_count[_currentIndex] < cost)
        {
            return -1;
        }
        else
        {
            _count[_currentIndex] -= cost;
            numberUI[_currentIndex].text = "" + _count[_currentIndex];
            if (_count[_currentIndex] == 0)
            {
                _items[_currentIndex] = null;
                iconUI[_currentIndex].sprite = null;
                iconUI[_currentIndex].gameObject.SetActive(false);
                numberUI[_currentIndex].text = "";
            }
            return _count[_currentIndex];
        }
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



