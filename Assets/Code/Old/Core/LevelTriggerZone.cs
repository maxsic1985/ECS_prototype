using UnityEngine;

public class LevelTriggerZone : MonoBehaviour
{
    [SerializeField] private LooseMenu _looseMenu;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other?.GetComponentInParent<PlayerControls>()) Win();
    }

    private void Win()
    {
        Debug.Log($"You Win");
        Time.timeScale = 0;
        _looseMenu.gameObject.SetActive(true);
    }
}
