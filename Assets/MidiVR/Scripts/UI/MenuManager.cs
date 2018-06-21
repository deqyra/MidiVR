using System;
using UnityEngine;

[Serializable]
public struct MenuObjectMap
{
    public string name;
    public GameObject objectPrefab;
    public Transform parentTransform;
}

class MenuManager : MonoBehaviour
{
    [Tooltip("Whether the instantion of an object destroys the previously instantiated object.")]
    [SerializeField]
    private bool managesUniqueCollection;

    [Tooltip("Whether the menu should automatically instantiate a first element.")]
    [SerializeField]
    private bool instantiateFirst;

    [Tooltip("Reference to the Menu Item prefab that this menu will be made of.")]
    [SerializeField]
    private MenuItemManager menuItemPrefab;

    [Tooltip("Which prefabs to instantiate upon pressing buttons.")]
    [SerializeField]
    private MenuObjectMap[] prefabs;

    private int currentIndex = -1;

    GameObject currentInstance = null;

    private void Start()
    {
        float height = menuItemPrefab.GetBodyMeshRenderer().bounds.size.y;
        float firstElementYPos = 0;
        float elementDistance = height * 1.5f;

        int index = 0;
        foreach (MenuObjectMap m in prefabs)
        {
            MenuItemManager instance = Instantiate(menuItemPrefab);
            instance.transform.SetParent(transform, false);
            instance.transform.localPosition = new Vector3(0, firstElementYPos + (index * elementDistance), 0);
            instance.SetIndex(index);
            instance.SetText(m.name);
            instance.SetMenu(this);

            index++;
        }

        if (index > 0)
        {
            InstantiateObject(0);
        }
    }

    public void InstantiateObject(int i)
    {
        if (managesUniqueCollection && i != currentIndex)
        {
            Destroy(currentInstance);
        }
        if (!managesUniqueCollection || i != currentIndex)
        {
            currentInstance = Instantiate(prefabs[i].objectPrefab);
            currentInstance.transform.SetParent(prefabs[i].parentTransform, false);
            currentIndex = i;
        }
    }
}
