using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * This class is prototype class for facilitate the creation 
 * of a main menu in new projet
 */
public class UI_MainMenu : MonoBehaviour
{
    /// <summary>
    /// Allow to load a new game scene
    /// </summary>
    /// <param name="indexScene">Index for the scene to load</param>
    public void LoadLevel(int indexScene)
    {
        SceneManager.LoadScene(indexScene);
    }

    /// <summary>
    /// Allow to quit the application. Does't work in editor.
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
    }
}
