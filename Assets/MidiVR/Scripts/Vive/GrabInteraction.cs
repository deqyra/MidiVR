using System;
using UnityEngine;

[Serializable]
public class GrabInteraction : MidiVRInteraction
{
    [SerializeField]
    private Rigidbody objectToGrab;

    private FixedJoint fj;
    private bool prevKinematicValue;

    public GameObject ObjectToGrab()
    {
        return objectToGrab.gameObject;
    }

    void Start()
    {
        interactionType = InteractionType.Grab;

        if (objectToGrab == null)
        {
            if (GetComponent<Rigidbody>())
            {
                objectToGrab = GetComponent<Rigidbody>();
            }
        }
    }

    public override void StartInteract(ViveHand hand)
    {
        if (objectToGrab)
        {
            fj = hand.gameObject.AddComponent<FixedJoint>();
            fj.breakForce = 20000;
            fj.breakTorque = 20000;
            fj.connectedBody = objectToGrab;

            prevKinematicValue = objectToGrab.isKinematic;
            objectToGrab.isKinematic = false;

            interacted = true;

            hand.MakeBusy();
        }
    }

    public override void StartInteractDelayed(ViveHand hand)
    {

    }

    public override void StopInteract(ViveHand hand)
    {
        if (fj = hand.GetComponent<FixedJoint>())
        {
            objectToGrab.isKinematic = prevKinematicValue;
            if (objectToGrab.useGravity)
            {
                objectToGrab.velocity = hand.Velocity();
                objectToGrab.angularVelocity = hand.AngularVelocity();
            }
            fj.connectedBody = null;
            Destroy(fj);

        }
        interacted = false;
        hand.UnmakeBusy();
    }
}
