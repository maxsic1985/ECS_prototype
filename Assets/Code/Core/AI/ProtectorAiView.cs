using System;
using UnityEngine;
public class ProtectorAiView:MonoBehaviour
{
    public Action GameOver;

    [SerializeField] private Transform[] _waypoints;
    public Transform[] Waypoints => _waypoints;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other?.GetComponentInParent<PlayerControls>()) GameOver?.Invoke();
    }

}
