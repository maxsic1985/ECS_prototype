using UniRx;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour, IPlayer
{
    public float Speed = 1;
    private Rigidbody2D _componentRigidbody;
    private INozeble _noizeObserver;

    [Inject]
    private void Init()
    {
        Time.timeScale = 1;
        _componentRigidbody =GetComponent<Rigidbody2D>();
        _noizeObserver = new NoizeObserver();
        var inpusKeyKodeStream = Observable.EveryFixedUpdate().Where(_ => Input.anyKey);
        var leftClick = inpusKeyKodeStream.Where(_ => Input.GetKey(KeyCode.LeftArrow)).Subscribe(_ => { _componentRigidbody.velocity = Vector2.zero; _componentRigidbody.velocity += Vector2.left * Speed; }).AddTo(this);
        var rightClick = inpusKeyKodeStream.Where(_ => Input.GetKey(KeyCode.RightArrow)).Subscribe(_ => { _componentRigidbody.velocity = Vector2.zero; _componentRigidbody.velocity += Vector2.right * Speed; }).AddTo(this);
        var downClick = inpusKeyKodeStream.Where(_ => Input.GetKey(KeyCode.DownArrow)).Subscribe(_ => { _componentRigidbody.velocity = Vector2.zero; _componentRigidbody.velocity += Vector2.down * Speed; }).AddTo(this);
        var upClick = inpusKeyKodeStream.Where(_ => Input.GetKey(KeyCode.UpArrow)).Subscribe(_ => { _componentRigidbody.velocity = Vector2.zero; _componentRigidbody.velocity += Vector2.up * Speed; }).AddTo(this);
    }
}
