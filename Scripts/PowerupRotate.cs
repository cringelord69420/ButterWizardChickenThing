using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupRotate : MonoBehaviour
{
   public float speed = 4f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Rotate", 0f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Rotate()
    {
    transform.Rotate(0, 1, 0, Space.Self);
    }
}
