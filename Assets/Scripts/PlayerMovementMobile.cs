using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementMobile : MonoBehaviour
{
	public static PlayerMovementMobile instance;
	
    private bool isJumping = false;
	private bool isDead = false;
	private bool isFinish = false;
	
	public int damage;

	private int toLeft = 0; //can turn left that many times
	private int toRight = 2; //can turn right that many times
	
	private Vector2 moveAmount;
	private Vector2 normalMove = new Vector2(0.1655f, 0.1f);
	private Vector2 normal = new Vector2(0.17f, 0.1f);
	private Vector2 jump = new Vector2(0.168f, 0.4f);
	private Vector2 fall = new Vector2(-0.168f, -0.4f);

	private Player player;

	public GameObject smoke;

	public float timerRoad = 2f;
	public bool firstRoad = true;
	private Vector2 normalFirst = new Vector2(0.168f, 0.1f);
	private Vector2 normalSecond = new Vector2(0.15f, 0.1f);

	//mew movement tyrout
	private Vector3 newPos;
	private Vector3 startPos;
	
	private float journeyLength;
	[SerializeField] private float speedNew = 10f;
	private float startTime;
	
	private bool isMoving = false;
	private bool onWait = false;
	private bool rightTurn = false;
	private bool leftTurn = false;
	public bool rivalMove = false;
	
	private Vector3 target;

	[SerializeField] private bool onTheMove;
	
	private Animator anim;

	[SerializeField] private GameObject rival;
	[SerializeField] private GameObject distancePoint;
	[SerializeField] private Transform followPoint;
	
	public float speed;
	private Rigidbody2D rb;

	void Awake()
	{
		instance = this;
	}
	
	void Start ()
	{
		//rb = GetComponent<Rigidbody2D>();
		player = GetComponent<Player>();
		anim = GetComponent<Animator>();

		normalMove = normalFirst;
	}
	
	
    void Update ()
    {
	    if (!isJumping && !isMoving && !onWait)
	    {
		    Vector2 moveInput;
		    //Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		    //we used normalized make player don't go faster when going diagonaly.
		    //moveAmount = moveInput.normalized * speed;
		
		    if(SwipeManager.swipeLeft && toLeft > 0)
		    {
			    //original movement:
			   // transform.Translate(new Vector3(-2f,1.5f));
			    
			  
			   
			   anim.Play("LeftTurn");
			   if (rivalMove)
			   {
				   StartCoroutine(RivalController.instance.DelayedLeft());
			   }
			   
			   
			    
			    toLeft--;
			    toRight++;
			    
		    }
		    if(SwipeManager.swipeRight && toRight > 0)
		    {
			    //original movement:
			    //transform.Translate(new Vector3(2f,-1.5f));
			    
			   
			    
			    anim.Play("RightTurn");
			    if (rivalMove)
			    {
				    StartCoroutine(RivalController.instance.DelayedRight());
			    }
			  
			    
			    
			    toRight--;
			    toLeft++;
			    //moveAmount = moveInput.normalized * speed;
		    }
		   
	    }
	    if (isMoving)
	    {
		    float step = speedNew * Time.deltaTime;
		    transform.position = Vector2.MoveTowards(transform.position, target, step);
		    /*Debug.Log("POS X: " + target.x);
		    Debug.Log("POS Y: " + target.y);
		    Debug.Log("POS z: " + target.z);*/
		   
		    if (rightTurn && transform.position.x >= target.x && transform.position.y <= target.y)
		    {
			    Debug.Log("IM HERE");
			    isMoving = false;
			    rightTurn = false;

		    }
		    else if (leftTurn && transform.position.x >= target.x && transform.position.y >= target.y)
		    {
			    isMoving = false;
			    leftTurn = false;
		    }
				   
	    }
	    
    }

    private void TurnLeft()
    {
	    target = new Vector3(transform.position.x + -2f, transform.position.y + 1.5f, transform.position.z + 0);

	    if (onTheMove)
	    {
		    target += ForwardMag(normalMove);
	    }
	    
	    isMoving = true;
	    leftTurn = true;
    }
    
    private void TurnRight()
    {
	    target = new Vector3(transform.position.x + 2f, transform.position.y + -1.5f, transform.position.z + 0);
	    
	    if (onTheMove)
	    {
		    target += ForwardMag(normalMove);
	    }
	   
	    isMoving = true;
	    rightTurn = true;
	    
    }

    private void FixedUpdate()
	{
		if (onTheMove)
		{
			if (!isFinish &&!isJumping && !isDead && !isMoving)
			{
				Move(normalMove);
			}
			else if(isFinish || isDead)
			{
			
				if (speed >= 0.1f)
				{
					speed -= 1f;
					Move(normalMove);
				}
				else
				{
					normalMove = Vector2.zero;
				}
			}
		}
		

	}

	
	
	void OnCollisionEnter2D(Collision2D collision)
    {
	    if (collision.collider.CompareTag("Car"))
	    {
		    CanvasController.Instance.ShowPanels("Lose");
		    isDead = true;
		    //collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		    smoke.SetActive(true);
	    }
	    
	    if (collision.collider.CompareTag("Pit"))
	    {
		    CanvasController.Instance.ShowPanels("Lose");
		    isDead = true;
		    //collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		    smoke.SetActive(true);
	    }
	    
	    if (collision.collider.CompareTag("Cow"))
	    {
		    CanvasController.Instance.ShowPanels("Lose");
		    isDead = true;
		    collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
		    smoke.SetActive(true);
	    }
	    
	    AudioManager.Instance.Stop("Drive");
    }
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Finish"))
		{
			isFinish = true;
			CanvasController.Instance.IsGameRunning = false;
			CanvasController.Instance.ShowPanels("Win");
		}
		
		if (col.CompareTag("Bump"))
		{
			//rb.isKinematic = false;
			
			StartCoroutine(Jump());
			isJumping = true;
			CanvasController.Instance.UpdateYolometer(20f);
			//Debug.Log("bumped");
		}
		
		if (col.CompareTag("Star"))
		{
			player.TakeDamage();
			Destroy(col.gameObject);
		}
		
		if (col.CompareTag("Crack"))
		{
			player.TakeDamage();
			CanvasController.Instance.UpdateYolometer(30f);
			//Debug.Log("jumped");
		}

		if (col.CompareTag("Damacana"))
		{
			player.CollectBottle();
			Destroy(col.gameObject);
		}

		if (col.CompareTag("RivalPoint"))
		{
			bool side;
			onWait = true;
			distancePoint.SetActive(true);
			if (toRight > 0)
			{
				rival.transform.position = new Vector3(followPoint.position.x + 2f, followPoint.position.y + -1.5f, followPoint.position.z + 0);
				side = true;
			}
			else
			{
				rival.transform.position = new Vector3(followPoint.position.x + -2f, followPoint.position.y + 1.5f, followPoint.position.z + 0);
				side = false;
			}
			
			StartCoroutine(RivalDelay(side));

		}

	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Close Call"))
		{
			CanvasController.Instance.UpdateYolometer(20f);
			if (!isDead)
			{
				AudioManager.Instance.Play("Honk");
			}
		}
	}

	private void Move(Vector2 moveVec)
	{
		//Vector2 currentPos = transform.position;
		Vector2 inputVector = moveVec;
		inputVector = Vector2.ClampMagnitude(inputVector, 1);
		Vector2 movement = inputVector * speed;
		//Vector2 newPos = currentPos + movement * Time.deltaTime;	
		
		//Debug.Log(newPos);
		transform.Translate(movement * Time.fixedDeltaTime, Space.World);
	}

	private Vector3 ForwardMag(Vector2 moveVec)
	{
		Vector2 inputVector = moveVec;
		inputVector = Vector2.ClampMagnitude(inputVector, 1);
		Vector2 movement = inputVector * 10f;

		return movement;
	}

	
	private IEnumerator Jump()
	{
		float originalHeight = transform.position.y;
		float maxHeight = originalHeight + 5f;

		Vector3 test = new Vector2(1f, 1.3f);
		Vector3 test2 = new Vector2(-0.8f, 0.8f);
		//rb.gravityScale = 0;
		//player.GetComponent<Player>().TakeDamage(damage);

		
		while (transform.position.y < maxHeight)
		{
			//Debug.Log("jumpingggg");
			transform.position += test * Time.deltaTime * 8f;
			//transform.position += transform.right * Time.deltaTime * 3f;
			yield return null;
		}

		while (transform.position.y > originalHeight + 3.3f) 
		{
			//Debug.Log("fallinggg");
			//Debug.Log("y: " + transform.position.y);
			//Debug.Log("o: " + originalHeight);
			transform.position -= test2 * Time.deltaTime * 8f;
			yield return null;
		}

		//rb.isKinematic = true;
		isJumping =  false;
		//rb.gravityScale = 1;
		yield return null;

	}

	private IEnumerator RivalDelay(bool turnLeft)
	{
		yield return new WaitForSeconds(1f);
		rival.SetActive(true);
		rivalMove = true;
		RivalController.instance.aheadLeft = turnLeft;
		yield return new WaitForSeconds(2f);
		onWait = false;
	}

	
	
}
