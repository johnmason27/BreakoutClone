using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject BallPrefab;
    public GameObject PlayerPrefab;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BallsText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI HighScoreText;

    public GameObject PanelMenu;
    public GameObject PanelPlay;
    public GameObject PanelLevelCompleted;
    public GameObject PanelGameOver;

    public GameObject[] Levels;

    public static GameManager Instance { get; private set; }

    private int score;

    public int Score 
    { 
        get { return this.score; } 
        set 
        { 
            this.score = value;
            this.ScoreText.text = "Score: " + this.score;
        } 
    }

    private int level;

    public int Level
    {
        get { return this.level; }
        set
        {
            this.level = value;
            this.LevelText.text = "Level: " + this.level;
        }
    }

    private int balls;

    public int Balls
    {
        get { return this.balls; }
        set
        {
            this.balls = value;
            this.BallsText.text = "Balls: " + this.balls;
        }
    }

    public enum State
    {
        MENU,
        INIT,
        PLAY,
        LEVELCOMPLETED,
        LOADLEVEL,
        GAMEOVER
    }

    private State state;
    private GameObject currentBall;
    private GameObject currentLevel;
    private bool isSwitchingState;

    public void ClickPlay()
    {
        this.SwitchState(State.INIT);
    }

    private void Start()
    {
        Instance = this;
        this.SwitchState(State.MENU);
    }

    public void SwitchState(State newState, float delay = 0)
    {
        this.StartCoroutine(this.SwitchDelay(newState, delay));
    }

    private IEnumerator SwitchDelay(State newState, float delay)
    {
        this.isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        this.EndState();
        this.state = newState;
        this.BeginState(newState);
        this.isSwitchingState = false;
    }

    private void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                Cursor.visible = true;
                this.HighScoreText.text = "Highscore: " + PlayerPrefs.GetInt("HighScore");
                this.PanelMenu.SetActive(true);
                break;
            case State.INIT:
                Cursor.visible = false;
                this.PanelPlay.SetActive(true);
                this.Score = 0;
                this.Level = 1;
                this.Balls = 3;
                if (this.currentLevel != null)
                {
                    Destroy(this.currentLevel);
                }
                Instantiate(this.PlayerPrefab);
                this.SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(this.currentBall);
                Destroy(this.currentLevel);
                this.Level++;
                this.PanelLevelCompleted.SetActive(true);
                this.SwitchState(State.LOADLEVEL, 2f);
                break;
            case State.LOADLEVEL:
                if (this.Level >= this.Levels.Length)
                {
                    this.SwitchState(State.GAMEOVER);
                } else
                {
                    this.currentLevel = Instantiate(this.Levels[this.Level]);
                    this.SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                if (this.Score > PlayerPrefs.GetInt("HighScore"))
                {
                    PlayerPrefs.SetInt("HighScore", this.Score);
                }
                this.PanelGameOver.SetActive(true);
                break;
        } 
    }

    private void Update()
    {
        switch (this.state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if (this.currentBall == null)
                {
                    if (this.Balls > 0)
                    {
                        this.currentBall = Instantiate(this.BallPrefab);
                    }
                    else
                    {
                        this.SwitchState(State.GAMEOVER);
                    }
                }

                if (this.currentLevel != null && this.currentLevel.transform.childCount == 0 && !this.isSwitchingState)
                {
                    this.SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if (Input.anyKeyDown)
                {
                    this.SwitchState(State.MENU);
                }
                break;
        }
    }

    private void EndState()
    {
        switch (this.state)
        {
            case State.MENU:
                this.PanelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                this.PanelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                this.PanelPlay.SetActive(false);
                this.PanelGameOver.SetActive(false);
                break;
        }
    }
}