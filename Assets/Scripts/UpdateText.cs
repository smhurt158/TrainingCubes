using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateText : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private float DefaultValue, MinValue, MaxValue, FloatIncAmount;
    public float Value;
    int timer = 0;
    void Start()
    {
        Value = DefaultValue;
        gameObject.GetComponent<TextMeshProUGUI>().text = text + Value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(float value)
    {
        Value = value;
        gameObject.GetComponent<TextMeshProUGUI>().text = text + Value.ToString();
    }

    public void DecrementValue()
    {
        if(Value > MinValue)
        {
            Value -= 1;
            gameObject.GetComponent<TextMeshProUGUI>().text = text + Value.ToString();
        }
        
    }
    public void IncrementValue()
    {
        if (Value < MaxValue)
        {
            Value += 1;
            gameObject.GetComponent<TextMeshProUGUI>().text = text + Value.ToString();
        }
    }
    public void FloatDec()
    {
        if (Value > MinValue)
        {
            Value -= FloatIncAmount;
            Value = (float)Math.Round(Value * 100f) / 100f;
            gameObject.GetComponent<TextMeshProUGUI>().text = text + Value.ToString();
        }
    }
    public void FloatInc()
    {
        if (Value < MaxValue)
        {
            Value += FloatIncAmount;
            Value = (float)Math.Round(Value * 100f) / 100f;
            gameObject.GetComponent<TextMeshProUGUI>().text = text + Value.ToString();
        }
    }
}
