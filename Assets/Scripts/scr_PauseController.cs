using UnityEngine;

public class scr_PauseController : MonoBehaviour
{
    private bool paused;
    [SerializeField] private AudioSource sceneAudioSource;
    [SerializeField] private GameObject pausePanel;

    public void PauseGame()
    {
        paused = !paused;
        if (!paused)
        {
            Time.timeScale = 1;
            sceneAudioSource.Play();
            pausePanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            sceneAudioSource.Pause();
        }
    }
}