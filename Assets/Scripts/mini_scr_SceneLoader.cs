using UnityEngine;
using UnityEngine.SceneManagement;

public class mini_scr_SceneLoader : MonoBehaviour
{
    public int level;

    public void StartLevel()
    {
        SceneManager.LoadScene(level);
    }
}
