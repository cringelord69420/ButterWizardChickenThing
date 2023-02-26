using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmitterPlayerFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offSet = new Vector3(0, 0, 0);
    public bool notBeingUsed = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {if(notBeingUsed)
        transform.position = player.transform.position + offSet;
    }
    
}
