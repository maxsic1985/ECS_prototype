using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    #region Fields
    [SerializeField] private Button _buttonStart;
    [SerializeField] private Button _buttonExit;
    #endregion
    #region Life cycle
    private void Start()
    {
        _buttonStart.onClick.AddListener(StartGame);
        _buttonExit.onClick.AddListener(ExitGame);

    }

    private void StartGame()
    {
        Application.LoadLevel("Game");
    }
    private void ExitGame()
    {
        Application.Quit();
    }

    protected void OnDestroy()
    {
        _buttonStart.onClick.RemoveAllListeners();
    }
    #endregion
}