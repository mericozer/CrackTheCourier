using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalController : MonoBehaviour
{
    public static RivalController instance;

    private Animator anim;
    
    private bool isMoving;
    private bool rightTurn;
    private bool leftTurn;

    private float attackTime = 0f;
    private float attackDuration = 1f;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float attackDurationMin;
    [SerializeField] private float attackDurationMax;

    private Vector2 normalMove = new Vector2(0.168f, 0.1f);
    
    private Vector3 target;

    [SerializeField] private GameObject star;
    [SerializeField] private Transform shootPoint;

    void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            float step = turnSpeed * Time.deltaTime;
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

        if (!isMoving)
        {
            attackTime += Time.deltaTime;

            if (attackTime >= attackDuration)
            {
                attackTime = 0;
                attackDuration = Random.Range(attackDurationMin, attackDurationMax);
                //TO DO 
                //ATTACK
                Instantiate(star, shootPoint.position, transform.rotation);
                Debug.Log("RIVAL ATTACKS");
            }
        }
    }
    
    public void TurnLeft()
    {
        //anim.Play("TurnLeft");
        target = new Vector3(transform.position.x + -2f, transform.position.y + 1.5f, transform.position.z + 0);
        //NEED WHEN IT GOES FORWARD
        //target += ForwardMag(normalMove);
        isMoving = true;
        leftTurn = true;
    }
    
    public void TurnRight()
    {
        //anim.Play("TurnRight");
        target = new Vector3(transform.position.x + 2f, transform.position.y + -1.5f, transform.position.z + 0);
        //NEED WHEN IT GOES FORWARD
        //target += ForwardMag(normalMove);
        isMoving = true;
        rightTurn = true;
	    
    }

    public IEnumerator DelayedLeft()
    {
        yield return new WaitForSeconds(0.5f);
        TurnLeft();
    }
    public IEnumerator DelayedRight()
    {
        yield return new WaitForSeconds(0.5f);
        TurnRight();
    }
}
