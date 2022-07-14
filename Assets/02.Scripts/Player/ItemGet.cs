using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;
using UnityEngine.UI;
public class ItemGet : MonoBehaviour
{
	public CanvasGroup pressF;
	public CanvasGroup dropUI;
	private bool dropUI_Opened = false;
	public Transform Content;
	GameObject[] item_buttons;
	public Item[] items;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("DROPITEM"))
		{
			pressF.alpha = 1f;
		}
	}
    public void Update()
    {
	}
    private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("DROPITEM"))
		{
			pressF.alpha = 0f;
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("DROPITEM"))
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				DropDisable();
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				if (!dropUI_Opened)
                {
                    OpenDrop(other);
                }
                else
				{
					DropDisable();
				}
			}

		}
	}

    private void OpenDrop(Collider other)
    {
		GameManager.instance.isPaused = true;
        items = other.GetComponent<DropItem>().items;
        item_buttons = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            if (items[i] != null)
            {
                item_buttons[i] = Instantiate(Resources.Load<GameObject>("Drop_Button_Item"), Content);
                item_buttons[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = items[i].name;
                item_buttons[i].GetComponent<ItemButton>().Item_index = i;
            }
        }
        dropUI.alpha = 1f;
        pressF.alpha = 0f;
        GameManager.instance.Pause(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        dropUI_Opened = true;
        dropUI.blocksRaycasts = true;
    }

    public void DropDisable()
	{
		GameManager.instance.isPaused = false;
		if (item_buttons != null)
			foreach (var x in item_buttons)
			{
				Destroy(x);
			}
		dropUI.alpha = 0f;
		pressF.alpha = 1f;
		GameManager.instance.Pause(false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		dropUI_Opened = false;
		dropUI.blocksRaycasts = false;
	}
}
