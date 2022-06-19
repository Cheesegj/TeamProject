using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSetting : MonoBehaviour
{
    public SoundManager SD;
    public static int poolCount = 3;
    public float backgroundLength;
    public Background[] backgrounds;

    private GameObject[,] bgPrefabs;
    public int bgms;
    void Start()
    {
        bgPrefabs = new GameObject[backgrounds.Length, poolCount];
        SD.BGMPlay(bgms);
        for (int i = 0; i < backgrounds.Length; i++)
        {
            for (int j = 0; j < poolCount; j++)
            {
                GameObject bgClone = Instantiate(backgrounds[i].m_image, Vector3.up * backgroundLength * j, Quaternion.identity);
                bgClone.transform.SetParent(gameObject.transform);
                bgPrefabs[i, j] = bgClone;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < bgPrefabs.GetLength(0); i++)
        {
            for (int j = 0; j < poolCount; j++)
            {
                if (bgPrefabs[i, j].transform.position.y < -backgroundLength)
                {
                    bgPrefabs[i, j].transform.position = Vector3.up * (bgPrefabs[i, backgrounds[i].Current].transform.position.y + backgroundLength);
                    backgrounds[i].Current++;
                }
                
                bgPrefabs[i, j].transform.position -= Vector3.up * backgrounds[i].speed * Time.deltaTime;
            }
        }
    }
}

[System.Serializable]
public class Background
{
    public int Current
    {
        get { return current; }
        set
        {
            if (value > EnvironmentSetting.poolCount - 1)
            {
                current = value - EnvironmentSetting.poolCount;
            }
            else
            {
                current = value;
            }
        }
    }
    public GameObject m_image;
    public float speed;

    private int current = EnvironmentSetting.poolCount - 1;
}
