using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalPoint : MonoBehaviour
{
    [SerializeField] private int loopCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("rival is here");
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.2f);
        RivalController.instance.attackLoopCount = loopCount;
    }
}
