using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject go = collider.gameObject;
        if (go.name.Equals("Player")) go.GetComponent<PlayerHealth>().TestDamage();
        gameObject.SetActive(false);
    }
}
