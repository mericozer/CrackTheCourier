using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{

    public int water;
    public int bottleCounter = 0;
    
    public Image[] bottles;
    public Sprite fullBottle;
    public Sprite emptyBottle;

    public GameObject projectile;
    public Transform shotPoint;

    public List<GameObject> bottleSprites = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damageAmount)
    {
        water -= damageAmount;
        Instantiate(projectile, shotPoint.position, transform.rotation);
        UpdateWaterUI(water);
        CloseBottleSprites();
        
        if (water <= 0)
        {
            Destroy(gameObject);
            CanvasController.Instance.ShowPanels("Lose");
        }
    }
    void UpdateWaterUI(int currentHealth)
    {
        for (int i = 0; i < bottles.Length; i++)
        {
            if (i < currentHealth)
            {
                bottles[i].sprite = fullBottle;
            }
            else
            {
                bottles[i].sprite = emptyBottle;
            }
        }
    }

    void CloseBottleSprites()
    {
        bottleSprites[bottleCounter].SetActive(false);
        bottleCounter++;
    }

}
