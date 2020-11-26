using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OpenSceneOnClick : MonoBehaviour
{
    public bool isChecker = false;

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
