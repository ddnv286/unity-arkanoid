using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int score = 0;
    public int lives = 3;
    public int level = 1;

    public Ball ball { get; private set; }
    public Paddle paddle { get; private set; }
    public Brick[] bricks { get; private set; }

    private void Awake ()
    {
        // make this game object persists the entire time even when we're loading levels
        DontDestroyOnLoad(this.gameObject);
        // subscrive to the event that Unity offers to provoke the references of ball and paddle
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void Start ()
    {
        NewGame();
    }

    private void NewGame()
    {
        this.score = 0;
        this.lives = 3;

        LoadLevel(1);
    }

    private void LoadLevel (int level)
    {
        this.level = level;
        //we can either specify the index of that scene or by name in build settings
        SceneManager.LoadScene("Level" +level);
    }

    private void OnLevelLoaded (Scene scene, LoadSceneMode mode)
    {
        this.ball = FindObjectOfType<Ball>();
        this.paddle = FindObjectOfType<Paddle>();
        this.bricks = FindObjectsOfType<Brick>();
    }

    private void GameOver ()
    {
        //SceneManager.LoadScene("Game Over");
        NewGame();
    }

    private void ResetLevel()
    {
        this.ball.ResetBall();
        this.paddle.ResetPaddle();
    }

    public void Hit (Brick brick)
    {
        this.score += brick.point;
        if (Cleared())
        {
            LoadLevel(this.level + 1);
        }
    }

    private bool Cleared ()
    {
        // loop through the list of bricks and check if any of them is still active
        for (int i = 0; i < this.bricks.Length; i++)
        {
            if (this.bricks[i].gameObject.activeInHierarchy && !this.bricks[i].unbreakable)
            {
                return false;
            }
        }

        return true;
    }

    public void Miss()
    {
        this.lives--;
        if (this.lives > 0)
        {
            ResetLevel();
        } else
        {
            GameOver();
        }
    }
}
