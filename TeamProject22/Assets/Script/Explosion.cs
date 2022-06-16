using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float timeOfExplosion = 0.6f;

    private float timeForWait;

    // Update is called once per frame
    void Update()
    {
        Waiting();
    }

    private void Waiting()
    {
        timeForWait += Time.deltaTime;

        if (timeForWait > timeOfExplosion)
        {
            Destroy(gameObject);
        }

    }
}
