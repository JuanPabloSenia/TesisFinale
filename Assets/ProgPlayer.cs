using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            ProgrammingMain.currentCoins++;
            Debug.Log(ProgrammingMain.currentCoins);
            other.gameObject.SetActive(false);
        }
    }
}
