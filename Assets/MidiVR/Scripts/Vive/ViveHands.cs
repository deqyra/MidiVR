using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveHands : MonoBehaviour
{
    [Tooltip("The shader with which to replace highlighted objects shader.")]
    [SerializeField]
    private Shader replacementShader;

    [Tooltip("Which hand this controller belongs to.")]
    [SerializeField]
    private ControllerHand hand;

    private SteamVR_TrackedObject trackedObj;

    private GameObject prevCollidingObject;
    private GameObject collidingObject;
    private GameObject objectInHand;
    private bool prevKinematicValue;


    private Shader originalShader;
    private bool shaderReplaced;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
    }

    private void SetCollidingObject(Collider col)
    {
        if (!collidingObject && col.GetComponent<MidiVRInteraction>())
        {
            prevCollidingObject = collidingObject;
            collidingObject = col.gameObject;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
        
        //If the new collision-entered object was made the actual colliding object...
        if (collidingObject == other.gameObject)
        {
            //If the trigger was just pressed...
            if (Controller.GetHairTriggerDown())
            {
                //Press it if possible
                if (CanPress())
                {
                    Press();
                }
            }
            else if (Controller.GetHairTrigger())
            {
                if (CanPress() && !Pressed())
                {
                    Press();
                }
            }

            //Restore the shader of the previous colliding object if need be
            if (prevCollidingObject)
            {
                if (shaderReplaced)
                {
                    if (prevCollidingObject.GetComponent<MeshRenderer>())
                    {
                        prevCollidingObject.GetComponent<MeshRenderer>().material.shader = originalShader;
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

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);

        //originalShader = other.GetComponent<MeshRenderer>().material.shader;
        //other.GetComponent<MeshRenderer>().material.shader = replacementShader;
    }

    public void OnTriggerExit(Collider other)
    {
        //Care only if the trigger collision box exits currently colliding object
        if (collidingObject == other.gameObject)
        {
            //Unpress the object if needed AND applicable
            if (Controller.GetHairTriggerDown() && CanPress())
            {
                Unpress();
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

    bool CanGrab()
    {
        return collidingObject && collidingObject.GetComponent<MidiVRInteraction>().interactionType == InteractionType.Grab;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject.GetComponent<GrabInteraction>().ObjectToGrab();
        //collidingObject = null;

        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        fx.connectedBody = objectInHand.GetComponent<Rigidbody>();

        prevKinematicValue = objectInHand.GetComponent<Rigidbody>().isKinematic;
        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            objectInHand.GetComponent<Rigidbody>().isKinematic = prevKinematicValue;
            if (objectInHand.GetComponent<Rigidbody>().useGravity)
            {
                objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
                objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
            }
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
        }

        objectInHand = null;
    }

    bool CanPress()
    {
        return collidingObject && collidingObject.GetComponent<MidiVRInteraction>().interactionType == InteractionType.Press;
    }

    void Press()
    {
        collidingObject.GetComponent<PressInteraction>().Press();
    }

    bool Pressed()
    {
        return collidingObject.GetComponent<PressInteraction>().IsPressed();
    }

    void Unpress()
    {
        collidingObject.GetComponent<PressInteraction>().Unpress();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Controller.GetHairTriggerDown())
        {
            if (CanGrab())
            {
                GrabObject();
            }
            else if (CanPress())
            {
                Press();
            }
        }

        if (Controller.GetHairTriggerUp())
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
            if (CanPress())
            {
                collidingObject.GetComponent<PressInteraction>().Unpress();
            }
        }
    }
}
