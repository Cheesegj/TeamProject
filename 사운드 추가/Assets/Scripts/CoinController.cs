using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private Rigidbody2D coinRigidbody2D;
    
    

    void Start()
    {
        coinRigidbody2D = GetComponent<Rigidbody2D>();
        coinRigidbody2D.AddRelativeForce(Vector2.up * 200);
        Destroy(gameObject, 5f);
    }
}
