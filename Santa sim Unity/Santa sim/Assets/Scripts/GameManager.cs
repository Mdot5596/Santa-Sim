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

        // Safety check
        if (presentID < 0 || presentID >= scores.Length)
        {
            Debug.LogError("Invalid present ID: " + presentID);
            return;
        }

        scores[presentID]++;
        UpdateUI();
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