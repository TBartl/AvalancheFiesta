using UnityEngine;
using System.Collections;

public class DestroyOnTime : MonoBehaviour {
	public float time;
	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, time);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
