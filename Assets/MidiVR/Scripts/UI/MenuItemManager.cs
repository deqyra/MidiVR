using System;
using UnityEngine;

class MenuItemManager : MonoBehaviour
{
    [Tooltip("Reference to the text element to write to.")]
    [SerializeField]
    private UnityEngine.UI.Text textCanvas;

    [Tooltip("Reference to the menu this item is part of.")]
    [SerializeField]
    private MenuManager menu;

    [Tooltip("Reference to the visible body of the button.")]
    [SerializeField]
    private MeshRenderer body;

    [Tooltip("Reference to the press button attached to this item.")]
    [SerializeField]
    private PressInteraction pressButton;

    private int index;

    private bool errorDisplayed = false;

    private void Update()
    {
        if (pressButton.IsPressed())
        {
            if (!pressButton.IsActioned())
            {
                if (menu != null)
                {
                    menu.InstantiateObject(index);
                    pressButton.ActionPerformed();
                }
                else if (!errorDisplayed)
                {
                    Debug.Log("MenuItemManager: Menu is null!");
                    errorDisplayed = true;
                }
            }
        }
    }

    public void SetText(string text)
    {
        textCanvas.text = text;
    }

    public int GetIndex()
    {
        return index;
    }

    public void SetIndex(int i)
    {
        index = i;
    }

    public void SetMenu(MenuManager m)
    {
        menu = m;
    }

    public MeshRenderer GetBodyMeshRenderer()
    {
        return body;
    }
}
