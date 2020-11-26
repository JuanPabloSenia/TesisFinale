using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour {

    public static CharacterMovement INSTANCE;

    public NavMeshAgent navMeshAgent;

    public SpriteRenderer playerSprite;
    public Animator anim;
    public Sprite plFront;
    public Sprite plBack;
    public Transform spriteTransform;

    public GameObject spritePlay;
    public GameObject spriteTalk;
    public GameObject spriteInfo;
    public GameObject spriteMenu;

    Vector2 pastPosition;

	void Start () {
        pastPosition = new Vector2(transform.position.x, transform.position.z);
        INSTANCE = this;
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (!DialogueController.INSTANCE.onDialogue)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
        Vector2 actualPos = new Vector2(transform.position.x, transform.position.z);
        if (actualPos != pastPosition)
        {
            /*playerSprite.flipX = (pastPosition.x < actualPos.x) ? false : true;
            playerSprite.sprite = (pastPosition.y < actualPos.y) ? plBack : plFront;
            pastPosition = actualPos;*/
            if ((actualPos - pastPosition).magnitude > .013f)
            {
                anim.SetBool("moving", true);
            }
            else
            {
                anim.SetBool("moving", false);
            }
            playerSprite.flipX = (pastPosition.x < actualPos.x) ? true : false;
            anim.SetBool("up", pastPosition.y < actualPos.y);
            pastPosition = actualPos;
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "collPlay":
                spritePlay.SetActive(true);
                break;
            case "collTalk":
                spriteTalk.SetActive(true);
                break;
            case "collInfo":
                spriteInfo.SetActive(true);
                break;
            case "collMenu":
                spriteMenu.SetActive(true);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.name)
        {
            case "collPlay":
                spritePlay.SetActive(false);
                break;
            case "collTalk":
                spriteTalk.SetActive(false);
                break;
            case "collInfo":
                spriteInfo.SetActive(false);
                break;
            case "collMenu":
                spriteMenu.SetActive(false);
                break;
        }
    }
}
