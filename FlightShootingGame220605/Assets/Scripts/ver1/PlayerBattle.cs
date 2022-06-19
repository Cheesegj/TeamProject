using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    public SoundManager SD;
    #region Battle

    #region public variable

    #region 방어아이템 재고관리

    /// <summary>
    /// 시작시 방어 아이템 갯수
    /// </summary>
    [SerializeField]
    private int itemOfDefence = 3;
    /// <summary>
    /// 방어 아이템의 최대치
    /// </summary>
    [SerializeField]
    public const int itemOfDefenceMax = 3;
    /// <summary>
    /// 방어 아이템의 재충전 시간
    /// </summary>
    [SerializeField] 
    public const float itemOfDefenceTimeMax = 2.5f;
    /// <summary>
    /// 방어 아이템의 재충전 시간측정
    /// </summary>
    [SerializeField] 
    public float itemOfDefenceTime = 0f;

    /// <summary>
    /// 방어아이템의 ui오브젝트
    /// </summary>
    public GameObject[] objectOfDefence;

    #endregion

    #region 방어아이템의 시작과 끝 재사용시간
    /// <summary>
    /// 방어아이템의 쿨타임
    /// </summary>
    [SerializeField] 
    public const float coolTimeOfDefence = 1.0f;
    /// <summary>
    /// 방어 아이템의 흡수시간
    /// </summary>
    [SerializeField] 
    public const float timeOfAbsorption = 0.6f;
    /// <summary>
    /// 방어아이템의 시간측정
    /// </summary>
    [SerializeField] 
    public float timeOfDefence = 0f;

    /// <summary>
    /// 흡수의 상태 유무
    /// </summary>
    [SerializeField] 
    public bool absorption = false;

    /// <summary>
    /// 방어아이템이 쿨타임인지 봅니다
    /// </summary>
    public bool coolTimeBool = false;

    #endregion

    #region 리스폰 관련들
    /// <summary>
    /// 이동유무
    /// </summary>
    public bool moveAble = true;

    /// <summary>
    /// 아래측의 벽
    /// </summary>
    public GameObject bottomWall;

    /// <summary>
    /// 재스폰중
    /// </summary>
    public bool reSpawn;

    public Rigidbody2D GetRigidbody2;

    public bool noDeath;

    public float noDeathTime;

    public const float noDeathTimeMax = 3f;

    #endregion

    #region 공격흡수 관련들

    public float absorptionEnergy;


    #endregion

    #region 공격력 레벨 관련들

    /// <summary>
    /// 공격력 레벨
    /// </summary>
    public int powerOfAttack;

    [SerializeField]
    public const int powerOfAttackMax = 3;

    public GameObject[] powerUi;
    public RectTransform powerBar;

    #endregion

    #endregion

    #region private variable



    #endregion


    #region         Default----------------------------------------


    #region public
    #endregion public


    #region private

    private void Awake()
    {
        noDeathTime = 0;
        powerOfAttack = 0;
        absorptionEnergy = 0;
           reSpawn = false;
        absorption = false;
        coolTimeBool = false;
        moveAble = true;
        GetRigidbody2 = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Update2()
    {
        defenceRecharge();

        defence();

        defenceTime();

        ReSpawning();

        if (noDeathTime != 0)
        {
            noDeathTime += Time.deltaTime;
            if(noDeathTime > noDeathTimeMax)
            {
                noDeathTime = 0;
            }
        }
    }

    // 충돌시스템 가져온것
    /*
    private void OnCollisionEnter2D2(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")

        {
            if (absorption == true)
            {
                Destroy(collision.gameObject);

            }
            else
            {
                Destroy(gameObject);
            }

        }


    }
    */

    #endregion private

    #endregion      Default----------------------------------------


    #region         Custom----------------------------------------

    #region public
    #endregion

    #region private

    /// <summary>
    /// 방어를 실행합니다
    /// </summary>
    public void defence()
    {
        if (itemOfDefence == 0 || coolTimeBool == true)
        {
            return;
        }

        if(moveAble == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            itemOfDefence--;
            absorption = true;
            coolTimeBool = true;
            timeOfDefence = 0.01f;

            objectOfDefence[itemOfDefence].SetActive(false);
        }



    }

    /// <summary>
    /// 방어의 지속시간동안 일을 합니다
    /// </summary>
    public void defenceTime()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);

        if (moveAble == false)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 125 / 255f);

            bottomWall.SetActive(false);
        }
        else
        {
            bottomWall.SetActive(true);
        }

        if (absorption == true || noDeathTime != 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 125 / 255f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.2f, 2f);
        }
        else
        {

            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.6f, 1f);
        }

        if (timeOfDefence == 0)
        {
            return;
        }


        timeOfDefence += Time.deltaTime;

        if (timeOfDefence < timeOfAbsorption)
        {
            absorption = true;
        }
        else
        {
            absorption = false;
        }


        if (timeOfDefence < coolTimeOfDefence)
        {
            coolTimeBool = true;
        }
        else
        {
            coolTimeBool = false;
            timeOfDefence = 0f;
        }
    }

    /// <summary>
    /// 방어아이템을 충전합니다
    /// </summary>
    public void defenceRecharge()
    {
        if (itemOfDefence == itemOfDefenceMax)
        {
            return;
        }

        itemOfDefenceTime += Time.deltaTime;

        if (itemOfDefenceTime > itemOfDefenceTimeMax)
        {
            itemOfDefence++;
            itemOfDefenceTime = 0;

            objectOfDefence[itemOfDefence - 1].SetActive(true);
        }

    }

    /// <summary>
    /// 재소환될때 행하는 것들 관련입니다
    /// </summary>
    public void ReSpawning()
    {
        if (reSpawn == true)
        {
            GetRigidbody2.velocity = (Vector3.right * 0f + Vector3.up * 1.0f) * 6.0f;
            
        }

        if(gameObject.transform.position.y > -3)
        {
            reSpawn = false;
            moveAble = true;
            
        }

    }



    #endregion

    #endregion      Custom----------------------------------------

    #endregion
}
