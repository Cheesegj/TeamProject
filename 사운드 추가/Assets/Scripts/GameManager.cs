using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            SoundManager.Inst.BGMPlay(i);
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
