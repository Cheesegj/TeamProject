using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    #region public variable

    #region 방어아이템 재고관리

    /// <summary>
    /// 방어 아이템 갯수
    /// </summary>
    private int   itemOfDefence =3;
    /// <summary>
    /// 방어 아이템의 최대치
    /// </summary>
    public const int   itemOfDefenceMax = 3;
    /// <summary>
    /// 방어 아이템의 재충전 시간
    /// </summary>
    public const float itemOfDefenceTimeMax = 1.5f;
    /// <summary>
    /// 방어 아이템의 재충전 시간측정
    /// </summary>
    public float itemOfDefenceTime = 0f;

    #endregion

    #region 방어아이템의 시작과 끝 재사용시간
    /// <summary>
    /// 방어아이템의 쿨타임
    /// </summary>
    public const float coolTimeOfDefence = 1.0f;
    /// <summary>
    /// 방어 아이템의 흡수시간
    /// </summary>
    public const float timeOfAbsorption = 0.6f;
    /// <summary>
    /// 방어아이템의 시간측정
    /// </summary>
    public float timeOfDefence = 0f;

    /// <summary>
    /// 흡수의 상태 유무
    /// </summary>
    public bool absorption = false;

    /// <summary>
    /// 방어아이템이 쿨타임인지 봅니다
    /// </summary>
    public bool coolTimeBool = false;

    #endregion

    #endregion

    #region private variable



    #endregion


    #region         Default----------------------------------------


    #region public
    #endregion public


    #region private

    private void Update()
    {
        defenceRecharge();

        defence();

        defenceTime();


    }

    private void OnCollisionEnter2D(Collision2D collision)
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


    #endregion private

    #endregion      Default----------------------------------------


    #region         Custom----------------------------------------

    #region public
    #endregion

    #region private

    /// <summary>
    /// 방어를 실행합니다
    /// </summary>
    private void defence()
    {
        if(itemOfDefence == 0 || coolTimeBool == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            itemOfDefence--;
            absorption = true;
            coolTimeBool = true;
            timeOfDefence = 0.01f;
        }



    }

    /// <summary>
    /// 방어의 지속시간동안 일을 합니다
    /// </summary>
    private void defenceTime()
    {
        if (absorption == true)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 125 / 255f);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
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


        if(timeOfDefence < coolTimeOfDefence)
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
    private void defenceRecharge()
    {
        if(itemOfDefence == itemOfDefenceMax)
        {
            return;
        }

        itemOfDefenceTime += Time.deltaTime;

        if(itemOfDefenceTime > itemOfDefenceTimeMax)
        {
            itemOfDefence++;
            itemOfDefenceTime =0;
        }

    }

    #endregion

    #endregion      Custom----------------------------------------

}
/*
 
     #region         Default----------------------------------------


    #region public
    #endregion public


    #region private



    #endregion private

    #endregion      Default----------------------------------------


    #region         Custom----------------------------------------

    #region public
    #endregion

    #region private
    #endregion

    #endregion      Custom----------------------------------------
 
 
 */