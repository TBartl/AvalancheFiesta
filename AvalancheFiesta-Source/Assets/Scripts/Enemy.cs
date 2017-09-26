using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public GameObject[] shootables;
	public GameObject mouth;
	public float rotationSpeed, lerpPower;
	public float shootPower;

	private float timeUntilShot;
	public float reloadTime;

	public GameObject[] players;


	private Quaternion targetRotation;

	void Start()
	{
		timeUntilShot = reloadTime;
		mouth = transform.Find("enemy/mouth").gameObject;
	}
	// Update is called once per frame
	void Update () {
		if (GameGen.gameStarted)
		{
			if (players.Length == 0)
			{
				players = GameObject.FindGameObjectsWithTag("Player");
			}
			transform.position = Vector3.Lerp (transform.position, new Vector3(0, Mathf.Max(3,GameGen.yReached-1.5f,0)), Time.deltaTime*lerpPower/6f);
			targetRotation = Quaternion.Euler(new Vector3(0,targetRotation.eulerAngles.y + Time.deltaTime*rotationSpeed,0));
			transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation, lerpPower*Time.deltaTime);

			
			timeUntilShot -= Time.deltaTime;
			if (timeUntilShot < 0)
			{
				timeUntilShot = reloadTime - GameGen.difficulty*10f;
				if (Network.isServer && players[0] != null)
				{
					GameObject temp = (GameObject)Network.Instantiate(shootables[(int)(Random.value*shootables.Length)], mouth.transform.position, Quaternion.identity, 4);
					temp.GetComponent<Rigidbody>().velocity = Vector3.Normalize(players[(int)(Random.value*players.Length)].transform.position - mouth.transform.position)*shootPower;
				}
			}
		}
	}
}
