using UnityEngine;
using UnityEngine.UI;

public class LooseMenu : MonoBehaviour
{
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonExit;


    private void Start()
    {
        _buttonRestart.onClick.AddListener(RestartGame);
        _buttonExit.onClick.AddListener(ExitGame);
        gameObject.SetActive(false);
    }

    private void RestartGame()
    {
        Application.LoadLevel("Game");
    }
    private void ExitGame()
    {
        Application.Quit();
    }

    protected void OnDestroy()
    {
        _buttonRestart.onClick.RemoveAllListeners();
        _buttonExit.onClick.RemoveAllListeners();

    }
}