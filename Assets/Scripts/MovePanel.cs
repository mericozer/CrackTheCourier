using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanel : MonoBehaviour
{
    public enum MoveType
    {
        Position,
        Rotation
    }

    public MoveType move;
    
    public AudioSource soundSource;
    public AudioClip[] audioList;

    private Vector3 newPos;
    private Vector3 startPos;
    
    [SerializeField] private float speed = 1f;
    [SerializeField] private float x = 0f;
    [SerializeField] private float y = 0f;
    [SerializeField] private float z = 0f;

    [SerializeField] private bool haveSound;
    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;
    
    
    [SerializeField] private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        if (move == MoveType.Position)
        {
            newPos = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
            startPos = transform.position;

            journeyLength = Vector3.Distance(startPos, newPos);
        }

        soundSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (isMoving)
        {

            if (move == MoveType.Position)
            {
                //Debug.Log("Position will change");
                
                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - startTime) * speed;

                // Fraction of journey completed equals current distance divided by total distance.
                float fractionOfJourney = distCovered / journeyLength;

                // Set our position as a fraction of the distance between the markers.
                transform.position = Vector3.Lerp(startPos, newPos, fractionOfJourney);

                if (haveSound)
                {
                    soundSource.PlayOneShot(audioList[0]);
                }
                
            }
            
            else if(move == MoveType.Rotation)

            {
                if (haveSound)
                {
                    soundSource.PlayOneShot(audioList[1]);
                    soundSource.PlayOneShot(audioList[0]);
                    haveSound = false;
                }
                
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(x, y, z), Time.deltaTime * speed);
                
               
            }

            
        }

    }

    public void ActivateObject()
    {
        isMoving = true;
        startTime = Time.time;
    }

   
}
