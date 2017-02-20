using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneState : MonoBehaviour {
    private int shelvedItemCount = 0;
    private int unloadedItemCount = 0;
    private Dictionary<string, Vector3> savedLocations = new Dictionary<string, Vector3>();
    private Dictionary<string, Quaternion> savedRotations = new Dictionary<string, Quaternion>();
    public ShelfController[] shelfAreas = new ShelfController[5];
    public ShelfController[] unloadAreas = new ShelfController[0];
    public float ambientIntensity = 1.5f;

    public int ShelvedItemCount
    {
        get { return this.shelvedItemCount; }
    }

    public int UnloadedItemCount
    {
        get { return this.unloadedItemCount; }
    }

    // Use this for initialization
    void Start()
    {
        SavePositionOfEverything();
    }

    public bool IsObjectiveMet()
    {
        shelvedItemCount = 0;
        
        foreach (ShelfController sc in shelfAreas)
        {
            shelvedItemCount += sc.shelvedItemCount;
        }

        unloadedItemCount = 0;
        foreach(ShelfController sc in unloadAreas)
        {
            unloadedItemCount += sc.shelvedItemCount;
        }

        return 
            (shelfAreas.GetUpperBound(0) + 1 <= shelvedItemCount) 
            && (unloadedItemCount == 0);
    }

    public void RestorePositionOfEverything()
    {
        foreach(string keyname in savedLocations.Keys)
        {
            Debug.Log(string.Format("  restorable item name: {0}, location:{1}, rotation:{2}", keyname, savedLocations[keyname].ToString(), savedRotations[keyname].ToString()));
        }
    }

    List<string> thingsToSave = new List<string>() { "Racks", "Pallets", "Vehicles" };

    private void SavePositionOfEverything()
    {
        savedLocations.Clear();
        savedRotations.Clear();
        foreach (GameObject foo in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach(Transform t in foo.transform)
            {
                RememberThingIfAppropriate(t);
            }
        }
    }

    private void RememberThingIfAppropriate(Transform t)
    {

        if (thingsToSave.Contains(t.name))
        {
            foreach (Transform childOfT in t.transform)
            {
                Vector3 tempLocation = childOfT.transform.position;
                Quaternion tempRotation = childOfT.transform.rotation;
                string keyname = t.name + ":" + childOfT.name;
                savedLocations.Add(keyname, tempLocation);
                savedRotations.Add(keyname, tempRotation);
            }
        }
    }
}
