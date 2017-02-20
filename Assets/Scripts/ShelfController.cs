using System.Collections.Generic;
using UnityEngine;

public class ShelfController : MonoBehaviour {

    private List<string> countShelvedTypes = new List<string>{ "Package" };
    public int shelvedItemCount;

	// Use this for initialization
	void Start () {
        shelvedItemCount = 0;
	}

    public void OnTriggerEnter(Collider other)
    {
        if (countShelvedTypes.Contains(other.name))
        {
            this.shelvedItemCount++;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (countShelvedTypes.Contains(other.name))
        {
            this.shelvedItemCount--;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (this.shelvedItemCount == 0)
        {
            if (countShelvedTypes.Contains(other.name))
            {
                this.shelvedItemCount--;
            }
        }
    }
}
