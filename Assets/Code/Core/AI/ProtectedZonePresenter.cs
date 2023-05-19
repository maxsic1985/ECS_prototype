using UnityEngine;

public class ProtectedZonePresenter : MonoBehaviour
{
    [SerializeField] private ProtectorAIPresenter _protector;
    [SerializeField] private EnemyGuardTrigger _guardTriggers;


    private void Start()
    {
        _guardTriggers.TriggerEnter += OnContact;
        _guardTriggers.TriggerExit += OnExit;
    }

    public void Deinit()
    {
        _guardTriggers.TriggerEnter -= OnContact;
        _guardTriggers.TriggerExit -= OnExit;
    }

    private void OnContact(GameObject gameObject)
    {
        _protector.StartProtection(gameObject);
    }

    private void OnExit(GameObject gameObject)
    {
        _protector.FinishProtection(gameObject);
    }

    private void OnDestroy()
    {
        Deinit();
    }
}
