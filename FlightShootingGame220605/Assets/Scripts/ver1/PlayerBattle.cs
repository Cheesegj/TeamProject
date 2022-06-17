using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    #region Battle

    #region public variable

    #region �������� ������

    /// <summary>
    /// ���۽� ��� ������ ����
    /// </summary>
    [SerializeField]
    private int itemOfDefence = 3;
    /// <summary>
    /// ��� �������� �ִ�ġ
    /// </summary>
    [SerializeField]
    public const int itemOfDefenceMax = 3;
    /// <summary>
    /// ��� �������� ������ �ð�
    /// </summary>
    [SerializeField] 
    public const float itemOfDefenceTimeMax = 2.5f;
    /// <summary>
    /// ��� �������� ������ �ð�����
    /// </summary>
    [SerializeField] 
    public float itemOfDefenceTime = 0f;

    /// <summary>
    /// ���������� ui������Ʈ
    /// </summary>
    public GameObject[] objectOfDefence;

    #endregion

    #region ���������� ���۰� �� ����ð�
    /// <summary>
    /// ���������� ��Ÿ��
    /// </summary>
    [SerializeField] 
    public const float coolTimeOfDefence = 1.0f;
    /// <summary>
    /// ��� �������� ����ð�
    /// </summary>
    [SerializeField] 
    public const float timeOfAbsorption = 0.6f;
    /// <summary>
    /// ���������� �ð�����
    /// </summary>
    [SerializeField] 
    public float timeOfDefence = 0f;

    /// <summary>
    /// ����� ���� ����
    /// </summary>
    [SerializeField] 
    public bool absorption = false;

    /// <summary>
    /// ���������� ��Ÿ������ ���ϴ�
    /// </summary>
    public bool coolTimeBool = false;

    #endregion


    #region ������ ���õ�
    /// <summary>
    /// �̵�����
    /// </summary>
    public bool moveAble = true;

    /// <summary>
    /// �Ʒ����� ��
    /// </summary>
    public GameObject bottomWall;

    /// <summary>
    /// �罺����
    /// </summary>
    public bool reSpawn;

    public Rigidbody2D GetRigidbody2;

    #endregion

    #region ������� ���õ�

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
    }

    // �浹�ý��� �����°�
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
    /// �� �����մϴ�
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
    /// ����� ���ӽð����� ���� �մϴ�
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

        if (absorption == true)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 125 / 255f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.9f, 1.5f);
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
    /// ���������� �����մϴ�
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
