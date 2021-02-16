using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chat", menuName = "NewChat")]
public class DialogPart : ScriptableObject
{
    [TextArea]
    public string[] texts;
    public DialogPart.Personaje[] characters;
    public bool[] yesNo;
    public bool[] isEnder;

    public enum Personaje
    {
        PJ,
        ING_ELECTRONICA
    }
}
