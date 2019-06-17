using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    #region Input Paramater
    public bool BtnEnterDown    {get; protected set;}
    public bool BtnCanselDown   {get; protected set;}
    public bool BtnRightDown    {get; protected set;}
    public bool BtnLeftDown     {get; protected set;}
    public bool BtnUpDown       {get; protected set;}
    public bool BtnDownDown     {get; protected set;}
    #endregion

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        
    }

    private void Update()
    {
        BtnEnterDown  = Input.GetKeyDown(KeyCode.Return);
        BtnCanselDown = Input.GetKeyDown(KeyCode.Backspace);
        BtnRightDown  = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
        BtnLeftDown   = Input.GetKeyDown(KeyCode.LeftArrow)  || Input.GetKeyDown(KeyCode.A);
        BtnUpDown     = Input.GetKeyDown(KeyCode.UpArrow)    || Input.GetKeyDown(KeyCode.W);
        BtnDownDown   = Input.GetKeyDown(KeyCode.DownArrow)  || Input.GetKeyDown(KeyCode.S);
    }
}
