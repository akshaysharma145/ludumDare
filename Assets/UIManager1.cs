using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager1 : MonoBehaviour
{
    public GameObject instructionsPanel;
    public void play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void mainmenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void quit()
    {
        Application.Quit();
    }
    public void pause()
    {
        Time.timeScale = 0f;
    }
    public void resume()
    {
        Time.timeScale = 1f;
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void instructions()
    {
        instructionsPanel.SetActive(true);
    }
    public void closeinstructions()
    {
        instructionsPanel.SetActive(false);
    }

}
