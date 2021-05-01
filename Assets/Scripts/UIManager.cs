using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject[] gamestateObjects;
    private GameObject[] menustateObjects;
    private GameObject uiManager;
    private Transform canvas;

    private void Awake()
    {
        uiManager = gameObject;
        gamestateObjects = GameObject.FindGameObjectsWithTag("GamestateObjects");
        menustateObjects = GameObject.FindGameObjectsWithTag("MenustateObjects");
        canvas = uiManager.transform.Find("Canvas");
        var playButton = GetButton("Play");
        playButton.onClick.AddListener(PlayClick); //TODO: somehow integrate into GetButton
        
        
        var quitButton = GetButton("Quit");
        quitButton.onClick.AddListener(QuitClick); //TODO: somehow integrate into GetButton
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
        print("clicked play");
        SetState("Game");
    }


    void Start()
    {
        SetState("Menu");
    }

    private void SetState(string state)
    {
        switch (state)
        {
            case "Menu":
                
                Time.timeScale = 0;
                foreach (var obj in gamestateObjects){
                    obj.SetActive(false);
                }
                foreach (var obj in menustateObjects){
                    obj.SetActive(true);
                }
                break;
            
            case "Game":
                Time.timeScale = 1;
                foreach (var obj in gamestateObjects){
                    obj.SetActive(true);
                }
                foreach (var obj in menustateObjects){
                    obj.SetActive(false);
                }
                break; 

        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
