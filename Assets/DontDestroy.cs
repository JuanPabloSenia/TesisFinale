using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

    public static DontDestroy INSTANCE;
    bool fadingIn = true;
    bool fadingOut = false;

	// Use this for initialization
	void Start () {
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

    public void StartTheCoroutine(int a)
    {
        StartCoroutine(FadeOut(a));
    }

    void Update()
    {
        if (Camera.main.GetComponent<AudioSource>().volume >= .6f)
            fadingIn = false;
        if (fadingIn)
        {
            Camera.main.GetComponent<AudioSource>().volume = Mathf.Lerp(Camera.main.GetComponent<AudioSource>().volume, .7f, Time.deltaTime);
        }
        if (fadingOut)
        {
            Camera.main.GetComponent<AudioSource>().volume = Mathf.Lerp(Camera.main.GetComponent<AudioSource>().volume, 0, Time.deltaTime);
        }
    }
}
