using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfoAction", menuName = "ScriptableObjects/InfoAction")]
public class InfoAction : ScriptableObject
{
    public bool used = false;
    public int ind;
    public int number;
    public ProgrammingMain.actionName aName;
    public Sprite image;
}