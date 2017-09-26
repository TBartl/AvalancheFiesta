using UnityEngine;
using System.Collections;

public class GroundDetection : MonoBehaviour {
	private Player p;
	
	void Start () {
		p = transform.parent.gameObject.GetComponent<Player>();
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Terrain")
		{
			p.grounded = true;
		}
	}
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Terrain")
		{
			p.grounded = true;
		}
	}
}
