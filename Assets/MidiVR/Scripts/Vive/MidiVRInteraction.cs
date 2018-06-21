using System;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(BoxCollider))]
public abstract class MidiVRInteraction : MonoBehaviour
{
    protected InteractionType interactionType;

    protected bool interacted = false;

    public abstract void StartInteract(ViveHand hand);
    public abstract void StartInteractDelayed(ViveHand hand);
    public abstract void StopInteract(ViveHand hand);

    public bool IsBeingInteracted()
    {
        return interacted;
    }

    public InteractionType Type()
    {
        return interactionType;
    }
}
