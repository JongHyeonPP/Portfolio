using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    public static GameManager instance;
    private CanvasGroup inventory;
    private GameObject button_inventory;
    public GameDataObject gameDataObject;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Cursor.visible=false;
        Cursor.lockState = CursorLockMode.Locked;
        button_inventory = GameObject.Find("Button-Inventory");
        inventory = GameObject.Find("Panel-Inventory").GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Inventory(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Inventory(false);
        }
    }
    public void Pause(bool stop)
    {
        Time.timeScale = (stop) ? 0.0f : 1.0f;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        var scripts = playerObj.GetComponents<MonoBehaviour>();

        foreach (var script in scripts)
        {
            script.enabled = !stop;
        }
        Camera.main.GetComponent<ThirdPersonOrbitCamBasic>().enabled = !stop;
    }
    public void Inventory(bool stop)
    {
        if (stop)
        {
            inventory.alpha = 1f;
            
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            inventory.alpha = 0f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        button_inventory.SetActive(!stop);
        Cursor.visible = stop;
        Pause(stop);
        inventory.interactable = stop;
    }
    public void OpenDrop(bool stop)
    {
        if (stop)
        {
            inventory.alpha = 1f;

            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            inventory.alpha = 0f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        button_inventory.SetActive(!stop);
        Cursor.visible = stop;
        inventory.interactable = stop;
    }
}
