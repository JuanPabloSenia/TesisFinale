using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OpenSceneOnClick : MonoBehaviour
{
    public bool isChecker = false;

    public bool isSelectMale = false;
    public bool isSelectFemale = false;

    public string sceneToOpen;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (isChecker)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit hit;
                Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.transform.GetComponent<OpenSceneOnClick>().isSelectFemale)
                    {
                        DontDestroy.isMale = false;
                    }
                    if (hit.transform.GetComponent<OpenSceneOnClick>().isSelectMale)
                    {
                        DontDestroy.isMale = true;
                    }

                    hit.transform.GetComponent<OpenSceneOnClick>().StartScene();
                }
            }
        }
    }

    public void StartScene()
    {
        DontDestroy.INSTANCE.StartTheCoroutine(sceneToOpen);
    }
}
