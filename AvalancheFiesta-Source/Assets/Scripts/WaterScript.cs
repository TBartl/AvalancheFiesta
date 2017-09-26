using UnityEngine;
using System.Collections;

public class WaterScript : MonoBehaviour {
	public float lerpPower;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameGen.gameStarted)
		{
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, Color.red, lerpPower*Time.deltaTime);
			transform.parent.localScale = Vector3.Lerp(transform.parent.localScale,new Vector3(1,Mathf.Max(.5f,GameGen.yReached-3), 1), lerpPower*Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player")
		{
			Player p = c.gameObject.GetComponent<Player>();
			p.LoseLife();
		}
	}
}
