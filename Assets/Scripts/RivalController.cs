using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalController : MonoBehaviour
{
    public static RivalController instance;

    private Animator anim;
    [SerializeField] private Animator yolometerAnim;
    
    private bool isMoving;
    private bool rightTurn;
    private bool leftTurn;
    private bool normalShoot = true;
    private bool goingAway = false;
    public bool aheadLeft;

    private float attackTime = -4f;
    private float attackDuration = 1f;
    private float fastAttackDuration = 0.2f;
    private float turnTime;
    private float turnTimeNormal = 0.5f;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float attackDurationMin;
    [SerializeField] private float attackDurationMax;
    [SerializeField] private float fastAttackPrepTime;
    [SerializeField] private float turnTimeFast;

    private int attackCounter = 0;
    public  int attackLoopCount = 1;
    private int currentLoop = 0;
    

    private Vector2 normalMove = new Vector2(0.168f, 0.1f);
    
    private Vector3 target;

    [SerializeField] private GameObject rivalPackage;
    [SerializeField] private GameObject star;
    [SerializeField] private GameObject shiningStar;
    
    [SerializeField] private Transform shootPoint;
    

    void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        turnTime = turnTimeNormal;
        attackLoopCount = 1;
        currentLoop = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            float step = turnSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target, step);
            
            
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

        if (currentLoop < attackLoopCount)
        {
           AttackPattern();
        }
        else
        {
            Debug.Log("current loop is: " + currentLoop);
            Debug.Log("attack loop count is: " + attackLoopCount);
            //wait
            if (!goingAway)
            {
                goingAway = true;
                yolometerAnim.SetBool("Up", false);
                CanvasController.Instance.isYolometerActive = true;
                CanvasController.Instance.UpdateYolometer(100);
                PlayerMovementMobile.instance.rivalMove = false;
                StartCoroutine(GoAway());
            }
           
        }

        
        
    }

    void AttackPattern()
    {
         if (normalShoot)
         {
            if (!isMoving)
            {
                attackTime += Time.deltaTime;
                
                if (attackTime >= attackDuration)
                {
                    attackTime = 0;
                    attackDuration = Random.Range(attackDurationMin, attackDurationMax);
                    //TO DO 
                    //ATTACK
                    GameObject temp = Instantiate(star, shootPoint.position, transform.rotation);
                    temp.transform.parent = transform;
                    temp.GetComponent<ShurikenRoll>().waitTime = 0.3f;
                    Debug.Log("RIVAL ATTACKS");

                    attackCounter++;
                }

                if (attackCounter > 10)
                {
                    normalShoot = false;
                    attackCounter = 0;
                    attackTime = 0f;
                    turnTime = turnTimeFast;
                }
                //if attack counter is more than ADD and attacktime adding wait
            }
         }
         else
         {
            if (attackCounter > 1)
            {
                attackTime += Time.deltaTime;
                
                if (attackTime >= fastAttackDuration)
                {
                    attackTime = 0;
                    //TO DO 
                    //ATTACK
                    GameObject temp = Instantiate(shiningStar, shootPoint.position, transform.rotation);
                    temp.transform.parent = transform;
                    temp.GetComponent<ShurikenRoll>().waitTime = 0.1f;
                    Debug.Log("RIVAL ATTACKS");

                    attackCounter++;
                }
            }
            else
            {
                attackTime += Time.deltaTime;
                
                if (attackTime >= attackDuration)
                {
                    
                    if (attackCounter == 0)
                    {
                        GameObject temp = Instantiate(shiningStar, shootPoint.position, transform.rotation);
                        temp.transform.parent = transform;
                        temp.GetComponent<ShurikenRoll>().waitTime = fastAttackPrepTime;
                        attackCounter++;
                        attackTime = 0f;
                        StartCoroutine(ShootDelay());
                    }
                }
               
                
            }

            if (attackCounter == 11)
            {
                normalShoot = true;
                attackCounter = 0;
                currentLoop++;
                attackTime = -1f;
                turnTime = turnTimeNormal;
            }
            //attack counter pass ADD
         }
    }
    void FixedUpdate()
    {
        Move(normalMove);
    }
    
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Distance"))
        {
            col.gameObject.SetActive(false);
            
            speed = speed / 2;
            
            Debug.Log("left turn is : " + aheadLeft);
            if (aheadLeft)
            {
                TurnLeft();
                
            }
            else
            {
                TurnRight();
            }

        }
    }

    void OnEnable()
    {
       yolometerAnim.SetBool("Up", true);
       CanvasController.Instance.isYolometerActive = false;
    }
    public void TurnLeft()
    {
        //anim.Play("TurnLeft");
        target = new Vector3(transform.position.x + -2f, transform.position.y + 1.5f, transform.position.z + 0);
        //NEED WHEN IT GOES FORWARD
        target += ForwardMag(normalMove);
        isMoving = true;
        leftTurn = true;
    }
    
    public void TurnRight()
    {
        //anim.Play("TurnRight");
        Debug.Log("it worked");
        target = new Vector3(transform.position.x + 2f, transform.position.y + -1.5f, transform.position.z + 0);
        //NEED WHEN IT GOES FORWARD
        target += ForwardMag(normalMove);
        isMoving = true;
        rightTurn = true;
	    
    }

    
    public IEnumerator DelayedLeft()
    {
        yield return new WaitForSeconds(turnTime);
        TurnLeft();
    }
    public IEnumerator DelayedRight()
    {
        yield return new WaitForSeconds(turnTime);
        TurnRight();
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(fastAttackPrepTime);
        attackCounter++;
    }

    private IEnumerator GoAway()
    {
        
        speed = speed * 2;

        yield return new WaitForSeconds(2f);
        
        rivalPackage.SetActive(false);
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
}
