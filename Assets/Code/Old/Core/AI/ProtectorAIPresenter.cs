using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ProtectorAIPresenter : MonoBehaviour, IProtector
{
    [SerializeField] private LooseMenu _gameView;
    [SerializeField] private PlayerControls _playerView;
    [SerializeField] private ProtectorAiView _view;
    [SerializeField] private AIDestinationSetter _destinationSetter;
    [SerializeField] private AIPatrolPath _patrolPath;


    private PatrolAIModel _model;
    private bool _isPatrolling;
    private SpriteRenderer _spriteRendere;

    private void Start()
    {
        _view.GameOver += GameOver;
        _model = new PatrolAIModel(_view.Waypoints);
        _destinationSetter.target = _model.GetNextTarget();
        _isPatrolling = true;
        _patrolPath.TargetReached += OnTargetReached;
        _spriteRendere = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Deinit()
    {
        _patrolPath.TargetReached -= OnTargetReached;
    }

    private void OnTargetReached()
    {
        _destinationSetter.target = _isPatrolling
            ? _model.GetNextTarget()
            : _model.GetClosestTarget(_playerView.transform.position);
    }

    public void StartProtection(GameObject invader)
    {
        _isPatrolling = false;
        _destinationSetter.target = invader.transform;
        _spriteRendere.color = Color.red;
    }

    public void FinishProtection(GameObject invader)
    {
        _isPatrolling = true;
        _destinationSetter.target = _model.GetClosestTarget(_playerView.transform.position);
        _spriteRendere.color = Color.blue;
    }

    public void GameOver()
    {
        _gameView.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        Deinit();
    }
}
