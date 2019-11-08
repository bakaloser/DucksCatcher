using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnController : MonoBehaviour {
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
    public void Gameover()
    {
        Application.Quit();
    }
}
