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

    // 원본 Update
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

    #region 원본업데이트의 수정본들
    private void Update()
    {
        UpdateNomal();
        PB.Update2();
    }

    /// <summary>
    /// 기존의 업데이트에 존재하던 이동관련 코드들
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
                PB.SD.SFXPlay(2);

                if(PB.powerOfAttack == 0)
                {
                    GameObject clone = Instantiate(bullet, transform.position, Quaternion.identity);
                    clone.GetComponent<Bullet>().Speed = 18.5f;
                    Destroy(clone, 3.5f);
                    isFireable = false;
                }
                else if(PB.powerOfAttack == 1)
                {
                    Vector3 vector1 = new Vector3(transform.position.x + 0.15f, transform.position.y, transform.position.z);
                    Vector3 vector2 = new Vector3(transform.position.x - 0.15f, transform.position.y, transform.position.z);
                    GameObject clone1 = Instantiate(bullet, vector1, Quaternion.identity);
                    GameObject clone2 = Instantiate(bullet, vector2, Quaternion.identity);

                    clone1.GetComponent<Bullet>().Speed = 18.5f;
                    clone2.GetComponent<Bullet>().Speed = 18.5f;
                    Destroy(clone1, 3.5f);
                    Destroy(clone2, 3.5f);
                    isFireable = false;
                }
                else
                {
                    Vector3 vector1 = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
                    Vector3 vector2 = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
                    Vector3 vector3 = new Vector3(transform.position.x , transform.position.y+0.2f, transform.position.z);
                    GameObject clone1 = Instantiate(bullet, vector1, Quaternion.identity);
                    GameObject clone2 = Instantiate(bullet, vector2, Quaternion.identity);
                    GameObject clone3 = Instantiate(bullet, vector3, Quaternion.identity);

                    clone1.GetComponent<Bullet>().Speed = 18.5f;
                    clone2.GetComponent<Bullet>().Speed = 18.5f;
                    clone3.GetComponent<Bullet>().Speed = 18.5f;
                    Destroy(clone1, 3.5f);
                    Destroy(clone2, 3.5f);
                    Destroy(clone3, 3.5f);
                    isFireable = false;
                }

            }
        }
    }

    #endregion
    private void GameEnd()
    {
        GameManager.Inst.OnGameFail();
    }

    // 원본 온트리거엔터
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

    #region 수정된 온트리거엔터
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("EnemyBullet"))
        {
            if (PB.absorption == true)
            {
                Destroy(other.gameObject);

                if(PB.powerOfAttack != 2)
                {
                    PB.absorptionEnergy += 25;
                    if (PB.absorptionEnergy >= 100)
                    {
                        PB.powerOfAttack++;
                        PB.absorptionEnergy = 0;
                        PB.powerUi[PB.powerOfAttack].SetActive(true);
                        PB.SD.SFXPlay(0);
                    }

                    PB.powerBar.offsetMax = new Vector2(-(100-PB.absorptionEnergy), 10);
                }



            }else if(PB.moveAble == false || PB.noDeathTime != 0)
            {

            }
            else if (Life > 0)
            {
                // 기존의 라이프 감소 시스템
                /*
                Life -= 1;
                GameObject targetLife = GameManager.Inst.lifeStorage.transform.GetChild(0).gameObject;
                Destroy(targetLife);
                Destroy(other.gameObject);
                */

                // 재생성되는 라이프 시스템
                Life -= 1;
                GameObject targetLife = GameManager.Inst.lifeStorage.transform.GetChild(0).gameObject;
                Destroy(targetLife);
                Destroy(other.gameObject);

                // 리스폰도중엔 움직이지 못합니다
                PB.moveAble = false;
                PB.reSpawn = true;

                // 격추될경우 아래쪽으로 이동시킵니다
                gameObject.transform.position = new Vector3(0,-10,0);
                PB.noDeathTime = 0.1f;
            }

        }
        else if (other.tag.Equals("Coin"))
        {
            GameManager.Inst.score += 200;
            Destroy(other.gameObject);
            PB.SD.SFXPlay(0);
        }
    }
    #endregion

    private void FixedUpdate()
    {
        // 이동가능한 상태일때만 불로 이동가능하게함
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