using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private GameObject Life4, Life3, Life2, Life1;
    private int _lives = 4;
    private float _lastDamageZ;

    public event Action Death;
    
    // Start is called before the first frame update
    void Start()
    {
        _lives = 4;
        _lastDamageZ = 0;
    }

  

    public void TestDamage()
    {
        float z = transform.position.z;
        if (z - _lastDamageZ > 25)
        {
            TakeDamage();
            _lastDamageZ = z;
        }
    }

    public void TakeDamage()
    {
        _lives--;
        UpdateLivesUI();
        if (_lives < 1 && Death != null) Death();
    }

    private void UpdateLivesUI()
    {
        if (_lives < 4)
        {
            Life4.SetActive(false);
            if (_lives < 3)
            {
                Life3.SetActive(false);
                if (_lives < 2)
                {
                    Life2.SetActive(false);
                    if (_lives < 1)
                    {
                        Life1.SetActive(false);
                    }
                }
            }
        }
    }

}
