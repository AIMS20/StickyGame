using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject[] gamestateObjects;
    private GameObject[] menustateObjects;
    private GameObject uiManager;
    private Transform canvas;
    private enum States{
        MENU, GAME
    }

    private States currentState;
    
    private void Awake()
    {
        uiManager = gameObject;
        gamestateObjects = GameObject.FindGameObjectsWithTag("GamestateObjects");
        menustateObjects = GameObject.FindGameObjectsWithTag("MenustateObjects");
        canvas = uiManager.transform.Find("Canvas");
        var playButton = GetButton("Play");
        playButton.onClick.AddListener(PlayClick); //TODO:  integrate into GetButton

        var quitButton = GetButton("Quit");
        quitButton.onClick.AddListener(QuitClick); //TODO:  integrate into GetButton
    }

    private void QuitClick()
    {
        print("clicked quit");
        Application.Quit();
    }

    private Button GetButton(string name)
    {
        var button = canvas.Find(name).GetComponent<Button>();

        return button;
    }


    private void PlayClick()
    {
        // print("clicked play");
        SetState(States.GAME);
    }


    void Start()
    {
        SetState(States.MENU);
    }

    private void SetState(States state)
    {
        switch (state)
        {
            case States.MENU:
                
                Time.timeScale = 0;
                SwitchStates(menustateObjects, gamestateObjects);
                break;
            
            case States.GAME:
                Time.timeScale = 1;
                SwitchStates(gamestateObjects, menustateObjects);
                break; 

        }
    }

    private void SwitchStates(GameObject[] activate, GameObject[] deactivate)
    {
        foreach (var obj in deactivate)
        {
            obj.SetActive(false);
        }

        foreach (var obj in activate)
        {
            obj.SetActive(true);
        }
    }

 
}
