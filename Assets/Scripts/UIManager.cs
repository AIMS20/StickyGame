using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ISelectHandler
{
    private GameObject uiManager;
    private Button playButton;
    private Transform canvas;
    private Camera mainCam;
    private Camera menuCam;
    public GameObject poopSelector;
    public float rotateSpeed = 25f;

    public Vector3 poopOffset;
    // private Button[] menuButtons;
    private GameObject currentSelection;
    
    public enum States{
        MENU, OPTIONS, GAME, QUIT
    }

    public States currentState;
    
    private void Awake()
    {
        uiManager = gameObject;
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        menuCam = GameObject.FindWithTag("MenuCamera").GetComponent<Camera>();
        
        canvas = uiManager.transform.Find("MainMenu");
        
        
        playButton = GetButton("Play");
        // playButton.onClick.AddListener(PlayClick); //TODO:  integrate into GetButton

        var quitButton = GetButton("Quit");
        // quitButton.onClick.AddListener(QuitClick); //TODO:  integrate into GetButton
        
    }

    private void Start()
    {
        SetState(0);
        
        //select first button
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        //spawn selection poop
        Instantiate(poopSelector, Vector3.zero, Quaternion.identity);
    }

    private Button GetButton(string name)
    {
        var button = canvas.Find(name).GetComponent<Button>();

        return button;
    }

    public void ChangeVolume()
    {
        print("changing volume!");
    }
    
    //int parameter to bypass OnClick() restriction of passing enum and still optimize speed
    public void SetState(int state)
    {
        switch (state)
        {
            case 0:
                currentState = States.MENU;
                Time.timeScale = 0;
                mainCam.enabled = false;
                menuCam.enabled = true;
                break;
            
            case 1:
                currentState = States.OPTIONS;
                Time.timeScale = 0;
                break;       
            
            case 2:
                currentState = States.GAME;
                Time.timeScale = 1;
                menuCam.enabled = false;
                mainCam.enabled = true;
                break; 
            
            case 3:
                currentState = States.QUIT;
                print("clicked quit");
                Application.Quit();
                break;
                
        }
    }

    void Update()
    {
        currentSelection = EventSystem.current.currentSelectedGameObject;
        print(currentSelection.name);
        
        //TODO: disable mouse or always enable one selection
        if (currentSelection != null){
            UpdatePoopSelector();
            
            currentSelection.transform.Rotate(new Vector3(1, 0, 1), Mathf.Sin(Time.unscaledTime)*rotateSpeed);
        }
    }

    private void UpdatePoopSelector()
    {

        poopOffset.y = currentSelection.transform.position.y-25f;
        poopSelector.transform.Rotate(new Vector3(0, 0, 1), rotateSpeed);
        poopSelector.transform.position = Vector3.MoveTowards(poopSelector.transform.position, poopOffset, 100f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        throw new NotImplementedException();
    }
}
