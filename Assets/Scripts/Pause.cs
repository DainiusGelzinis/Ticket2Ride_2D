using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    [SerializeField] private AudioClip menuSound;

    [SerializeField] private PlayerController playerController;

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private AudioSource audioSource;

    private void Start()
    {
        // grab your AudioSource as before
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = menuSound;

        // make sure nothing is paused when this scene begins
        GameIsPaused = false;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !PlayerController.GameIsDead)
        {
            if (GameIsPaused)
            {
                audioSource.Play();
                Resume();
            }
            else
            {
                audioSource.Play();
                PauseGame();
            }

        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        audioSource.Play();
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        audioSource.Play();
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ReloadScene()
{
    // Reset both the “dead” flag and your saved checkpoints:
    PlayerController.GameIsDead = false;
    CheckpointManager.Instance.ResetCheckpoints();

    // Play SFX & unpause:
    Time.timeScale = 1f;
    audioSource.Play();

    // Hide the pause UI:
    pauseMenuUI.SetActive(false);

    // Now reload the scene fresh
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

    public void MainMenu()
{
    PlayerController.GameIsDead = false;
    CheckpointManager.Instance.ResetCheckpoints();

    Time.timeScale = 1f;
    audioSource.Play();
    pauseMenuUI.SetActive(false);

    SceneManager.LoadScene("MainMenu");
}
    
    public void RestartFromCheckpoint()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        playerController.RespawnAtCheckpoint();
        pauseMenuUI.SetActive(false);
    }
    
}
