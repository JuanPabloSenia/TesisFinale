using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CameraController : MonoBehaviour, IPointerDownHandler
{
    public string link;

    public string playScene;

    public float topMax;
    public float topMin;
    public float rightMax;
    public float rightMin;

    public Transform player;
    public Vector3 targetPos;
    public float camSpeed;

    public string gameSceneName;

    Vector3 aux;
    void Start () {
        targetPos.y = transform.position.y;
	}

    void Update() {
        if (player == null)
            GameObject.FindGameObjectWithTag("Player");
        targetPos.x = player.position.x;
        targetPos.z = player.position.z;
        if (targetPos.x > rightMin && targetPos.x < rightMax)
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x, transform.position.y, transform.position.z), Time.deltaTime * camSpeed);
        if (targetPos.z > topMin && targetPos.z < topMax)
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, targetPos.z), Time.deltaTime * camSpeed);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward);
            aux = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.name == "buttonPlay")
                {
                    ProgrammingMain.currentLevel = 1;
                    DontDestroy.INSTANCE.StartTheCoroutine(gameSceneName);
                    CharacterMovement.INSTANCE.navMeshAgent.isStopped = true;
                }
                if (hit.collider.name == "buttonTalk")
                {
                    DialogueController.INSTANCE.StartDialogue();
                    CharacterMovement.INSTANCE.navMeshAgent.isStopped = true;
                }
                if (hit.collider.name == "buttonInfo")
                {
                    Application.OpenURL(link);
                    CharacterMovement.INSTANCE.navMeshAgent.isStopped = true;
                }
                if (hit.collider.name == "buttonMenu")
                {
                    DontDestroy.INSTANCE.StartTheCoroutine("TreeLevelSelection");
                    CharacterMovement.INSTANCE.navMeshAgent.isStopped = true;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData ped)
    {
        Debug.Log("asd");
        aux = Camera.main.ScreenToWorldPoint(ped.position);
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.ScreenToWorldPoint(ped.position), Vector3.forward);
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.name == "buttonPlay")
            {
                SceneManager.LoadScene(playScene);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(aux, transform.forward * 100);
    }
}
