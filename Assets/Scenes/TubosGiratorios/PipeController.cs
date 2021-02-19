using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PipeController : MonoBehaviour
{
    public static PipeController INSTANCE;

    public bool levelCreatorMode;
    public DialogPart tutorialTwo;

    public bool generateRandomEverything;

    public bool hasPositiveNegative;

    public int connectedPoints;

    [Header("Tutorial")]

    public GameObject cellHL;
    public GameObject batteryHL;

    [Header("Level Stuff")]

    public GameObject pipeCanvasGO;

    public float targetPercentInThisLevel;

    public GameObject levelToLoad;

    public int width, height;
    public float cellSize;

    public GameObject[,] pipesGO;
    public PipeBehaviour[,] pipesBe;

    public PipeBehaviour StartPipe;

    public GameObject pipeSlotPrefab;

    public Image loadBarImage;
    public Image loadBarTarget;
    public Text percentText;
    public ParticleSystem loadBarPSys;

    public GameObject nxtLvlBtn;

    public GameObject alert;
    public RectTransform canvasTransform;

    public int activeLevel;
    public bool levelFinished;

    public GameObject[] levels;

    public int dialogueProgress = 0;

    void Awake () {
        INSTANCE = this;
        CreateLevel();
    }
    void Start()
    {
        DialogueController.INSTANCE.StartDialogue();
        GameObject.Find("PipeCanvas").SetActive(false);
        StartCoroutine(eofStart());
    }

    IEnumerator eofStart()
    {
        yield return new WaitForEndOfFrame();
        StartPipe.checkDelay = true;
        connectedPoints = 0;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("TubosGiratorios");
    }

    public void CreateLevel()
    {
        if (activeLevel == 6)
        {
            DialogueController.INSTANCE.activeChat = tutorialTwo;
            DialogueController.INSTANCE.StartDialogue();
            GameObject.Find("PipeCanvas").SetActive(false);
        }
        if (activeLevel == 9)
        {
            DontDestroy.timer = -2;
            DontDestroy.elecFinished = true;
            SceneManager.LoadScene("PeroniaLevel");
        }
        levelFinished = false;
        if (levelCreatorMode)
        {
            pipesGO = new GameObject[width, height];
            pipesBe = new PipeBehaviour[width, height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pipesGO[j, i] = GameObject.Instantiate(pipeSlotPrefab, new Vector3(-width / 2f + j, height / 2f - i, 0) * cellSize + new Vector3(cellSize / 2, -cellSize / 2, 0), new Quaternion(0, 0, 0, 1), transform.GetChild(0));
                    pipesBe[j, i] = pipesGO[j, i].transform.GetChild(0).GetComponent<PipeBehaviour>();
                    pipesGO[j, i].name += j + "  " + i;
                    pipesBe[j, i].j = j;
                    pipesBe[j, i].i = i;
                }
            }
        }
        else
        {
            loadBarImage.fillAmount = 0;
            connectedPoints = 0;
            loadBarImage.fillAmount = 0;
            Destroy(transform.GetChild(0).gameObject);
            GameObject loadedLevel = Instantiate(levels[activeLevel], transform);
            width = loadedLevel.transform.GetChild(0).GetChild(0).GetComponent<PipeBehaviour>().width;
            height = loadedLevel.transform.GetChild(0).GetChild(0).GetComponent<PipeBehaviour>().height;
            pipesGO = new GameObject[width, height];
            pipesBe = new PipeBehaviour[width, height];
            targetPercentInThisLevel = loadedLevel.GetComponent<LvlToFillData>().targetPercent;
            hasPositiveNegative = loadedLevel.GetComponent<LvlToFillData>().hasPositiveNegative;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pipesGO[j, i] = loadedLevel.transform.GetChild((j+i*width)).gameObject;
                    pipesBe[j, i] = pipesGO[j, i].GetComponentInChildren<PipeBehaviour>();
                }
            }
        }
        loadBarTarget.rectTransform.localPosition = new Vector3(loadBarTarget.rectTransform.localPosition.x, Mathf.Lerp(loadBarImage.GetComponent<RectTransform>().rect.yMin, loadBarImage.GetComponent<RectTransform>().rect.yMax, targetPercentInThisLevel/100f), 0);
        percentText.text = targetPercentInThisLevel + "%";
    }

    public void CheckLevel()
    {
        if (connectedPoints / 20f * 100 >= targetPercentInThisLevel && !levelFinished)
        {
            levelFinished = true;
            //targetPercentInThisLevel = 1000;
            activeLevel++;
            if (activeLevel < levels.Length)
            //nxtLvlBtn.SetActive(true);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (pipesBe[j, i].isConnected)
                        pipesBe[j, i].StartCoroutine("ActivateWithDelay");
                }
            }
            StartCoroutine(LoadLevelDelay());
        }
        else
        {
            GameObject alertAux = Instantiate(alert, canvasTransform);
            Destroy(alertAux, 2f);
        }
    }

    IEnumerator LoadLevelDelay()
    {
        yield return new WaitForSeconds(4f);
        CreateLevel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DontDestroy.timer = -2;
            DontDestroy.elecFinished = true;
            SceneManager.LoadScene("PeroniaLevel");
        }

        if (levelFinished)
        {
            loadBarImage.fillAmount = Mathf.Lerp(loadBarImage.fillAmount, (float)connectedPoints / 20f, Time.deltaTime * 1.5f);
            loadBarPSys.emissionRate = 50;
        }
        else
        {
            loadBarPSys.emissionRate = 0;
        }
        loadBarPSys.transform.position = new Vector3(loadBarPSys.transform.position.x, Mathf.Lerp(-1.167757f, 1.106f, loadBarImage.fillAmount), loadBarPSys.transform.position.z);
        dialogueProgress = DialogueController.INSTANCE.progress;

    }

    IEnumerator endofframe()
    {
        yield return new WaitForEndOfFrame();

        StartPipe.CheckActive(1);
    }

    public void FinishedDialogue()
    {
        Debug.Log("asd");
        pipeCanvasGO.SetActive(true);
    }

    public void DialogueProgress(int a)
    {
        batteryHL.SetActive(false);
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
        }
    }

    public void ExitToMain()
    {
        DontDestroy.INSTANCE.StartTheCoroutine("Electronica");
    }
}
