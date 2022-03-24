using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_PlantExplode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Die()); 
    }

    IEnumerator Die ()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
