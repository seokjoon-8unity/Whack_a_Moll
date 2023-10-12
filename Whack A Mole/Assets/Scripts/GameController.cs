using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CountDown countDown;
    [SerializeField] 
    private MoleSpawner moleSpawner;
    private int score;

    public int Score
    {
        set => score = Mathf.Max(0, value);
        get => score;
    }

    [field:SerializeField]
    public float MaxTime { private set; get; }
    public float CurrentTime { private set; get; }

    private void Start()
    {
        countDown.StartCountDown(GameStart);
    }
    private void GameStart()
    {
        moleSpawner.Setup();

        StartCoroutine("OnTimeCount");
    }

    private IEnumerator OnTimeCount()
    {
        CurrentTime = MaxTime;

        while ( CurrentTime > 0 )
        {
            CurrentTime -= Time.deltaTime;

            yield return null;
        }

        GameOver();
    }

    private void GameOver()
    {
        PlayerPrefs.SetInt("CurrentScore", Score);

        SceneManager.LoadScene("GameOver");
    }
}
