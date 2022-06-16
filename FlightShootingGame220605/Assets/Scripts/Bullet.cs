using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed {get; set;} = 4.5f;

    void Update()
    {
        transform.position += transform.up * Speed * Time.deltaTime;
    }
}
