using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicLogic : MonoBehaviour
{
    public static CinematicLogic INSTANCE;

    public Transform holder;
    public Transform[] parts;
    public int progress = 0;

    void Start()
    {
        INSTANCE = this;

        parts = new Transform[holder.childCount];
        for (int i = 0; i < holder.childCount; i++)
        {
            parts[i] = holder.GetChild(i);
            if (i != 0)
                parts[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {/*
        if (!DontDestroy.INSTANCE.fadingIn)
        {
            bool somethingChanged = false;
            if (Input.GetKeyDown(KeyCode.Mouse0) && progress < parts.Length - 1)
            {
                wentRight(true);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1) && progress > 0)
            {
                wentRight(false);
            }
        }*/
    }

    public void wentRight(bool isRight)
    {
        if (!DontDestroy.INSTANCE.fadingIn)
        {
            if (isRight && progress < parts.Length - 1)
            {
                progress += 1;
                UpdateChange();
            }
            if (!isRight && progress > 0)
            {
                UpdateChange();
                progress -= 1;
            }
        }
    }

    void UpdateChange()
    {
        parts[progress].gameObject.SetActive(!parts[progress].gameObject.activeSelf);
    }
}
