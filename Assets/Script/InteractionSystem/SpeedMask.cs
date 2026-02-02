using System.Runtime.CompilerServices;
using StarterAssets;
using UnityEngine;

namespace Script.InteractionSystem
{
    public class SpeedMask : Mask
    {
        [SerializeField] private float _slowScale = 0.5f;
        private FirstPersonController _controller;
        private float _firstPlayerSpeed;
        private float _firstPlayerSprintSpeed;
        private float _fistSpeedChangeRate;


        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<FirstPersonController>();
            if (_controller == null) return;
            _firstPlayerSpeed = _controller.MoveSpeed;
            _firstPlayerSprintSpeed = _controller.SprintSpeed;
            _fistSpeedChangeRate = _controller.SpeedChangeRate;
        }

        public override void EquipMask()
        {
            base.EquipMask();
            float ratio = 1 / _slowScale;
            Time.timeScale = _slowScale;
            _controller.MoveSpeed *= ratio;
            _controller.SprintSpeed *= ratio;
            _controller.SpeedChangeRate *= ratio;
        }

        public override void UnEquipMask()
        {
            base.UnEquipMask();
            Time.timeScale = 1.0f;
            _controller.MoveSpeed = _firstPlayerSpeed;
            _controller.SprintSpeed = _firstPlayerSprintSpeed;
            _controller.SpeedChangeRate = _fistSpeedChangeRate;
        }
    }
}