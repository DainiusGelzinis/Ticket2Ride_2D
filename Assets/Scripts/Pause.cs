using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    [SerializeField] private AudioClip menuSound;

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

    // hide the UI so ESC will actually open it
    pauseMenuUI.SetActive(false);
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
        GameIsPaused = false;
        Time.timeScale = 1f;  // in case time was paused elsewhere
        audioSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;  // in case time was paused elsewhere
        audioSource.Play();
        SceneManager.LoadScene("MainMenu");
    }
    
}
