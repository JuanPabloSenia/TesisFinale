using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour {

    public bool shouldWave = false;

    public static DialogueController INSTANCE;

    public bool onDialogue = false;

    public GameObject receiver;

    public GameObject chat;
    public GameObject yesNoGO;
    public GameObject nextGO;

    public Text namePlayer;
    public Text nameIng;
    public Text chatTextLeft;
    public Text chatTextRight;
    
    public Image imagePlayer;
    public Image imageIng;

    public DialogPart activeChat;

    public int progress;

    public bool nextEnds = false;

    void Awake () {
        INSTANCE = this;
        progress = 0;
        imagePlayer.sprite = DontDestroy.isMale ? DontDestroy.INSTANCE.malePj : DontDestroy.INSTANCE.femalePj;
	}
    public void NextDialogue()
    {
        if (activeChat.characters.Length > progress)
        {
            if (nextEnds)
            {
                EndDialogue();
            }
            receiver.SendMessage("DialogueProgress", progress, SendMessageOptions.DontRequireReceiver);
            bool isPJ = activeChat.characters[progress] == DialogPart.Personaje.PJ;
            imagePlayer.gameObject.SetActive(isPJ);
            imageIng.gameObject.SetActive(!isPJ);
            //nameIng.text = activeChat.characters[progress].ToString();
            //namePlayer.text = "Mike";
            yesNoGO.SetActive(false);
            nextGO.SetActive(true);
            if (activeChat.yesNo[progress])
            {
                yesNoGO.SetActive(true);
                nextGO.SetActive(false);
                chatTextLeft.text = "";
            }
            else
            {
                if (isPJ)
                {
                    chatTextLeft.text = activeChat.texts[progress];
                }
                else
                {
                    chatTextRight.text = activeChat.texts[progress];
                }
            }
            if (progress == 3 && SceneManager.GetActiveScene().buildIndex == 0) progress += 2;
            if (activeChat.isEnder[progress])
            {
                nextEnds = true;
            }
            progress++;
        }
        else
            EndDialogue();
    }

    public void YesNo(bool isYes)
    {
        if (!isYes) progress += 2;
        NextDialogue();
        //if (isYes) progress++;
    }

    public void StartDialogue()
    {
        if (shouldWave)
            GameObject.Find("PJ_Ingeniero").GetComponent<Animator>().SetTrigger("Wave");
        onDialogue = true;
        chat.SetActive(true);
        progress = 0;
        nextEnds = false;
        NextDialogue();
    }

    public void EndDialogue()
    {
        if (shouldWave)
            GameObject.Find("PJ_Ingeniero").GetComponent<Animator>().SetTrigger("Wave");
        onDialogue = false;
        chat.SetActive(false);
        receiver.SendMessage("DialogueProgress", progress, SendMessageOptions.DontRequireReceiver);
        receiver.SendMessage("FinishedDialogue", SendMessageOptions.DontRequireReceiver);
    }
}
