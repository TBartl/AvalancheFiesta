using UnityEngine;
using System.Collections;

public class GameGen : MonoBehaviour {
	public string typeName;
	public string gameName;
	private HostData[] hostList;
	public GameObject playerPrefab;
	public Vector3 spawnPoint;


	public static bool gameStarted;
	public static float yReached;
	private float timeToNextSpawn;
	public float minSpawnTime;
	public float maxSpawnTimeAddition;
	public GameObject[] fallingObjects;
	public Gradient colors;
	public Vector2 range;
	public float maxScale;
	public static float difficulty;
	// Use this for initialization
	void Start () {
		yReached = -.5f;
		timeToNextSpawn = minSpawnTime;
		difficulty = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameStarted && Network.isServer)
		{
			difficulty += Time.deltaTime/200;
			transform.position = new Vector3(0, yReached+20, 0);
			timeToNextSpawn -= Time.deltaTime;
			if (timeToNextSpawn <= 0)
			{
				timeToNextSpawn = minSpawnTime +Random.value*maxSpawnTimeAddition- difficulty;
				GameObject temp = (GameObject)Network.Instantiate(fallingObjects[(int)(Random.value*fallingObjects.Length)], 
				            new Vector3((Random.value-.5f)*range.x, transform.position.y, (Random.value-.5f)*range.y),
				            Quaternion.Euler(new Vector3(360*Random.value,360*Random.value,360*Random.value)), 2);
				if (temp.name != "gold(Clone)" && temp.name != "BowtiePower(Clone)" && temp.name != "MoustachePower(Clone)")
				{
					temp.transform.localScale = Vector3.one*(Mathf.Sqrt(1+Random.value*maxScale));
					temp.GetComponent<Renderer>().material.color = colors.Evaluate(Random.value);
				}
				
			}
		}


	}

	

	void OnGUI()
	{
		if (!gameStarted && !Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
			{
				Network.InitializeServer(4, 7777, !Network.HavePublicAddress());
				MasterServer.RegisterHost(typeName,gameName);
			}
			if (GUI.Button(new Rect(100, 250, 250, 100), "Find Servers"))
				MasterServer.RequestHostList(typeName);
			if (hostList != null)
			{
				for (int index = 0; index < hostList.Length; index += 1)
				{
					if (GUI.Button(new Rect(400,100+ (110*index), 250, 100), gameName))
						Network.Connect(hostList[index]);
				}
			}
				
		}
		if (yReached < -100)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Restart Game?"))
			{
				Application.LoadLevel(0);
				Network.Disconnect();
			}
		}
	}
	void OnServerInitialized()
	{
		SpawnPlayer();
	}
	void OnConnectedToServer()
	{
		SpawnPlayer();
	}

	void SpawnPlayer()
	{
		Network.Instantiate(playerPrefab, spawnPoint, Quaternion.identity, 0);
	}
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent ==  MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}
}



