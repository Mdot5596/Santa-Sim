using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public Text redScoreText;
    public Text blueScoreText;
    public Text greenScoreText;
    public Text timerText;
    public GameObject endScreen;
    public Text finalScoreText;

    [Header("Game Settings")]
    public float gameDuration = 60f;  // seconds

    [Header("Win Condition")]
    public int targetPerColour = 10;
    public string nextSceneName = "Level 2";


    private float timer;
    private bool gameActive = false;

    // Score for each present ID (0,1,2)
    private int[] scores = new int[3];

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timer = gameDuration;
        gameActive = true;
        UpdateUI();
    }

    void Update()
    {
        if (!gameActive) return;

        // countdown the timer
        timer -= Time.deltaTime;
        timerText.text = Mathf.Ceil(timer).ToString();

        // when timer ends
        if (timer <= 0f)
        {
            EndGame();
        }
    }

    public bool IsGameActive()
    {
        return gameActive;
    }

    public void AddScore(int presentID)
    {
        if (!gameActive) return;

        if (presentID < 0 || presentID >= scores.Length)
            return;

        scores[presentID]++;
        UpdateUI();

        CheckLevelComplete();
    }

    void CheckLevelComplete()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] < targetPerColour)
                return; // not complete yet
        }

        // If we reach here, all colours meet the target
        LevelComplete();
    }

    void LevelComplete()
    {
        gameActive = false;

        // Optional: stop spawning
        ConveyorSpawner spawner = FindAnyObjectByType<ConveyorSpawner>();
        if (spawner)
            spawner.StopSpawning();

        // Optional delay or UI
        Invoke(nameof(LoadNextLevel), 1.5f);
    }

    void LoadNextLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }

    private void UpdateUI()
    {
        redScoreText.text = scores[0].ToString();
        blueScoreText.text = scores[1].ToString();
        greenScoreText.text = scores[2].ToString();
    }

    private void EndGame()
    {
        gameActive = false;

        // Stop conveyor belt from spawning
        FindObjectOfType<ConveyorSpawner>().StopSpawning();

        // Show final totals
        int total = scores[0] + scores[1] + scores[2];
        finalScoreText.text = total.ToString();

        endScreen.SetActive(true);
    }
}