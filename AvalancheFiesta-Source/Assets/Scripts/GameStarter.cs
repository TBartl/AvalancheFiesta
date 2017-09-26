using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour {

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			GameGen.gameStarted = true;
		}
	}
}
