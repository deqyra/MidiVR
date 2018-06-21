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
        float height = keyPrefab.GetComponent<MeshRenderer>().bounds.size.y; // height is 0.025 for the default key
        float depth  = keyPrefab.GetComponent<MeshRenderer>().bounds.size.z; // depth  is 0.100 for the default key
        float keyDistance = width * 1.3f;
        float firstKeyXPos = -(((nKeys-1) / 2) * keyDistance);

        // Retrieve dimensions of piano body
        Vector3 bodyScale = body.localScale;
        bodyScale.x = nKeys * keyDistance;
        body.transform.localScale = bodyScale;

        // Retrieve key rack
        GameObject keyRack = FindGameObjectInChildrenByName(gameObject, "Keys");

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

    // Used to retrieve an object's children bounding box bounds 
    Bounds GetMaxBounds(GameObject g)
    {
        var b = new Bounds(g.transform.position, Vector3.zero);
        foreach (Renderer r in g.GetComponentsInChildren<Renderer>())
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }

    GameObject FindGameObjectInChildrenByName(GameObject root, string name)
    {
        Transform[] ts = transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == name)
            {
                return t.gameObject;
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update () {

    }
}
