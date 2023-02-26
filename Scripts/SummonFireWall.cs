using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonFireWall : MonoBehaviour
{
    
    public bool triggered = false;
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            triggered = true;
        }
    }
}
