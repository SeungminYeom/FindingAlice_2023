using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    void Update()
    {
        gameObject.transform.Translate(transform.up* Time.deltaTime);
    }
}
