using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    //private Vector2 moveAmount;
    
    public float speed;
    private Rigidbody2D rb;
    
    private Vector2 normalFirst = new Vector2(-0.168f, -0.1f);
    
    void Start ()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        Move(normalFirst);
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

   
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("TestBlock"))
        {
            Destroy(gameObject);
        }
    }
  
}
