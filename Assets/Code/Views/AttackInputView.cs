using System;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    public sealed class AttackInputView : BaseView
    {
        [SerializeField] private GameObject _attackBtn;
        public GameObject AttackBtn => _attackBtn;


        private void Start()
        {
            ShowBtn(false);
        }

        public void ShowBtn(bool show)
        {
            if (show) _attackBtn.SetActive(true);
            else _attackBtn.SetActive(false);
        }
    }
}