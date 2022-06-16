using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public static int damage = 4;
    [Range(1, 8)]
    public float planeSpeed = 1;
    public int Life
    {
        get { return life; }
        set
        {
            life = value;
            if (life <= 0)
            {
                GameObject effectClone = Instantiate(GameManager.Inst.destroyEffect, transform.position, Quaternion.identity);
                Destroy(effectClone, 0.6f);
                Destroy(gameObject);

                GameEnd();
            }
        }
    }
    public PrefabInformation prefabs;
    public GameObject bullet;
    public float fireRate = 0.2f;


    private Animator playerAnimController;
    private Rigidbody2D playerRigidbody;
    private float xVal, yVal;
    private bool isFireable = true;
    private float rechargeCoolTime;
    private int life;
    
    void Start()
    {
        playerAnimController = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        rechargeCoolTime = fireRate;
        Life = GameManager.Inst.lifeStorage.transform.childCount;
    }

    void Update()
    {
        xVal = Input.GetAxisRaw("Horizontal");
        yVal = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(xVal) < 0.1f)
        {
            playerAnimController.Play("pCenter");
        }
        else if (xVal > 0)
        {
            playerAnimController.Play("pRight");
        }
        else if (xVal < 0)
        {
            playerAnimController.Play("pLeft");
        }

        if (!isFireable)
        {
            rechargeCoolTime -= Time.deltaTime;
            if (rechargeCoolTime < 0)
            {
                isFireable = true;
                rechargeCoolTime = fireRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.J))
        {
            if (isFireable)
            {
                GameObject clone = Instantiate(bullet, transform.position, Quaternion.identity);
                clone.GetComponent<Bullet>().Speed = 18.5f;
                Destroy(clone, 3.5f);
                isFireable = false;
            }
        }
    }

    private void GameEnd()
    {
        GameManager.Inst.OnGameFail();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("EnemyBullet"))
        {
            if (Life > 0)
            {
                Life -= 1;
                GameObject targetLife = GameManager.Inst.lifeStorage.transform.GetChild(0).gameObject;
                Destroy(targetLife);
                Destroy(other.gameObject);
            }
        }
        else if (other.tag.Equals("Coin"))
        {
            GameManager.Inst.score += 200;
            Destroy(other.gameObject);
        }
    }

    private void FixedUpdate()
    {
        playerRigidbody.velocity = (Vector3.right * xVal + Vector3.up * yVal) * planeSpeed;
    }
}

[System.Serializable]
public class PrefabInformation
{
    public GameObject bullet;
}