using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileInputAnalog : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //RectTransform component of analog object
    private RectTransform analog;
    //Analog starting position
    private RectTransform analogCenter;
    //Radius of analog movemenent range
    private float analogRadius = 50.0f;
    //Analog movement threshold
    private float threshold = 0.2f;
    //Should output vector be reset on release
    public bool resetOutput;
    //Output vector
    public Vector2 output;
    //If left analog
    public bool leftAnalog = true;
    //If analog is engaged
    public bool isActive = false;
    //separate touch for separate analogs
    public int touchID;

    private Canvas canvas;

    // Use this for initialization
    void Start()
    {
        canvas = transform.root.GetComponent<Canvas>();
        analog = GetComponent<RectTransform>();
        analogCenter = transform.parent.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && isActive)
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            Touch myTouch = Input.GetTouch(0);

            Touch[] touch = Input.touches;
            for (int i = 0; i < touch.Length; i++)
            {
                if (leftAnalog)
                {
                    if (touch[i].position.x < Screen.width / 2)
                    {
                        myTouch = touch[i];
                        break;
                    }
                    else
                        continue;
                }
                else
                {
                    if (touch[i].position.x > Screen.width / 2)
                    {
                        myTouch = touch[i];
                        break;
                    }
                    else
                        continue;
                }
            }
            
            float distance = Vector2.Distance(analogCenter.position, myTouch.position);
#else
            float distance = Vector2.Distance(analogCenter.position, Input.mousePosition);
#endif


            //Check if analog stick is within radius
            if (distance <= analogRadius)
            {

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                analog.position = myTouch.position;
#else
                analog.position = Input.mousePosition;
#endif
            }
            //If analog stick is not within the radius, do not make it go outside
            else
            {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                analog.position = analogCenter.position + ((Vector3) myTouch.position - analogCenter.position).normalized * analogRadius;
#else
                analog.position = analogCenter.position + (Vector3)(Input.mousePosition - analogCenter.position).normalized * analogRadius;
#endif
            }

            if (distance > threshold)
            {
                output = new Vector2(analog.localPosition.x / analogRadius, analog.localPosition.y / analogRadius) * canvas.scaleFactor;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isActive = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isActive = false;
        analog.localPosition = Vector2.zero;
        if (resetOutput)
            output = Vector2.zero;
    }

}