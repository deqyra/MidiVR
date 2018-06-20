using System;
using UnityEngine;

[RequireComponent(typeof(MidiVRInteraction))]
[RequireComponent(typeof(Rigidbody))]
[Serializable]
public class GrabInteraction : MonoBehaviour
{
    [SerializeField]
    private Rigidbody objectToGrab;

    public GameObject ObjectToGrab()
    {
        return objectToGrab.gameObject;
    }

    void Start()
    {
        if (objectToGrab == null)
        {
            objectToGrab = GetComponent<Rigidbody>();
        }
    }
}
