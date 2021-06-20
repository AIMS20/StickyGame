using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour, ISelectHandler
{
    private GameObject menuManager;
    private GameObject currentSelection;
    public Slider volumeSlider;
    public Slider difficultySlider;
    public GameObject poopSelector;
    public Vector3 poopOffset;
    
    public float rotateSpeed = 25f;

    [SerializeField] private PersistentSettings persistentSettings;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Material deselectedMat;
    [SerializeField] private Material selectedMat;

    
    public enum States{
        MENU, OPTIONS, GAME, QUIT
    }

    public States currentState;
    
    private void Awake()
    {
        menuManager = gameObject;
    }

    private void Start()
    {
        SetState(0);
        var mainMenu = menuManager.transform.Find("MainMenu");
        var optionsMenu = menuManager.transform.Find("OptionsMenu");
        
        optionsMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
        
        DontDestroyOnLoad(persistentSettings);
        
        //set starting difficulty in beginning
        PersistentSettings.UpdateDiff(1);
        
        //select first button
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Play"));
    }


    public void ChangeVolume()
    {
        print("changing volume to "+volumeSlider.value+"!");
        AudioListener.volume = volumeSlider.value;
        UpdateSliderCube(volumeSlider);
    }
    
    public void ChangeDifficulty()
    {
        print("changing difficulty to "+difficultySlider.value+"!");
        PersistentSettings.UpdateDiff((int)difficultySlider.value);
        UpdateSliderCube(difficultySlider);
    }

    //scales the cube under the specific slider depending on it's value
    private void UpdateSliderCube(Slider slider)
    {
        var sliderCubeScale = currentSelection.transform.GetChild(1).transform.localScale;
        currentSelection.transform.GetChild(1).transform.localScale = new Vector3(
            sliderCubeScale.x,
            sliderCubeScale.y,
            Remap(slider.value, slider.minValue, slider.maxValue, 0.25f, 60f));
    }

    //remaps incoming values of slider to adjust for SliderCubeScale-width-transformation
    private float Remap(float input, float oldLow, float oldHigh, float newLow, float newHigh)
    {
        float t = Mathf.InverseLerp(oldLow, oldHigh, input);
        return Mathf.Lerp(newLow, newHigh, t);
    }
    
    //int parameter to bypass OnClick() restriction of passing enum and still optimize speed
    //TODO: outsource
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
                break;       
            
            case 2:
                currentState = States.GAME;
                Time.timeScale = 1;
                SceneManager.LoadScene("Gameplay");

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
        if (currentSelection != null)
        {
            UpdatePoopSelector();

            UpdateSelectionAnim();
        }
    }
    
    //updates the animation of selected button/slider
    private void UpdateSelectionAnim()
    {
        currentSelection.transform.Rotate(new Vector3(1, 0, 1), Mathf.Sin(Time.unscaledTime) * rotateSpeed);
    }

    //updates the 3D poop icon next to buttons
    private void UpdatePoopSelector()
    {
        var poopTarget = currentSelection.transform.Find("PoopTarget").position;
        poopOffset = new Vector3(poopTarget.x, poopTarget.y-35f, poopTarget.z);
        // var newPoopPos = new Vector3(currButtonTransform.rect., currentSelection.transform.position.y + poopOffset.y, currentSelection.transform.position.z);
        poopSelector.transform.Rotate(new Vector3(0, 1, 0), rotateSpeed*250f, Space.World);
        poopSelector.transform.position = Vector3.MoveTowards(poopSelector.transform.position, poopOffset, 125f);
    }

    //change color to red on select
    public void OnSelect(BaseEventData eventData){
        currentSelection = eventData.selectedObject;

        var parent = currentSelection.transform.GetChild(0).Find("Text");
        foreach (Transform child in parent){
            child.GetComponent<Renderer>().material = selectedMat;
        }

        // print("change selection to " + eventData.selectedObject);
    }
    
    //change color on deselect
    public void OnDeselect(BaseEventData eventData){ 
        var parent = currentSelection.transform.GetChild(0).Find("Text");
        foreach (Transform child in parent){
            child.GetComponent<Renderer>().material = deselectedMat;
        }
        
        // print("DEselection from " + eventData.selectedObject);
    }
}
