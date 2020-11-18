using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PipeBehaviour : MonoBehaviour {

    public Animator fillAnim;

    public bool canRotate;
    Transform t;
    Quaternion targetRotation;
    public Animator anim;

    int fillRotation;
    int actualRotIndex;

    public bool isConnected;
    public bool toCheck;

    public bool startPipe = false;
    public bool targetPipe = false;

    [Space(10)]
    public bool up;
    public bool right, down, left;

    [Space(10)]
    [Header("0none-1pos-2neg")]
    public int upPositive;
    public int rightPositive, downPositive, leftPositive;

    [Space(10)]
    public float activationOrder;

    public PipeTypes pipe;
    public int boost = 0;
    public Transform spriteT;
    public Transform bgT;

    public Sprite spriteTwo;
    public Sprite spriteThree;
    public Sprite spriteFour;
    public Sprite spriteL;
    public Sprite spriteNothing;

    public Sprite bgTwo;
    public Sprite bgThree;
    public Sprite bgFour;
    public Sprite bgL;

    public Sprite BGNormal;
    public Sprite BGBoosted;
    public Sprite BGDebuffed;

    public Sprite positive;
    public Sprite negative;

    public GameObject onOff;

    public SpriteRenderer background;

    public int j, i;

    public SpriteRenderer spup;
    public SpriteRenderer spdown;
    public SpriteRenderer spright;
    public SpriteRenderer spleft;

    public int width, height;

    public bool checkDelay = false;

    public GameObject startSprite;

    private void OnValidate()
    {
        SetSprite();
    }

    public enum PipeTypes
    {
        TWO_HORIZONTAL,
        TWO_VERTICAL,
        THREE_UP,
        THREE_RIGHT,
        THREE_DOWN,
        THREE_LEFT,
        L_IZQ_ARRIBA,
        L_IZQ_ABAJO,
        L_DER_ARRIBA,
        L_DER_ABAJO,
        FOUR,
        NOTHING
    }

    void Start () {
        if (i == 0 && j == 0 && PipeController.INSTANCE.levelCreatorMode)
        {
            width = PipeController.INSTANCE.width;
            height = PipeController.INSTANCE.height;
        }
        bgT = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().transform;
        if (startPipe)
            PipeController.INSTANCE.StartPipe = this;
        t = transform;
        targetRotation = new Quaternion(0, 0, 0, 1);
        transform.rotation = targetRotation;
        if (PipeController.INSTANCE.levelCreatorMode)
        {
            pipe = PipeTypes.NOTHING;
            //Randomizer!!!!!!!!
            if (PipeController.INSTANCE.generateRandomEverything)
            {
                switch (Random.Range(0, 12))
                {
                    case 0:
                        pipe = PipeTypes.FOUR;
                        break;
                    case 1:
                        pipe = PipeTypes.NOTHING;
                        break;
                    case 2:
                        pipe = PipeTypes.THREE_DOWN;
                        break;
                    case 3:
                        pipe = PipeTypes.THREE_LEFT;
                        break;
                    case 4:
                        pipe = PipeTypes.THREE_RIGHT;
                        break;
                    case 5:
                        pipe = PipeTypes.THREE_UP;
                        break;
                    case 6:
                        pipe = PipeTypes.TWO_HORIZONTAL;
                        break;
                    case 7:
                        pipe = PipeTypes.TWO_VERTICAL;
                        break;
                    case 8:
                        pipe = PipeTypes.L_IZQ_ARRIBA;
                        break;
                    case 9:
                        pipe = PipeTypes.L_IZQ_ABAJO;
                        break;
                    case 10:
                        pipe = PipeTypes.L_DER_ARRIBA;
                        break;
                    case 11:
                        pipe = PipeTypes.L_DER_ABAJO;
                        break;
                }
                if (PipeController.INSTANCE.hasPositiveNegative)
                {
                    upPositive = Random.Range(1, 3);
                    rightPositive = Random.Range(1, 3);
                    leftPositive = Random.Range(1, 3);
                    downPositive = Random.Range(1, 3);
                }
                if (i == 0 && j == 0)
                {
                    PipeController.INSTANCE.StartPipe = this;
                    startPipe = true;
                }
            }
        }
        SetConnections();
        SetSprite();
        checkDelay = true;
        CheckActive(1);
        startSprite.SetActive(startPipe);
    }

    void SetConnections()
    {
        up = (pipe == PipeTypes.TWO_VERTICAL || pipe == PipeTypes.THREE_UP || pipe == PipeTypes.THREE_RIGHT || pipe == PipeTypes.THREE_LEFT || pipe == PipeTypes.FOUR || pipe == PipeTypes.L_DER_ARRIBA || pipe == PipeTypes.L_IZQ_ARRIBA);
        right = (pipe == PipeTypes.TWO_HORIZONTAL || pipe == PipeTypes.THREE_UP || pipe == PipeTypes.THREE_RIGHT || pipe == PipeTypes.THREE_DOWN || pipe == PipeTypes.FOUR || pipe == PipeTypes.L_DER_ARRIBA || pipe == PipeTypes.L_DER_ABAJO);
        down = (pipe == PipeTypes.TWO_VERTICAL || pipe == PipeTypes.THREE_DOWN || pipe == PipeTypes.THREE_RIGHT || pipe == PipeTypes.THREE_LEFT || pipe == PipeTypes.FOUR || pipe == PipeTypes.L_DER_ABAJO || pipe == PipeTypes.L_IZQ_ABAJO);
        left = (pipe == PipeTypes.TWO_HORIZONTAL || pipe == PipeTypes.THREE_UP || pipe == PipeTypes.THREE_DOWN || pipe == PipeTypes.THREE_LEFT || pipe == PipeTypes.FOUR || pipe == PipeTypes.L_IZQ_ARRIBA || pipe == PipeTypes.L_IZQ_ABAJO);
    }

    public void RotatePipe()
    {
        if (!PipeController.INSTANCE.levelFinished)
        {
            //testing

            for (int i = 0; i < PipeController.INSTANCE.height; i++)
            {
                for (int j = 0; j < PipeController.INSTANCE.width; j++)
                {
                    PipeController.INSTANCE.pipesBe[j, i].SetDefault();
                }
            }
            PipeController.INSTANCE.StartPipe.checkDelay = true;
            //till here

            transform.Rotate(new Vector3(0, 0, -1));
            actualRotIndex++;
            if (actualRotIndex == 4) actualRotIndex = 0;
            switch (actualRotIndex)
            {
                case 0:
                    targetRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                    break;
                case 1:
                    targetRotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
                    break;
                case 2:
                    targetRotation = Quaternion.LookRotation(Vector3.forward, -Vector3.up);
                    break;
                case 3:
                    targetRotation = Quaternion.LookRotation(Vector3.forward, -Vector3.right);
                    break;
            }

            bool aux = up;
            up = left;
            left = down;
            down = right;
            right = aux;

            int aux2 = upPositive;
            upPositive = leftPositive;
            leftPositive = downPositive;
            downPositive = rightPositive;
            rightPositive = aux2;

            anim.SetBool("isChanging", true);
        }
    }

    public void CheckActive(int order)
    {
        activationOrder = order;
        isConnected = false;
        //Debug.Log("Checked" + j + "  " + i);
        toCheck = false;

        if (startPipe) isConnected = true;

        if (left && j > 0)
        {
            if (PipeController.INSTANCE.pipesBe[j - 1, i].isConnected && PipeController.INSTANCE.pipesBe[j - 1, i].right)
            {
                if (!PipeController.INSTANCE.hasPositiveNegative)
                {
                    fillRotation = 0;
                    isConnected = true;
                }
                else if ((leftPositive == 1 && PipeController.INSTANCE.pipesBe[j - 1, i].rightPositive == 2) || (leftPositive == 2 && PipeController.INSTANCE.pipesBe[j - 1, i].rightPositive == 1))
                {
                    fillRotation = 0;
                    isConnected = true;
                }
            }
        }
        if (right && j < PipeController.INSTANCE.width - 1)
        {
            if (PipeController.INSTANCE.pipesBe[j + 1, i].isConnected && PipeController.INSTANCE.pipesBe[j + 1, i].left)
            {
                if (!PipeController.INSTANCE.hasPositiveNegative)
                {
                    fillRotation = 2;
                    isConnected = true;
                }
                else if ((rightPositive == 1 && PipeController.INSTANCE.pipesBe[j + 1, i].leftPositive == 2) || (rightPositive == 2 && PipeController.INSTANCE.pipesBe[j + 1, i].leftPositive == 1))
                {
                    fillRotation = 2;
                    isConnected = true;
                }
            }
        }
        if (up && i > 0)
        {
            if (PipeController.INSTANCE.pipesBe[j, i - 1].isConnected && PipeController.INSTANCE.pipesBe[j, i - 1].down)
            {
                if (!PipeController.INSTANCE.hasPositiveNegative)
                {
                    fillRotation = 3;
                    isConnected = true;
                }
                else if ((upPositive == 1 && PipeController.INSTANCE.pipesBe[j, i - 1].downPositive == 2) || (upPositive == 2 && PipeController.INSTANCE.pipesBe[j, i - 1].downPositive == 1))
                {
                    fillRotation = 3;
                    isConnected = true;
                }
            }
        }
        if (down && i < PipeController.INSTANCE.height - 1)
        {
            if (PipeController.INSTANCE.pipesBe[j, i + 1].isConnected && PipeController.INSTANCE.pipesBe[j, i + 1].up)
            {
                if (!PipeController.INSTANCE.hasPositiveNegative)
                {
                    fillRotation = 1;
                    isConnected = true;
                }
                else if ((downPositive == 1 && PipeController.INSTANCE.pipesBe[j, i + 1].upPositive == 2) || (downPositive == 2 && PipeController.INSTANCE.pipesBe[j, i + 1].upPositive == 1))
                {
                    fillRotation = 1;
                    isConnected = true;
                }
            }
        }
        
        onOff.SetActive(isConnected);

        if (left && j > 0 && isConnected)
        {
            if (PipeController.INSTANCE.pipesBe[j - 1, i].targetPipe) PipeController.INSTANCE.pipesBe[j - 1, i].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            if (PipeController.INSTANCE.pipesBe[j - 1, i].toCheck && PipeController.INSTANCE.pipesBe[j - 1, i].right) PipeController.INSTANCE.pipesBe[j - 1, i].CheckActive(order + 1);
        }
        if (right && j < PipeController.INSTANCE.width - 1 && isConnected)
        {
            if (PipeController.INSTANCE.pipesBe[j + 1, i].targetPipe) PipeController.INSTANCE.pipesBe[j + 1, i].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            if (PipeController.INSTANCE.pipesBe[j + 1, i].toCheck && PipeController.INSTANCE.pipesBe[j + 1, i].left) PipeController.INSTANCE.pipesBe[j + 1, i].CheckActive(order + 1);
        }
        if (up && i > 0 && isConnected)
        {
            if (PipeController.INSTANCE.pipesBe[j, i - 1].targetPipe) PipeController.INSTANCE.pipesBe[j, i - 1].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            if (PipeController.INSTANCE.pipesBe[j, i - 1].toCheck && PipeController.INSTANCE.pipesBe[j, i - 1].down) PipeController.INSTANCE.pipesBe[j, i - 1].CheckActive(order + 1);
        }
        if (down && i < PipeController.INSTANCE.height - 1 && isConnected)
        {
            if (PipeController.INSTANCE.pipesBe[j, i + 1].targetPipe) PipeController.INSTANCE.pipesBe[j, i + 1].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            if (PipeController.INSTANCE.pipesBe[j, i + 1].toCheck && PipeController.INSTANCE.pipesBe[j, i + 1].up) PipeController.INSTANCE.pipesBe[j, i + 1].CheckActive(order + 1);
        }
        
        StopAllCoroutines();
        if (isConnected)
        {
            //StartCoroutine(ActivateWithDelay(activationOrder));
            if (boost >= 0 && !targetPipe) PipeController.INSTANCE.connectedPoints++;
            PipeController.INSTANCE.connectedPoints += boost;
        }
        //else spriteT.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }

    IEnumerator ActivateWithDelay()
    {
        yield return new WaitForSeconds(0.3f + activationOrder * 0.14f * 2f);
        fillAnim.gameObject.transform.rotation = new Quaternion(0, 0, 0, 1);
        fillAnim.gameObject.transform.Rotate(new Vector3(0, 0, 90 * fillRotation));
        fillAnim.SetTrigger("Load");
        //spriteT.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
    }

    public void SetSprite()
    {
        //if (startPipe) PipeController.INSTANCE.StartPipe = this;

        //Sets sprite
        if (pipe == PipeTypes.THREE_RIGHT || pipe == PipeTypes.THREE_DOWN || pipe == PipeTypes.THREE_LEFT || pipe == PipeTypes.THREE_UP)
        {
            spriteT.GetComponent<SpriteRenderer>().sprite = spriteThree;
            bgT.GetComponent<SpriteRenderer>().sprite = bgThree;
        }
        if (pipe == PipeTypes.TWO_HORIZONTAL || pipe == PipeTypes.TWO_VERTICAL)
        {
            spriteT.GetComponent<SpriteRenderer>().sprite = spriteTwo;
            bgT.GetComponent<SpriteRenderer>().sprite = bgTwo;
        }
        if (pipe == PipeTypes.FOUR)
        {
            spriteT.GetComponent<SpriteRenderer>().sprite = spriteFour;
            bgT.GetComponent<SpriteRenderer>().sprite = bgFour;
        }
        if (pipe == PipeTypes.L_DER_ABAJO || pipe == PipeTypes.L_DER_ARRIBA || pipe == PipeTypes.L_IZQ_ABAJO || pipe == PipeTypes.L_IZQ_ARRIBA)
        {
            spriteT.GetComponent<SpriteRenderer>().sprite = spriteL;
            bgT.GetComponent<SpriteRenderer>().sprite = bgL;
        }
        if (pipe == PipeTypes.NOTHING)
        {
            spriteT.GetComponent<SpriteRenderer>().sprite = spriteNothing;
            bgT.GetComponent<SpriteRenderer>().sprite = null;
        }

        //Rotates sprite (so we dont have repeated images)
        bgT.localRotation = Quaternion.Euler(0, 0, 0);
        if (pipe == PipeTypes.THREE_RIGHT || pipe == PipeTypes.TWO_VERTICAL || pipe == PipeTypes.L_DER_ABAJO) bgT.Rotate(new Vector3(0, 0, -90));
        if (pipe == PipeTypes.THREE_DOWN || pipe == PipeTypes.L_IZQ_ABAJO) bgT.Rotate(new Vector3(0, 0, -180));
        if (pipe == PipeTypes.THREE_LEFT || pipe == PipeTypes.L_IZQ_ARRIBA) bgT.Rotate(new Vector3(0, 0, -270));

        /*
        if (boost == 0) background.sprite = BGNormal;
        if (boost > 0) background.sprite = BGBoosted;
        if (boost < 0) background.sprite = BGDebuffed;*/


        spup.sprite = (upPositive == 1) ? negative : positive;
        spdown.sprite = (downPositive == 1) ? negative : positive;
        spright.sprite = (rightPositive == 1) ? negative : positive;
        spleft.sprite = (leftPositive == 1) ? negative : positive;
        if (upPositive == 0) spup.gameObject.SetActive(false); else spup.gameObject.SetActive(true);
        if (rightPositive == 0) spright.gameObject.SetActive(false); else spright.gameObject.SetActive(true);
        if (leftPositive == 0) spleft.gameObject.SetActive(false); else spleft.gameObject.SetActive(true);
        if (downPositive == 0) spdown.gameObject.SetActive(false); else spdown.gameObject.SetActive(true);
    }

    public void SetCanRotateTrue()
    {
        canRotate = true;
    }

    public void SetDefault()
    {
        activationOrder = 0;
        toCheck = true;
        PipeController.INSTANCE.pipesBe[j, i].isConnected = false;
        PipeController.INSTANCE.pipesBe[j, i].toCheck = true;
        onOff.SetActive(false);
        //spriteT.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }

    void Update() {
        if (canRotate) t.rotation = Quaternion.RotateTowards(t.rotation, targetRotation, Time.deltaTime * 200);
        if (canRotate && t.rotation.eulerAngles == targetRotation.eulerAngles)
        {
            canRotate = false;
            anim.SetBool("isChanging", false);
        }
        if (checkDelay)
        {
            checkDelay = false;
            CheckActive(1);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetDefault();
            RotatePipe();
            if (startPipe) checkDelay = true;
        }
    }
}
