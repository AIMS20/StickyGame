using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject[] gamestateObjects;
    private GameObject[] menustateObjects;
    private GameObject[] optionstateObjects;
    private GameObject uiManager;
    private Transform canvas;
    public enum States{
        MENU, OPTIONS, GAME, QUIT
    }

    public States currentState;
    
    private void Awake()
    {
        SetState(0);
        
        uiManager = gameObject;
        
        
        gamestateObjects = GameObject.FindGameObjectsWithTag("GamestateObjects");
        menustateObjects = GameObject.FindGameObjectsWithTag("MenustateObjects");
        // optionstateObjects = GameObject.FindGameObjectsWithTag("OptionsObjects");
        
        canvas = uiManager.transform.Find("MainMenu");
        
        
        var playButton = GetButton("Play");
        // playButton.onClick.AddListener(PlayClick); //TODO:  integrate into GetButton

        var quitButton = GetButton("Quit");
        // quitButton.onClick.AddListener(QuitClick); //TODO:  integrate into GetButton
        
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
    }

    private void Start()
    {
    }
    

    private Button GetButton(string name)
    {
        var button = canvas.Find(name).GetComponent<Button>();

        return button;
    }

    public void Test(States state)
    {
        
    }
    
    //int parameter to bypass OnClick() restriction of passing enum and still optimize speed
    public void SetState(int state)
    {
        switch (state)
        {
            case 0:
                currentState = States.MENU;
                Time.timeScale = 0;
                break;
            
            case 1:
                currentState = States.OPTIONS;
                Time.timeScale = 0;
                break;       
            
            case 2:
                currentState = States.GAME;
                Time.timeScale = 1;
                break; 
            
            case 3:
                currentState = States.QUIT;
                print("clicked quit");
                Application.Quit();
                break;
                
        }
    }
    

 
}
