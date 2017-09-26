using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float cameraPower, lerpPower, moveSpeed, gravity, jumpPower;
	public bool grounded;
	public bool lastGrounded;
	public Transform cameraHolder;

	private Quaternion targetRotation;
	private CharacterController charControl;
	private float yVelocity;
	public GameObject[] walls;
	public Animator anim;

	public float money;
	public float lives;
	public float moustache;
	public float bowTie;

	private GUIText moneyText, livesText,finalScore, gameOver;
	private GUITexture moneyTexture, livesTexture, moustacheTexture, bowtieTexture;
	// Use this for initialization
	void Start () {
		targetRotation = Quaternion.identity;
		charControl = transform.gameObject.GetComponent<CharacterController>();
		yVelocity = 0;
		cameraHolder = Camera.main.transform.parent;
		for (int index = 0; index < 4; index += 1)
		{
			walls[index] = GameObject.Find("Walls/Wall"+index.ToString());
		}
		anim = GetComponent<Animator>();
		moneyText = GameObject.Find("MoneyText").GetComponent<GUIText>();
		livesText = GameObject.Find("LivesText").GetComponent<GUIText>();
		finalScore = GameObject.Find("FinalScore").GetComponent<GUIText>();
		gameOver = GameObject.Find("GameOver").GetComponent<GUIText>();
		moneyTexture = GameObject.Find("Money").GetComponent<GUITexture>();
		livesTexture = GameObject.Find("Lives").GetComponent<GUITexture>();
		moustacheTexture = GameObject.Find("Moustache").GetComponent<GUITexture>();
		bowtieTexture = GameObject.Find("Bowtie").GetComponent<GUITexture>();
		

	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<NetworkView>().isMine)
		{
			if (GameGen.gameStarted == true)
			{
				if ((bowTie>.05))
					money += (1 + GameGen.difficulty*10)*Time.deltaTime*5f;
				else
					money += (1 + GameGen.difficulty*10)*Time.deltaTime;
				moneyText.enabled = true;
				moneyText.text = ((int)money).ToString();
				livesText.enabled = true;
				livesText.text = ((int)lives).ToString();
				moneyTexture.enabled = true;
				livesTexture.enabled = true;
				moustache = Mathf.Max(0, moustache-Time.deltaTime);
				bowTie = Mathf.Max(0, bowTie-Time.deltaTime);
				moustacheTexture.enabled = (moustache>.05);
				bowtieTexture.enabled = (bowTie>.05);
				finalScore.text = "Final Score : " + ((int)money).ToString();
			}

			targetRotation = Quaternion.Euler(new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y,0) + new Vector3(-Input.GetAxis("Mouse Y"),Input.GetAxis("Mouse X"), 0)*Time.deltaTime*cameraPower);
			cameraHolder.rotation = Quaternion.Lerp(cameraHolder.rotation, targetRotation, Time.deltaTime*lerpPower);
			cameraHolder.position = Vector3.Lerp(cameraHolder.position, transform.position + Vector3.up*2, Time.deltaTime*lerpPower);
			if (GameGen.gameStarted)
			{
				for (int index = 0; index < walls.Length; index+= 1)
				{
					float alpha = .5f-.5f*Mathf.Abs(cameraHolder.rotation.eulerAngles.y - walls[index].transform.rotation.eulerAngles.y)/180f;
					walls[index].GetComponent<Renderer>().material.color = new Color(1,1,1,Mathf.Lerp(walls[index].GetComponent<Renderer>().material.color.a,alpha,Time.deltaTime*8f));
				}
			}
			float velocityMag = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude;
			if (velocityMag > .05)
			{
				Quaternion temp = Quaternion.Euler(new Vector3(0,cameraHolder.rotation.eulerAngles.y + Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))*Mathf.Rad2Deg,0));
				transform.rotation = temp;
			}
			if (grounded)
			{
				if (yVelocity <.005)
					yVelocity = 0;
				if (Input.GetKey(KeyCode.Space))
				{
					yVelocity = jumpPower;
					if (moustache>.05)
						yVelocity = jumpPower*2;
				}
			}
			else
			{
				yVelocity -= gravity*Time.deltaTime;
			}
			charControl.Move(transform.TransformDirection(new Vector3(0,yVelocity,velocityMag)*moveSpeed*Time.deltaTime));
			anim.SetFloat("velocity", velocityMag);
			anim.SetFloat("upVelocity", yVelocity);
		}
		anim.SetBool("grounded", !grounded && !lastGrounded);
		lastGrounded = grounded;
		grounded = false;
		if (lives <1)
		{
			moneyText.enabled = false;
			livesText.enabled = false;
			moneyTexture.enabled = false;
			livesTexture.enabled = false;
			moustacheTexture.enabled = false;
			bowtieTexture.enabled = false;
			finalScore.enabled = true;
			gameOver.enabled = true;
			Destroy(this.gameObject);
			GameGen.yReached = -50000f;
			GameGen.gameStarted = false;
			GameGen.difficulty = 0;
		}
	}
	public void LoseLife()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			lives-= 1;
			transform.position = new Vector3(0,GameGen.yReached + 3f,0);
		}
	}
	public void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Gold")
		{
			Destroy(c.gameObject.transform.parent.gameObject);
			if ((bowTie>.05))
				money += 50*(1+GameGen.difficulty)*10f;
			else
				money += 50*(1+GameGen.difficulty);
		}
		if (c.gameObject.tag == "Moustache")
		{
			Destroy(c.gameObject.transform.parent.gameObject);
			moustache = 5;
		}
		if (c.gameObject.tag == "Bowtie")
		{
			Destroy(c.gameObject.transform.parent.gameObject);
			lives += .5f;
			bowTie = 5;
		}
	}

}
