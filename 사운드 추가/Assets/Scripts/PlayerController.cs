using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public PlayerBattle PB;

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

    private void Awake()
    {
        PB = gameObject.GetComponent<PlayerBattle>();
    }

    void Start()
    {
        playerAnimController = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        rechargeCoolTime = fireRate;
        Life = GameManager.Inst.lifeStorage.transform.childCount;
    }

    // ���� Update
    /*
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
    */

    #region ����������Ʈ�� ��������
    private void Update()
    {
        UpdateNomal();
        PB.Update2();
    }

    /// <summary>
    /// ������ ������Ʈ�� �����ϴ� �̵����� �ڵ��
    /// </summary>
    private void UpdateNomal()
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

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.J))
        {
            if (isFireable && PB.moveAble)
            {
                SoundManager.Inst.SFXPlay(2);
                GameObject clone = Instantiate(bullet, transform.position, Quaternion.identity);
                clone.GetComponent<Bullet>().Speed = 18.5f;
                Destroy(clone, 3.5f);
                isFireable = false;
            }
        }
    }

    #endregion
    private void GameEnd()
    {
        GameManager.Inst.OnGameFail();
    }

    // ���� ��Ʈ���ſ���
    /*
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
    */

    #region ������ ��Ʈ���ſ���
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("EnemyBullet"))
        {
            if (PB.absorption == true)
            {
                Destroy(other.gameObject);

                PB.absorptionEnergy += 25;
                if(PB.absorptionEnergy >= 100)
                {
                    PB.powerOfAttack++;
                    PB.absorptionEnergy =0;
                }

            }else if(PB.moveAble == false)
            {

            }
            else if (Life > 0)
            {
                // ������ ������ ���� �ý���
                /*
                Life -= 1;
                GameObject targetLife = GameManager.Inst.lifeStorage.transform.GetChild(0).gameObject;
                Destroy(targetLife);
                Destroy(other.gameObject);
                */

                // ������Ǵ� ������ �ý���
                Life -= 1;
                GameObject targetLife = GameManager.Inst.lifeStorage.transform.GetChild(0).gameObject;
                Destroy(targetLife);
                Destroy(other.gameObject);

                // ���������߿� �������� ���մϴ�
                PB.moveAble = false;
                PB.reSpawn = true;

                // ���ߵɰ�� �Ʒ������� �̵���ŵ�ϴ�
                gameObject.transform.position = new Vector3(0,-10,0);
            }

        }
        else if (other.tag.Equals("Coin"))
        {
            SoundManager.Inst.SFXPlay(1);
            GameManager.Inst.score += 200;
            Destroy(other.gameObject);
        }
    }
    #endregion

    private void FixedUpdate()
    {
        // �̵������� �����϶��� �ҷ� �̵������ϰ���
        if(PB.moveAble == true)
        {
            playerRigidbody.velocity = (Vector3.right * xVal + Vector3.up * yVal) * planeSpeed;
        }

    }




}

[System.Serializable]
public class PrefabInformation
{
    public GameObject bullet;
}