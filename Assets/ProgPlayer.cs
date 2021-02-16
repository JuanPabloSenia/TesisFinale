using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgPlayer : MonoBehaviour
{
    TMPro.TextMeshPro text;

    private void Start()
    {
        text = GameObject.Find("Counter").GetComponent<TMPro.TextMeshPro>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            ProgrammingMain.currentCoins++;
            text.text = ProgrammingMain.currentCoins+"/"+ProgrammingMain.totalCoinsStatic;
            Debug.Log(ProgrammingMain.currentCoins);
            other.gameObject.SetActive(false);
        }
    }
}
