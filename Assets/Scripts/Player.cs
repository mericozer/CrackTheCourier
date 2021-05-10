using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{

    public int water;
    private int maxBottle;
    
    public Image[] bottles; //UI Bottles
    
    public Sprite fullBottle;
    public Sprite emptyBottle;

    public GameObject projectile;
    
    public Transform shotPoint;

    public List<GameObject> bottleSprites = new List<GameObject>(); //On Player Bottles
    // Start is called before the first frame update
    void Start()
    {
        maxBottle = water;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage()
    {
        water -= 1;
        Instantiate(projectile, shotPoint.position, transform.rotation);
        UpdateWaterUI(false);
        CloseBottleSprites();
        
        if (water <= 0)
        {
            Destroy(gameObject);
            CanvasController.Instance.ShowPanels("Lose");
        }
    }
    
    public void CollectBottle()
    {
        if (water < maxBottle)
        {
            UpdateWaterUI(true);
            OpenBottleSprite();
            water += 1;
        }
    }
    
    void UpdateWaterUI(bool increase)
    {

        if (increase)
        {
            bottles[water].sprite = fullBottle;
        }
        else
        {
            bottles[water].sprite = emptyBottle;
        }
    
    }
    
    void OpenBottleSprite()
    {
        bottleSprites[water].SetActive(true);
    }

    void CloseBottleSprites()
    {
        bottleSprites[water].SetActive(false);
    }

   

}
