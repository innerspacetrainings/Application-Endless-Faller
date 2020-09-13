using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary> Manages the state of the level </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int Score { get; set; }

    [SerializeField] private Text scoreText, HiscoreText;
    [Space]
    [SerializeField] private string HomeSceneName;
    [SerializeField] private GameObject escPopup;
    [SerializeField] private GameObject failPopup;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject spawnOrigin;
    [SerializeField] private float platformLifetime, spawnTimer;

    bool fail;

    void Start()
    {
        instance = this;
        InvokeRepeating(nameof(Spawn), 0, spawnTimer);
        SaveHighScore();
    }

    void Update()
    {
        scoreText.text = "SCORE: " + Score;

        if (Input.GetKeyUp(KeyCode.Escape) && !fail)
            Pause();
    }

    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        escPopup.active = !escPopup.active;
    }

    public void Fail()
    {
        fail = true;
        Time.timeScale = 0;
        SaveHighScore();
        failPopup.SetActive(true);
    }

    public void Reset()
    {
        Time.timeScale = 1;
        Score = 0;
        fail = false;
        SaveHighScore();
        failPopup.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(HomeSceneName);
    }

    public void IncrementScore()
    {
        Score++;
    }

    void Spawn()
    {
        var newPlatform = Instantiate(platformPrefab);
        newPlatform.transform.position = spawnOrigin.transform.position + Vector3.up;
        Destroy(newPlatform, platformLifetime);
    }

    void SaveHighScore()
    {
        if (Score > PlayerPrefs.GetInt("HS"))
        {
            PlayerPrefs.SetInt("HS", Score);
            HiscoreText.text = "YOUR NEW HIGH SCORE IS: " + PlayerPrefs.GetInt("HS") + "!!!";
            HiscoreText.color = Color.red;
        }
        else
        {
            HiscoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HS");
            HiscoreText.color = Color.yellow;
        }
    }

    private void OnApplicationQuit()
    {
        SaveHighScore();
    }
}
