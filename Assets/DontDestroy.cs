using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour {

    public static bool elecFinished = false;
    public static bool elecFixed = false;
    public static bool infoFinished = false;
    public static bool infoFixed = false;

    public static float timer = 0;

    public static bool isMale;

    public static DontDestroy INSTANCE;
    public bool fadingIn = true;
    public bool fadingOut = false;

    public AudioClip gameMusic;
    public AudioClip menuMusic;

    public AudioSource MusicManagerSource;

    public Sprite malePj;
    public Sprite femalePj;

    // Use this for initialization
    void Start() {
        if (INSTANCE == null)
            INSTANCE = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator FadeOut(int a)
    {
        fadingOut = true;
        transform.GetChild(0).GetComponent<Animator>().SetBool("GoBlack", true);
        yield return new WaitForSeconds(1);
        transform.GetChild(0).GetComponent<Animator>().SetBool("GoBlack", false);
        fadingIn = true;
        fadingOut = false;
        SceneManager.LoadScene(a);
    }
    IEnumerator FadeOut(string a)
    {
        fadingOut = true;
        transform.GetChild(0).GetComponent<Animator>().SetBool("GoBlack", true);
        yield return new WaitForSeconds(1);
        transform.GetChild(0).GetComponent<Animator>().SetBool("GoBlack", false);
        fadingIn = true;
        fadingOut = false;
        if ((a == "TubosGiratorios" || a == "Informatica") && MusicManagerSource.clip != gameMusic)
        {
            MusicManagerSource.clip = gameMusic;
            MusicManagerSource.Play();
        }
        else if (MusicManagerSource.clip != menuMusic)
        {
            MusicManagerSource.clip = menuMusic;
            MusicManagerSource.Play();
        }
        SceneManager.LoadScene(a);
    }

    public void StartTheCoroutine(int a)
    {
        if (fadingOut == false)
            StartCoroutine(FadeOut(a));
    }

    public void StartTheCoroutine(string a)
    {
        if (fadingOut == false)
            StartCoroutine(FadeOut(a));
    }

    void Update()
    {
        if (MusicManagerSource.volume >= .6f)
            fadingIn = false;
        if (fadingIn)
        {
            MusicManagerSource.volume = Mathf.Lerp(MusicManagerSource.volume, .7f, Time.deltaTime);
        }
        if (fadingOut)
        {
            MusicManagerSource.volume = Mathf.Lerp(MusicManagerSource.volume, 0, Time.deltaTime);
        }
        Vector3 camPos = GameObject.Find("Main Camera").transform.position;
        MusicManagerSource.transform.position = camPos;

        if (SceneManager.GetActiveScene().name == "PeroniaLevel")
        {
            timer += Time.deltaTime;
            if (elecFixed)
            {
                GameObject.Find("Electro").GetComponent<Image>().fillAmount = 1;
                GameObject.Find("ElectroIcon").GetComponent<Image>().fillAmount = 1;
            }
            else if (elecFinished)
            {
                GameObject.Find("Electro").GetComponent<Image>().fillAmount = (timer - 1f) / 2f;
                GameObject.Find("ElectroIcon").GetComponent<Image>().fillAmount = (timer - 1f) / 2f;
                if (timer > 3)
                {
                    elecFixed = true;
                }
            }
            if (infoFixed)
            {
                GameObject.Find("Info").GetComponent<Image>().fillAmount = 1;
                GameObject.Find("InfoIcon").GetComponent<Image>().fillAmount = 1;
            }
            else if (infoFinished)
            {
                GameObject.Find("Info").GetComponent<Image>().fillAmount = (timer - 1f) / 2f;
                GameObject.Find("InfoIcon").GetComponent<Image>().fillAmount = (timer - 1f) / 2f;
                if (timer > 3)
                {
                    infoFixed = true;
                }
            }
        }
    }
}
