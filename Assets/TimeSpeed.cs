using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSpeed : MonoBehaviour
{
    private float Speed;
    // Start is called before the first frame update
    void Start()
    {
        Speed = Time.timeScale;
        GetComponent<Slider>().value = Mathf.Log(Speed, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpeed()
    {
        Speed = Mathf.Pow(10, GetComponent<Slider>().value);
        Time.timeScale = Speed;
    }
}
