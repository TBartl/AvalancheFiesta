using UnityEngine;
using System.Collections;

public class RemoveOnLow : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < GameGen.yReached-7)
		{
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			if (transform.position.y < GameGen.yReached-13)
			{
				Destroy(this.gameObject);
			}

		}
	
	}
}
