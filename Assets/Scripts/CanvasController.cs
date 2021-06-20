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
    [SerializeField] private  GameObject YoloShout;
    
    [SerializeField] private Slider yoloMeter;

    private Color red;
    private Color yellow;
    private Color green;
    
    private float loseYoloTime = 1f;
    private float cooldownTimer = 0f;
    private float shoutTimer = 2f;
    private float maxYOLO = 100f;
    private float currentYolo;

    private bool isGameRunning = true;
    private bool panelOpen = false;
    private bool shout = true;
    private bool onWait = false;
    public bool isYolometerActive = true;

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

        Time.timeScale = 1;
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
            else if (cooldownTimer >= 0)
            {
                cooldownTimer -= Time.deltaTime;
                //Debug.Log("DECREASE: " + cooldownTimer);
            }
            else
            {
               /* if (cooldownTimer <= 0 && onWait)
                {
                    //Debug.Log("countdown fin");
                    onWait = false;
                }*/

                if (isYolometerActive)
                {
                    //Debug.Log("yolo work");
                    UpdateYolometer(-7f * Time.deltaTime);
                }
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
        
        //Debug.Log("cdt is: " + cooldownTimer);
        if (yoloValue > 0)
        {
           /* if (!onWait)
            {
                onWait = true;
                cooldownTimer = 1.5f;
                Debug.Log("IM HERE NOT ON WAIT");
            }
            else
            {
                cooldownTimer += 0.1f;
            }*/
           cooldownTimer += 0.3f;
           //Debug.Log("WHAT COOLDOWN TIME IS: " + cooldownTimer);
        }

        if (yoloMeter.value < 100f)
        {
            shout = true;
            shoutTimer = 2f;
        }

        if (yoloMeter.value.Equals(100f))
        {
            //shoutTimer += Time.deltaTime;
            if (!YoloShout.activeSelf && shout)
            {
                shout = false;
                YoloShout.SetActive(true);
            }
        }

        if (currentYolo <= 0)
        {
            ShowPanels("Low Yolo");
            filler.SetActive(false);
            AudioManager.Instance.Stop("Alert");
            AudioManager.Instance.GameOver();
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
        float delay = 2f;
        switch (panelName)
        {
            case "Win":
                if (!panelOpen)
                {
                    winPanel.SetActive(true);
                    panelOpen = true;
                    delay = 5f;
                }
                
                break;
            
            case "Lose":
                if (!panelOpen)
                {
                    losePanel.SetActive(true);
                    panelOpen = true;
                    delay = 2f;
                }
                
                break;
            
            case "Low Yolo":
                if (!panelOpen)
                {
                    lowYoloPanel.SetActive(true);
                    panelOpen = true;
                    delay = 2f;
                }
               
                break;
        }

        StartCoroutine(GameOverDelay(delay));

    }


    IEnumerator GameOverDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0f;
    }

}
