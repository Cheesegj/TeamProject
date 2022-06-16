using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    public Transform[] loop1;
    public Text scoreTextInMain;
    public Text scoreText;
    public Text fianlScoreText;
    public GameObject destroyEffect;
    public GameObject[] enemies;
    public Transform spawnPos;
    public Transform lifeStorage;
    public GameObject pause;
    public GameObject fail;
    public GameObject coin;
    public int score;

    private void Awake()
    {
        score = 0;
        if (Inst == null)
            Inst = this;
    }

    void Start()
    {
        StartCoroutine(EnemyMaker(0));
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        scoreTextInMain.text = score.ToString();
    }

    IEnumerator EnemyMaker(int targetEnemy)
    {
        for (int i = 0; i < 5; i++)
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
}

public enum EnemyBulletPattern
{
    Direct,
    Spiral,
    Cruise,
}
