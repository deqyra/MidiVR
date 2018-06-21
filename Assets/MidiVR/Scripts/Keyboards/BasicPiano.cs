using MIDI;
using UnityEngine;

public class BasicPiano : MonoBehaviour
{
    [Tooltip("The number of keys the keyboard should have.")]
    [Range(1f,24f)]
    [SerializeField]
    private int nKeys = 24;

    [Tooltip("The prefab to instantiate for keys.")]
    [SerializeField]
    private BasicPianoKey keyPrefab;

    [Tooltip("The note played by the first key on the Keyboard.")]
    [SerializeField]
    private Note startingNote;

    [Tooltip("The transform containing the body of the piano. Used to scale it according to the number of keys.")]
    [SerializeField]
    private Transform body;

    [Tooltip("The transform to contain the keys. It must be separate from the body transform because its scale must stay at 1.")]
    [SerializeField]
    private Transform keyRack;

    // Use this for initialization
    void Start ()
    {
        // Retrieve dimensions of keys
        float width  = keyPrefab.GetComponent<MeshRenderer>().bounds.size.x; // width  is 0.050 for the default key
        float keyDistance = width * 1.3f;
        float firstKeyXPos = -(((nKeys-1) / 2f) * keyDistance);

        // Retrieve dimensions of piano body
        Vector3 bodyScale = body.localScale;
        bodyScale.x = (nKeys + 0.5f) * keyDistance;
        body.transform.localScale = bodyScale;

        if (keyRack != null)
        {
            // Instantiate and set position and note of all keys in the key rack
            for (int i = 0; i < nKeys; i++)
            {
                // Key instantiation
                BasicPianoKey keyInstance = Instantiate(keyPrefab);
                // Setting the base note by retrieving the behaviour script
                keyInstance.note = startingNote.PitchUp((byte) i);

                // Putting it in place
                keyInstance.transform.SetParent(keyRack.transform, false);
                keyInstance.transform.localPosition = new Vector3(firstKeyXPos + (i * keyDistance), 0, 0);
            }
        }
    }
}
