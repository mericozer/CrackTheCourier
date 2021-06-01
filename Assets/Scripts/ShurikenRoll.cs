using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenRoll : MonoBehaviour
{
    public float speed;
    public float lifeTime;

    public int damage;
    
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
        transform.Translate(move * speed * Time.deltaTime);
    }

    void DestroyProjectile()
    {
        //Instantiate, what am I spawning, at what position, at what rotation
        Destroy(gameObject);
    }
}
