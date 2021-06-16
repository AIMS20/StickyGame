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
    public Slider volumeSlider;
    public GameObject gameManager;
    private Button playButton;
    private Transform canvas;
    public GameObject poopSelector;
    public float rotateSpeed = 25f;

    [SerializeField] private EventSystem eventSystem;

    
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
    }

    private Button GetButton(string name)
    {
        var button = canvas.Find(name).GetComponent<Button>();

        return button;
    }

    public void ChangeVolume()
    {
        print("changing volume to "+volumeSlider.value+"!");
        AudioListener.volume = volumeSlider.value;

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

    void Update()
    {
        
        //TODO: disable mouse or always enable one selection
        if (currentSelection != null){
            // print(currentSelection.name);
            
            // print(currentSelection.GetComponent<Graphic>().canvasRenderer.GetColor());
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

    public void OnSelect(BaseEventData eventData){
            currentSelection = eventData.selectedObject;
            currentSelection.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().sharedMaterial.color = Color.red;

            print("change selection to " + eventData.selectedObject);
    }
    public void OnDeselect(BaseEventData eventData){ 
            eventData.selectedObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().sharedMaterial.color = Color.white;
  
        
            print("change selection to " + eventData.selectedObject);
    }
}
