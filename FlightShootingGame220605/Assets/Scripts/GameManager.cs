using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.IO;
using System.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    public int stageLevel = 0;
    public static int enemyCount = 0;
    public MovementDirectionLoop[] loops;
    public List<Vector3> loop1;
    public List<Vector3> loop2;
    public Text scoreTextInMain;
    public Text scoreText;
    public Text fianlScoreText;
    public Text stageText;
    public GameObject destroyEffect;
    public GameObject[] enemies;
    public GameObject[] stageEnvs;
    //public List<SpawnData> spawnSet = new List<SpawnData>();
    public SpawnRes spawnSetSave = new SpawnRes();
    public SpawnRes spawnSet = new SpawnRes();
    public Transform lifeStorage;
    public GameObject pause;
    public GameObject fail;
    public GameObject coin;
    public int score;

    private const int totalStageCount = 3;

    private void Awake()
    {
        stageLevel = 0;
        enemyCount = 0;
        score = 0;
        if (Inst == null)
            Inst = this;
    }

    void Start()
    {
        SetLoop();
        SaveData();
        ReadSpawnData();

        StartCoroutine(StageController());
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        scoreTextInMain.text = score.ToString();
    }

    IEnumerator StageTextSetting()
    {
        stageText.text = "STAGE " + (stageLevel + 1);

        float alpha = 1;
        stageText.color = new Color(0, 0.5f, 1, alpha);

        while (alpha >= 0)
        {
            alpha -= Time.deltaTime * 0.5f;
            stageText.color = new Color(0, 0.5f, 1, alpha);
            yield return null;
        }
    }

    IEnumerator StageController()
    {
        for (int i = 0; i < totalStageCount; i++)
        {
            StageEnvironmentSetting();
            StartCoroutine(StageTextSetting());
            yield return EnemyMaker();
            //yield return new WaitUntil(() => enemyCount <= 0);
        }
    }

    IEnumerator EnemyMaker()
    {
        while (enemyCount < spawnSet.spawnDataList.Count)
        {
            if (spawnSet.spawnDataList[enemyCount].stage != stageLevel)
            {
                stageLevel++;
                break;
            }
            GameObject enemyClone = Instantiate(enemies[spawnSet.spawnDataList[enemyCount].type], spawnSet.spawnDataList[enemyCount].spawnPosition, Quaternion.identity);
            enemyClone.GetComponent<EnemyController>().SetParameter(spawnSet.spawnDataList[enemyCount].hp, spawnSet.spawnDataList[enemyCount].speed, spawnSet.spawnDataList[enemyCount].movePatten, spawnSet.spawnDataList[enemyCount].attackDelay, spawnSet.spawnDataList[enemyCount].attackPatten);
            yield return new WaitForSeconds(spawnSet.spawnDataList[enemyCount].spawnDelay);
            enemyCount++;
        }
    }

    public void TitleScene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void GamePause()
    {
        pause.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameContinue()
    {
        pause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnGameFail()
    {
        fail.SetActive(true);
        fianlScoreText.text = score.ToString();
    }

    private void StageEnvironmentSetting()
    {
        for (int i = 0; i < stageEnvs.Length; i++)
        {
            stageEnvs[i].SetActive(false);
        }
        stageEnvs[stageLevel].SetActive(true);
    }

    private void ReadSpawnData()
    {
        FileStream stream = new FileStream(Application.dataPath + "/SpawnData.json", FileMode.Open);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        spawnSet = JsonUtility.FromJson<SpawnRes>(jsonData);
        Debug.Log(jsonData);
    }

    private void SetLoop()
    {
        loop1.Add(new Vector3(-2.5f, 4.0f));
        loop1.Add(new Vector3(2.5f, 3.0f));
        loop1.Add(new Vector3(-2.5f, 2.0f));
        loop1.Add(new Vector3(2.5f, 1.0f));
        loop1.Add(new Vector3(-2.5f, 0.0f));
        loop1.Add(new Vector3(2.5f, -1.0f));
        loop1.Add(new Vector3(-2.5f, -2.0f));
        loop1.Add(new Vector3(2.5f, -3.0f));
        loop1.Add(new Vector3(-2.5f, -4.0f));
        loop1.Add(new Vector3(2.5f, -5.0f));
        loop1.Add(new Vector3(-2.5f, -6.0f));


        loop2.Add(new Vector3(2.5f, 4.0f));
        loop2.Add(new Vector3(-2.5f, 3.0f));
        loop2.Add(new Vector3(2.5f, 2.0f));
        loop2.Add(new Vector3(-2.5f, 1.0f));
        loop2.Add(new Vector3(2.5f, 0.0f));
        loop2.Add(new Vector3(-2.5f, -1.0f));
        loop2.Add(new Vector3(2.5f, -2.0f));
        loop2.Add(new Vector3(-2.5f, -3.0f));
        loop2.Add(new Vector3(2.5f, -4.0f));
        loop2.Add(new Vector3(-2.5f, -5.0f));
        loop2.Add(new Vector3(2.5f, -6.0f));
    }

    private void SaveData()
    {
        //json 데이터 저장용이고 나중에 비활성화
        //type, spawnPosition, spawnDelay, hp, speed, movePatten, attackDelay, attackPatten

        //SpawnRes spawnSetSave = new SpawnRes();

        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(-2.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));
        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(-0.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));
        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(1.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));
        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(-1.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));
        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(0.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));
        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(2.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));
        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(-2.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));
        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(-0.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));
        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(1.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));
        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(-1.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));
        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(0.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));
        spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(2.5f, 4.0f), 1.0f, 2, 0.8f, 0, 2.0f, 0));

        for (int i = 0; i < 10; i++)
        {
            spawnSetSave.spawnDataList.Add(new SpawnData(0, 0, new Vector3(-2.5f, 4.0f), 2.0f, 2, 1.0f, 1, 3.0f, 0));
        }
        spawnSetSave.spawnDataList[spawnSetSave.spawnDataList.Count - 1].spawnDelay = 2.0f;

        for (int i = 0; i < 2; i++)
        {
            spawnSetSave.spawnDataList.Add(new SpawnData(1, 1, new Vector3(-2.5f + (1.5f * i), 4.0f), 0.0f, 5, 1.0f, 0, 1.5f, 1));
        }
        spawnSetSave.spawnDataList[spawnSetSave.spawnDataList.Count - 1].spawnDelay = 5.0f;
        for (int i = 0; i < 2; i++)
        {
            spawnSetSave.spawnDataList.Add(new SpawnData(1, 1, new Vector3(2.5f - (1.5f * i), 4.0f), 0.0f, 5, 1.0f, 0, 1.5f, 1));
        }
        spawnSetSave.spawnDataList[spawnSetSave.spawnDataList.Count - 1].spawnDelay = 5.0f;
        for (int i = 0; i < 2; i++)
        {
            spawnSetSave.spawnDataList.Add(new SpawnData(1, 1, new Vector3(-2.5f + (1.5f * i), 4.0f), 0.0f, 5, 1.0f, 0, 1.5f, 1));
        }
        spawnSetSave.spawnDataList[spawnSetSave.spawnDataList.Count - 1].spawnDelay = 5.0f;
        for (int i = 0; i < 2; i++)
        {
            spawnSetSave.spawnDataList.Add(new SpawnData(1, 1, new Vector3(2.5f - (1.5f * i), 4.0f), 0.0f, 5, 1.0f, 0, 1.5f, 1));
        }
        spawnSetSave.spawnDataList[spawnSetSave.spawnDataList.Count - 1].spawnDelay = 5.0f;
        for (int i = 0; i < 2; i++)
        {
            spawnSetSave.spawnDataList.Add(new SpawnData(1, 1, new Vector3(-2.5f + (1.5f * i), 4.0f), 0.0f, 5, 1.0f, 0, 1.5f, 1));
        }
        spawnSetSave.spawnDataList[spawnSetSave.spawnDataList.Count - 1].spawnDelay = 5.0f;

        //spawnSetSave.Print();

        string jsonData = JsonUtility.ToJson(spawnSetSave, true);
        Debug.Log(jsonData);
        FileStream stream = new FileStream(Application.dataPath + "/SpawnData.json", FileMode.OpenOrCreate);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        stream.Write(data, 0, data.Length);
        stream.Close();


    }
}

public enum EnemyBulletPattern
{
    Direct,
    Spiral,
    Cruise,
}

[System.Serializable]
public class MovementDirectionLoop
{
    public int spawnCount;
    public Transform[] positions;
}

[System.Serializable]
public class SpawnData
{
    public int stage;

    public int type;
    public Vector2 spawnPosition;
    public float spawnDelay;

    public int hp;
    public float speed;
    public int movePatten;
    public float attackDelay;
    public int attackPatten;


    public SpawnData()
    {
        stage = 0;

        type = 0;
        spawnPosition = new Vector3(2.5f, 4.0f);
        spawnDelay = 0.85f;

        hp = 2;
        speed = 1.5f;
        movePatten = 0;
        attackDelay = 0;
        attackPatten = 0;
    }
    public SpawnData(int _stage, int _type, Vector3 _position, float _spawnDelay,
                    int _hp, float _speed, int _movePatten, float _attackDelay, int _attackPatten)
    {
        stage = _stage;

        type = _type;
        spawnPosition = _position;
        spawnDelay = _spawnDelay;

        hp = _hp;
        speed = _speed;
        movePatten = _movePatten;
        attackDelay = _attackDelay;
        attackPatten = _attackPatten;
    }

    public void SetParameter(int _stage, int _type, Vector3 _position, float _spawnDelay,
                    int _hp, float _speed, int _movePatten, float _attackDelay, int _attackPatten)
    {
        stage = _stage;

        type = _type;
        spawnPosition = _position;
        spawnDelay = _spawnDelay;

        hp = _hp;
        speed = _speed;
        movePatten = _movePatten;
        attackDelay = _attackDelay;
        attackPatten = _attackPatten;
    }

    public void Print()
    {
        Debug.Log("Type : " + type);
        Debug.Log("Position : " + spawnPosition);
    }
}

public class SpawnRes
{
    public List<SpawnData> spawnDataList = new List<SpawnData>();

    public void Print()
    {
        for (int i = 0; i < spawnDataList.Count; i++)
        {
            spawnDataList[i].Print();
        }
    }
}
/*
public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    public static int enemyCount = 0;
    public MovementDirectionLoop[] loops;
    public Text scoreTextInMain;
    public Text scoreText;
    public Text fianlScoreText;
    public Text stageText;
    public GameObject destroyEffect;
    public GameObject[] enemies;
    public GameObject[] stageEnvs;
    public Transform spawnPos;
    public Transform lifeStorage;
    public GameObject pause;
    public GameObject fail;
    public GameObject coin;
    public int score;

    private int totalStageCount = 3;

    private void Awake()
    {
        score = 0;
        if (Inst == null)
            Inst = this;
    }

    void Start()
    {
        StartCoroutine(StageController());
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        scoreTextInMain.text = score.ToString();
    }

    IEnumerator StageTextSetting(int stageLevel)
    {
        stageText.text = "STAGE " + stageLevel;

        float alpha = 1;
        stageText.color = new Color(0, 0.5f, 1, alpha);

        while (alpha >= 0)
        {
            alpha -= Time.deltaTime * 0.5f;
            stageText.color = new Color(0, 0.5f, 1, alpha);
            yield return null;
        }
    }

    IEnumerator StageController()
    {
        for (int i = 0; i < totalStageCount; i++)
        {
            StageEnvironmentSetting(i);
            StartCoroutine(StageTextSetting(i + 1));
            yield return EnemyMaker(i, GameManager.Inst.loops[i].spawnCount);
            yield return new WaitUntil(() => enemyCount <= 0);
        }
    }

    IEnumerator EnemyMaker(int targetEnemy, int spawnCount)
    {
        enemyCount = spawnCount;
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject enemyClone = Instantiate(enemies[targetEnemy], spawnPos.position, Quaternion.identity);
            yield return new WaitForSeconds(0.85f);
        }
    }

    public void TitleScene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void GamePause()
    {
        pause.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameContinue()
    {
        pause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnGameFail()
    {
        fail.SetActive(true);
        fianlScoreText.text = score.ToString();
    }

    private void StageEnvironmentSetting(int stageLevel)
    {
        for (int i = 0; i < stageEnvs.Length; i++)
        {
            stageEnvs[i].SetActive(false);
        }
        stageEnvs[stageLevel].SetActive(true);
    }
}

public enum EnemyBulletPattern
{
    Direct,
    Spiral,
    Cruise,
}

[System.Serializable]
public class MovementDirectionLoop
{
    public int spawnCount;
    public Transform[] positions;
}
*/