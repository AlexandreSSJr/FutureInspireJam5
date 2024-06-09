using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Env : MonoBehaviour
{
    public static Env Instance { get; private set; }

    public float timer;
    public bool countTime = true;
    [SerializeField] private TMP_Text timerText;
    private readonly string victoryScene = "Victory";

    private void UpdateTimer() {
        timer -= Time.deltaTime;
        timerText.text = Mathf.FloorToInt(timer / 60).ToString() + ":" + Mathf.FloorToInt(timer % 60).ToString();
        if (timer <= 0) {
            Victory();
        }
    }

    private void Victory() {
      countTime = false;
      SceneManager.LoadScene(victoryScene, LoadSceneMode.Single);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        timer = 300;
        countTime = true;
    }

    void Update () {
        if (countTime) {
            UpdateTimer();
        }
    }
}