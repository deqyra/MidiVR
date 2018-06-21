using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveHand : MonoBehaviour
{
    [Tooltip("The shader with which to replace highlighted objects shader.")]
    [SerializeField]
    /// <summary>
    ///     Any interactable object being hovered upon may have its shader temporarily replaced, to highlight it for example.
    /// </summary>
    private Shader replacementShader;

    [Tooltip("Which hand this controller belongs to.")]
    [SerializeField]
    /// <summary>
    ///     Left/right controller differentiation may be used for several purposes.
    /// </summary>
    private ControllerHand hand;

    [Tooltip("Which menu prefab to display upon grip press.")]
    [SerializeField]
    private MenuManager menu;

    /// <summary>
    ///     The inner tracked object cached reference, used to access the controller reference.
    /// </summary>
    private SteamVR_TrackedObject trackedObj;

    /// <summary>
    ///     Used to keep track of the previously focused colliding object.
    /// </summary>
    private MidiVRInteraction prevCollidingObject;
    
    /// <summary>
    ///     The object currently being collided with. Objects must be MidiVR-interactable to qualify for focus.
    /// </summary>
    private MidiVRInteraction collidingObject;

    /// <summary>
    ///     Prevents the controller from focusing on other objects when the currently focused one requires special attention.
    /// </summary>
    private bool isBusy = false;

    /// <summary>
    ///     If shaders must be temporarily replaced, they must be restored at one point. This is to keep track of the pre-replacement shader.
    /// </summary>
    private Shader originalShader;

    /// <summary>
    ///     Indicates whether the shader of the focused object was replaced, and thus if it needs being restored afterwards.
    /// </summary>
    private bool shaderReplaced;

    /// <summary>
    ///     The controller reference.
    /// </summary>
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    /// <summary>
    ///     Caching the tracked object reference.
    /// </summary>
    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Start()
    {
        HideMenu();
    }

    /// <summary>
    ///     Assigns the new colliding object if it qualifies for focus.
    /// </summary>
    private void SetCollidingObject(Collider col)
    {
        if (!collidingObject && col.GetComponent<MidiVRInteraction>())
        {
            prevCollidingObject = collidingObject;
            collidingObject = col.GetComponent<MidiVRInteraction>();
        }
    }

    /// <summary>
    ///     Event fired whenever object colliders cross the controller collider.
    /// </summary>
    public void OnTriggerEnter(Collider other)
    {
        //Prevent focus is the controller is busy with anther object.
        if (!isBusy)
        {
            SetCollidingObject(other);
        }

        //If the new collision-entered object was made the actual colliding object...
        if (collidingObject && collidingObject.gameObject == other.gameObject)
        {
            //If the trigger was just pressed...
            if (Controller.GetHairTriggerDown())
            {
                //Interact if possible
                if (Interactable())
                {
                    collidingObject.StartInteract(this);
                }
            }
            //Else, if the trigger was already being pressed...
            else if (Controller.GetHairTrigger())
            {
                //Interact if possible and not already the case
                if (Interactable() && !collidingObject.IsBeingInteracted())
                {
                    collidingObject.StartInteractDelayed(this);
                }
            }

            //Restore the shader of the previous colliding object if need be
            if (prevCollidingObject)
            {
                if (shaderReplaced)
                {
                    if (collidingObject.GetComponent<MeshRenderer>())
                    {
                        collidingObject.GetComponent<MeshRenderer>().material.shader = originalShader;
                    }
                }
            }

            //And also assign the replacement shader to the new colliding object if need be
            if (collidingObject.GetComponent<MeshRenderer>())
            {
                originalShader = collidingObject.GetComponent<MeshRenderer>().material.shader;
                collidingObject.GetComponent<MeshRenderer>().material.shader = replacementShader;
                shaderReplaced = true;
            }
            else
            {
                shaderReplaced = false;
            }
        }
    }

    /// <summary>
    ///     Keep refreshing the colliding object in order to avoid buggy ineraction.
    /// </summary>
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    /// <summary>
    ///     Event fired whenever an object collider stops crossing the controller collider.
    /// </summary>
    public void OnTriggerExit(Collider other)
    {
        //Care only if the collider being exited is the current colliding object
        if (collidingObject && collidingObject.gameObject == other.gameObject)
        {
            //Stop interacting with the object if needed
            if (Controller.GetHairTrigger())
            {
                collidingObject.StopInteract(this);
            }

            //Restore the original shader if applicable
            if (shaderReplaced)
            {
                if (collidingObject.GetComponent<MeshRenderer>())
                {
                    collidingObject.GetComponent<MeshRenderer>().material.shader = originalShader;
                }
            }

            prevCollidingObject = collidingObject;
            collidingObject = null;
        }
    }

    /// <summary>
    ///     Indicates whether interaction is immediately possible.
    /// </summary>
    private bool Interactable()
    {
        return collidingObject;
    }

    /// <summary>
    ///     Returns the velocity of the controller.
    /// </summary>
    public Vector3 Velocity()
    {
        return Controller.velocity;
    }

    /// <summary>
    ///     Returns the angular velocity of the controller.
    /// </summary>
    public Vector3 AngularVelocity()
    {
        return Controller.angularVelocity;
    }

    /// <summary>
    ///     Make the controller busy to prevent it from interacting with other objects.
    /// </summary>
    public void MakeBusy()
    {
        isBusy = true;
    }

    /// <summary>
    ///     Stop having the controller busy.
    /// </summary>
    public void UnmakeBusy()
    {
        isBusy = false;
    }

    /// <summary>
    ///     Indicates whether the controller is busy.
    /// </summary>
    public bool IsBusy()
    {
        return isBusy;
    }

    /// <summary>
    ///     Called once per frame.
    /// </summary>
    private void Update ()
    {
        //Interact with objects if needed.
        if (Controller.GetHairTriggerDown())
        {
            if (Interactable())
            {
                collidingObject.StartInteract(this);
            }
        }

        //Uninteract with objects if needed.
        if (Controller.GetHairTriggerUp())
        {
            if (Interactable() && collidingObject.IsBeingInteracted())
            {
                collidingObject.StopInteract(this);
            }
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            ShowMenu();
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            HideMenu();
        }
    }

    private void ShowMenu()
    {
        menu.gameObject.SetActive(true);
        MakeBusy();
    }

    private void HideMenu()
    {
        menu.gameObject.SetActive(false);
        UnmakeBusy();
    }
}
