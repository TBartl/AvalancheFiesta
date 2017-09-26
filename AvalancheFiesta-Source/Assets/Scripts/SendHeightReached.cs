using UnityEngine;
using System.Collections;

public class SendHeightReached : MonoBehaviour {

	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.tag == "Terrain")
		{
			if (GetComponent<Rigidbody>().velocity.y > 0)
			{
				if (transform.position.y > GameGen.yReached && transform.position.y - GameGen.yReached < 5f)
					GameGen.yReached = transform.position.y;
				Destroy(this);
			}
		}
	}
}
