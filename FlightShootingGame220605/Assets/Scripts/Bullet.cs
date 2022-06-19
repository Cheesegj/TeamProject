using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed {get; set;} = 4.5f;

    void Update()
    {
        if (transform.position.y < -5.5)
            Destroy(gameObject);
        transform.position += transform.up * Speed * Time.deltaTime;
    }
}
