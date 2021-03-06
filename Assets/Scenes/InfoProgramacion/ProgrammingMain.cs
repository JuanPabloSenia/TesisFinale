﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgrammingMain : MonoBehaviour
{
    public DialogueController dialog;

    public static int currentLevel = 1;
    public static int maxLevel = 6;

    public bool isPlaying = false;

    public int cursor = 0;

    public bool repeatStarted = false;
    public int repeatStartedAt = 0;

    public InfoAction[] possibleActions;

    public Transform textContainer;
    public Transform[] letters;
    public Transform cursorTransform;
    public bool[] usedLetter;

    public int progressInGame = 0;
    public Transform playerTransform;

    public Transform coinsContainer;

    public static int currentCoins = 0;
    public int totalCoins = 0;
    public static int totalCoinsStatic = 0;

    public GameObject doingHighlight;
    public GameObject gratzScreen;
    public GameObject tryButton;
    public GameObject cancelButton;

    public RectTransform canvasTransform;
    public GameObject badRepeatsAlert;

    public bool isTutorial = false;

    Vector3 ogPos;
    Quaternion ogRot;

    public enum actionName
    {
        EMPTY,
        MOVE,
        ROTATE_LEFT,
        ROTATE_RIGHT,
        START_REPEAT,
        END_REPEAT
    }
    public List<InfoAction> currentList = new List<InfoAction>();

    void Start()
    {
        ogPos = playerTransform.position;
        ogRot = playerTransform.rotation;
        if (dialog != null)
            dialog.StartDialogue();
        totalCoins = coinsContainer.childCount;
        totalCoinsStatic = totalCoins;
        GameObject.Find("Counter").GetComponent<TMPro.TextMeshPro>().text = "0/" + totalCoins;
        currentCoins = 0;

        letters = new Transform[textContainer.childCount];
        for (int i = 0; i < letters.Length; i++)
        {
            letters[i] = textContainer.GetChild(i);
        }
        TypeList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DontDestroy.timer = -2;
            DontDestroy.infoFinished = true;
            SceneManager.LoadScene("PeroniaLevel");
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (dialog != null)
            {
                isTutorial = dialog.onDialogue;
            }
            if (Physics.Raycast(ray, out hit, 100) && !isTutorial)
            {
                switch (hit.collider.transform.name)
                {
                    case "ExitLevel":
                        SceneManager.LoadScene("Informatica");
                        break;
                    case "Replay":
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                        break;
                    case "NextLevel":
                        if (currentLevel == maxLevel)
                        {
                            DontDestroy.timer = -2;
                            DontDestroy.infoFinished = true;
                            SceneManager.LoadScene("PeroniaLevel");
                        }
                        else
                        {
                            currentLevel++;
                            SceneManager.LoadScene("Info_Lvl_" + currentLevel);
                        }
                        break;
                    case "Left":
                        if (cursor > 0)
                        {
                            cursor--;
                        }
                        break;
                    case "Right":
                        if (cursor < currentList.Count)
                        {
                            cursor++;
                        }
                        break;
                    case "Try":
                        if (currentList.Count == 0)
                        {
                            break;
                        }
                        if (RepeatsAreGood())
                        {
                            Debug.Log("GoodRepeats");
                        }
                        else
                        {
                            Debug.Log("BadRepeats");
                            GameObject alertAux = Instantiate(badRepeatsAlert, canvasTransform);
                            Destroy(alertAux, 2f);
                            break;
                        }
                        ResetWorld();
                        isPlaying = true;
                        tryButton.SetActive(false);
                        cancelButton.SetActive(true);
                        usedLetter = new bool[currentList.Count];
                        StartCoroutine(Play());
                        break;
                    case "Cancel":
                        ResetWorld();
                        isPlaying = false;
                        StopAllCoroutines();
                        tryButton.SetActive(true);
                        cancelButton.SetActive(false);
                        doingHighlight.SetActive(false);
                        break;
                    case "Move":
                        if (cursor < letters.Length && !isPlaying)
                        {
                            currentList.Insert(cursor, possibleActions[1]);
                            cursor++;
                        }
                        break;
                    case "RotateLeft":
                        if (cursor < letters.Length && !isPlaying)
                        {
                            currentList.Insert(cursor, possibleActions[2]);
                            cursor++;
                        }
                        break;
                    case "RotateRight":
                        if (cursor < letters.Length && !isPlaying)
                        {
                            currentList.Insert(cursor, possibleActions[3]);
                            cursor++;
                        }
                        break;
                    case "StartRepeat":
                        if (cursor < letters.Length && !isPlaying)
                        {
                            currentList.Insert(cursor, possibleActions[4]);
                            cursor++;
                        }
                        break;
                    case "EndRepeat":
                        if (cursor < letters.Length && !isPlaying)
                        {
                            currentList.Insert(cursor, possibleActions[5]);
                            cursor++;
                        }
                        break;
                    case "Delete":
                        if (cursor > 0 && !isPlaying)
                        {
                            currentList.RemoveAt(cursor - 1);
                            cursor--;
                        }
                        break;
                }
                TypeList();
            }
        }
    }

    void TypeList()
    {
        for (int i = 0; i < letters.Length; i++)
        {
            letters[i].GetComponent<SpriteRenderer>().sprite = possibleActions[0].image;
            if (i < currentList.Count && currentList.Count != 0)
            {
                letters[i].GetComponent<SpriteRenderer>().sprite = currentList[i].image;
            }
            if (i == cursor)
            {
                cursorTransform.position = letters[i].transform.position - Vector3.right + (Vector3.right / 2);
            }
        }
        //Exception for the cursor to work on the last letter
        if (cursor == letters.Length && letters.Length > 0)
        {
            Debug.Log(letters.Length - 1);
            cursorTransform.position = letters[letters.Length-1].transform.position + Vector3.right;
        }
    }

    void ResetWorld()
    {
        currentCoins = 0;
        for (int i = 0; i < coinsContainer.childCount; i++)
        {
            coinsContainer.GetChild(i).gameObject.SetActive(true);
        }
        playerTransform.position = ogPos;
        playerTransform.rotation = ogRot;
        progressInGame = 0;
    }

    bool RepeatsAreGood()
    {
        int openRepeats = 0;
        for (int i = 0; i < currentList.Count; i++)
        {
            if (currentList[i].aName == actionName.START_REPEAT)
            {
                openRepeats++;
            }
            if (currentList[i].aName == actionName.END_REPEAT)
            {
                openRepeats--;
                if(openRepeats < 0)
                {
                    return false;
                }
            }
        }
        if (openRepeats == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator Play()
    {
        if (isPlaying)
        {
            yield return new WaitForSeconds(.8f);
        }
        if (isPlaying)
        {
            if (progressInGame <= currentList.Count - 1)
            {
                switch (currentList[progressInGame].aName)
                {
                    case actionName.MOVE:
                        bool shouldMove = true;
                        RaycastHit hit;
                        if (Physics.Raycast(playerTransform.position, playerTransform.right, out hit, 4))
                        {
                            if (hit.collider.tag == "ProgObstacle")
                            {
                                shouldMove = false;
                            }
                        }
                        if (shouldMove)
                        {
                            playerTransform.position += playerTransform.right * 4f;
                        }
                        break;
                    case actionName.ROTATE_LEFT:
                        playerTransform.Rotate(new Vector3(0, 0, 90));
                        break;
                    case actionName.ROTATE_RIGHT:
                        playerTransform.Rotate(new Vector3(0, 0, -90));
                        break;
                    case actionName.END_REPEAT:
                        if (!usedLetter[progressInGame])
                        {
                            usedLetter[progressInGame] = true;
                            for (int i = progressInGame; i >= 0; i--)
                            {
                                if (currentList[i].aName == actionName.START_REPEAT && !usedLetter[i])
                                {
                                    usedLetter[i] = true;
                                    progressInGame = i;
                                    Debug.Log(progressInGame);
                                    break;
                                }
                            }
                        }
                        break;
                }
                {
                    doingHighlight.SetActive(true);
                    doingHighlight.transform.position = letters[progressInGame].position - Vector3.forward;
                    progressInGame++;
                    if (progressInGame < currentList.Count)
                    {
                        while (usedLetter[progressInGame])
                        {
                            progressInGame++;
                            if (progressInGame >= currentList.Count)
                            {
                                break;
                            }
                        }
                    }
                    StartCoroutine(Play());
                }
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
                if (currentCoins < totalCoins)
                {
                    ResetWorld();
                    Debug.Log("Ended Wrong");
                    isPlaying = false;
                    tryButton.SetActive(true);
                    cancelButton.SetActive(false);
                    doingHighlight.SetActive(false);
                }
                else
                {
                    gratzScreen.SetActive(true);
                    Debug.Log("Ended Right");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            currentCoins++;
            Destroy(other.gameObject);
        }
    }

    public void DialogueProgress(int a)
    {

        switch (a)
        {
            case 2:
                isTutorial = false;
                break;
        }
        /*batteryHL.SetActive(false);
        cellHL.SetActive(false);
        if (activeLevel == 0)
        {
            switch (a)
            {
                case 2:
                    cellHL.SetActive(true);
                    break;
                case 3:
                    batteryHL.SetActive(true);
                    break;
            }
        }*/
    }
}