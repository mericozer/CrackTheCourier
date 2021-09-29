using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowMovement : MonoBehaviour
{
    
    private Vector2 normalMove = new Vector2(0.17f, -0.1f);
    
    public float speed;
    private Rigidbody2D rb;
    
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
       
            Move(normalMove);
       
		

    }
    private void Move(Vector2 moveVec)
    {
        Vector2 currentPos = rb.position;
        Vector2 inputVector = moveVec;
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * speed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;	
		
        rb.MovePosition(newPos);
    }
}
