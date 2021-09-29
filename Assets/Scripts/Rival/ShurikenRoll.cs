using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenRoll : MonoBehaviour
{
    public float speed;
    public float lifeTime;

    public float waitTime = 0f;
    public float timer = 0f;

    public int damage;

    private bool shoot = false;
    
    private Vector2 move = new Vector2(-0.168f, -0.1f);
    // Start is called before the first frame update
    void Start()
    {
        //Destroys game object when the lifetime is passed. 
        Invoke("DestroyProjectile", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(!shoot)
        {
            /*if (waitTime < 1.2f)
            {
                shoot = true;
                RandomStarSound();
            }
            else
            {
                StartCoroutine(DelaySound());
            }*/
            
            shoot = true;
            RandomStarSound();
            
        }
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            
            
            transform.Translate(move * speed * Time.deltaTime);
            gameObject.transform.parent = null;
        }
        
    }

    void DestroyProjectile()
    {
        //Instantiate, what am I spawning, at what position, at what rotation
        Destroy(gameObject);
    }
    
    private void RandomStarSound()
    {
        int i = Random.Range(1, 3);

        if (i == 1)
        {
            AudioManager.Instance.Play("StarOne");
            //Debug.Log("STARONE");
        }
        else
        {
            AudioManager.Instance.Play("StarTwo");
            //Debug.Log("STARTWO");
        }
    }

    IEnumerator DelaySound()
    {
        yield return new WaitForSeconds(1f);
        RandomStarSound();
    }
}
