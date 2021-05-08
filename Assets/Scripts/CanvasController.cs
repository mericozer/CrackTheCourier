using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

//General control point of UI elements
//Singleton pattern
public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject lowYoloPanel;
    [SerializeField] private GameObject filler;
    
    [SerializeField] private Slider yoloMeter;

    private Color red;
    private Color yellow;
    private Color green;
    
    private float loseYoloTime = 2f;
    private float cooldownTimer = 0f;
    private float maxYOLO = 100f;
    private float currentYolo;

    private bool isGameRunning = true;
    private bool panelOpen = false;

    private int yoloColorValue = 0;
        //Singleton
    private static CanvasController _instance;

    public static CanvasController Instance
    {
        get { return _instance; }
    }

    public bool IsGameRunning
    {
        get => isGameRunning;
        set => isGameRunning = value;
    }
  
    public void Awake()
    {
        //Checks if there are any Canvas controller in the scene because of the singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentYolo = maxYOLO;
        yoloMeter.maxValue = maxYOLO;
        yoloMeter.value = currentYolo;
        
        ColorUtility.TryParseHtmlString("#CE1212", out red);
        ColorUtility.TryParseHtmlString("#184D47", out green);
        ColorUtility.TryParseHtmlString("#FDCA40", out yellow);
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameRunning)
        {
            if (loseYoloTime > 0)
            {
                loseYoloTime -= Time.deltaTime;
            }
            else if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                UpdateYolometer(-0.1f);
                //yolo += -0.1f;
                //yoloMeter.value = yolo;
            }
        }

       
    }

    public void UpdateYolometer(float yoloValue)
    {

        currentYolo += yoloValue;
        yoloMeter.value = currentYolo;

        YoloColorValueChecker();
        
        if (yoloValue > 0)
        {
            cooldownTimer = 1.5f;
        }

        if (currentYolo <= 0)
        {
            ShowPanels("Low Yolo");
            filler.SetActive(false);
            AudioManager.Instance.Stop("Alert");
        }
        
    }

    public void YoloColorValueChecker()
    {
        int c;
        
        if (yoloMeter.value >= 75)
        {
            c = 0;
            if (c != yoloColorValue)
            {
                AudioManager.Instance.Stop("Alert");
                YolometerColor(c);
                yoloColorValue = c;
            }
        }
        else if (yoloMeter.value > 25 )
        {
            c = 1;
            if (c != yoloColorValue)
            {
                AudioManager.Instance.Stop("Alert");
                YolometerColor(c);
                yoloColorValue = c;
            }
        }
        else
        {
            c = 2;
            if (c != yoloColorValue)
            {
                AudioManager.Instance.Play("Alert");
                YolometerColor(c);
                yoloColorValue = c;
            }
        }
    }

    public void YolometerColor(int i)
    {
        if (i == 0)
        {
            filler.GetComponent<Image>().color = green;
        }
        else if (i == 1)
        {
            filler.GetComponent<Image>().color = yellow;
        }
        else
        {
            filler.GetComponent<Image>().color = red;
        }
        
    }
    

    public void ShowPanels(string panelName)
    {
        switch (panelName)
        {
            case "Win":
                if (!panelOpen)
                {
                    winPanel.SetActive(true);
                    panelOpen = true;
                }
                
                break;
            
            case "Lose":
                if (!panelOpen)
                {
                    losePanel.SetActive(true);
                    panelOpen = true;
                }
                
                break;
            
            case "Low Yolo":
                if (!panelOpen)
                {
                    lowYoloPanel.SetActive(true);
                    panelOpen = true;
                }
               
                break;
        }
        
    }
    

}
