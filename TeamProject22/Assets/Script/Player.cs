using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    #region public variable

    #region �������� ������

    /// <summary>
    /// ��� ������ ����
    /// </summary>
    private int   itemOfDefence =3;
    /// <summary>
    /// ��� �������� �ִ�ġ
    /// </summary>
    public const int   itemOfDefenceMax = 3;
    /// <summary>
    /// ��� �������� ������ �ð�
    /// </summary>
    public const float itemOfDefenceTimeMax = 1.5f;
    /// <summary>
    /// ��� �������� ������ �ð�����
    /// </summary>
    public float itemOfDefenceTime = 0f;

    #endregion

    #region ���������� ���۰� �� ����ð�
    /// <summary>
    /// ���������� ��Ÿ��
    /// </summary>
    public const float coolTimeOfDefence = 1.0f;
    /// <summary>
    /// ��� �������� ����ð�
    /// </summary>
    public const float timeOfAbsorption = 0.6f;
    /// <summary>
    /// ���������� �ð�����
    /// </summary>
    public float timeOfDefence = 0f;

    /// <summary>
    /// ����� ���� ����
    /// </summary>
    public bool absorption = false;

    /// <summary>
    /// ���������� ��Ÿ������ ���ϴ�
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
    /// �� �����մϴ�
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
    /// ����� ���ӽð����� ���� �մϴ�
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
    /// ���������� �����մϴ�
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