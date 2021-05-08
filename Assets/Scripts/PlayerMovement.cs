using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private bool isJumping = false;
	private bool isDead = false;
	private bool isFinish = false;
	
	public int damage;

	private int toLeft = 0;
	private int toRight = 2;
	
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

	public float speed;
	private Rigidbody2D rb;
	void Start ()
	{
		//rb = GetComponent<Rigidbody2D>();
		player = GetComponent<Player>();

		normalMove = normalFirst;
	}
	
	
    void Update ()
    {
	    if (!isJumping)
	    {
		    Vector2 moveInput;
		    //Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		    //we used normalized make player don't go faster when going diagonaly.
		    //moveAmount = moveInput.normalized * speed;
		
		    if(Input.GetKeyDown(KeyCode.A) && toLeft > 0)
		    {
			    transform.Translate(new Vector3(-2f,1.5f));
			    //moveInput = new Vector2(-1, 1);
			    toLeft--;
			    toRight++;
			    //moveAmount = moveInput.normalized * speed;
		    }
		    if(Input.GetKeyDown(KeyCode.D) && toRight > 0)
		    {
			    transform.Translate(new Vector3(2f,-1.5f));
			    toRight--;
			    toLeft++;
			    //moveAmount = moveInput.normalized * speed;
		    }
	    }
	    
		 
	}
	private void FixedUpdate()
	{
		/*if (timerRoad <= 0)
		{
			timerRoad -= Time.fixedDeltaTime;
			
			if (firstRoad)
			{
				normalMove = normalSecond;
				firstRoad = false;
				timerRoad = 3f;
			}
			else
			{
				normalMove = normalFirst;
				firstRoad = true;
				timerRoad = 2f;
			}
		}*/
		
		if (!isFinish &&!isJumping && !isDead )
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
			Debug.Log("bumped");
		}
		
		if (col.CompareTag("Crack"))
		{
			player.TakeDamage(1);
			CanvasController.Instance.UpdateYolometer(30f);
			Debug.Log("jumped");
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
		transform.Translate(movement * Time.deltaTime, Space.World);
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
			Debug.Log("jumpingggg");
			transform.position += test * Time.deltaTime * 8f;
			//transform.position += transform.right * Time.deltaTime * 3f;
			yield return null;
		}

		while (transform.position.y > originalHeight + 3.3f) 
		{
			Debug.Log("fallinggg");
			Debug.Log("y: " + transform.position.y);
			Debug.Log("o: " + originalHeight);
			transform.position -= test2 * Time.deltaTime * 8f;
			yield return null;
		}

		//rb.isKinematic = true;
		isJumping =  false;
		//rb.gravityScale = 1;
		yield return null;

	}

	
	
	
	



} 