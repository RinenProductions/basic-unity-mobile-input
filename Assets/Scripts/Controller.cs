using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private MobileInputAnalog analog;
    // Start is called before the first frame update
    void Start()
    {
        analog = GameObject.Find("LeftAnalog").GetComponent<MobileInputAnalog>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3) analog.output * Time.deltaTime * 5.0f;
    }
}
