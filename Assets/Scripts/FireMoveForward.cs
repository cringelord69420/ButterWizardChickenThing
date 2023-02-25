using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMoveForward : MonoBehaviour
{

    public GameObject fireTrigger;
    private float wallSpeed = 4f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fireTrigger.GetComponent<SummonFireWall>().triggered)
        {
            transform.Translate(Vector3.right * wallSpeed * Time.deltaTime);
        }
    }
}
