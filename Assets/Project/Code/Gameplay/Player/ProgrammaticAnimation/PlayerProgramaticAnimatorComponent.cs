using System;
using DG.Tweening;
using UnityEngine;

namespace Project.Code.Gameplay.Player.ProgrammaticAnimation
{
    [RequireComponent(typeof(PlayerAimComponent))]
    public class PlayerProgrammaticAnimatorComponent : MonoBehaviour
    {
        private PlayerAimComponent _playerAimComponent;
        private SpriteRenderer _faceSpriteRenderer;

        [SerializeField] private GameObject face;

        [Header("Breath settings")] 
        [SerializeField] private float breathOffset = 0.2f;
        [SerializeField] private float breathDuration = 0.7f;
        [SerializeField] private Ease breathEasingCurve = Ease.InOutExpo;

        private void Awake()
        {
            _playerAimComponent = GetComponent<PlayerAimComponent>();
            _faceSpriteRenderer = face?.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var lookX = _playerAimComponent.AimDirection.x;
            var lookY = _playerAimComponent.AimDirection.y;
            
            var lookXRevMap = Mathf.InverseLerp(-1f, 1f, lookX);
            var lookYRevMap = Mathf.InverseLerp(-1f, 1f, lookY);
            var lookXMap = Mathf.Lerp(0f, 1f, lookXRevMap);
            var lookYMap = Mathf.Lerp(0f, 1f, lookYRevMap);

            var offSetX = Mathf.Lerp(0.5f, -0.5f, lookXMap);
            var offSetY = Mathf.Lerp(-0.2f, 0.2f, lookYMap);
            
            face.transform.localPosition = new Vector3(offSetX, offSetY, 0);
            _faceSpriteRenderer.sortingOrder = lookY >= 0 ? -1 : 1;
            
            //TODO: Rotate by parts ?
            //TODO: Add googly eyes ?
            //TODO: Eyes perspectives ?
        }

        private void Start()
        {
            StartBreathing();
        }

        private void StartBreathing()
        {
            transform.DOLocalMove(Vector3.up * breathOffset, 1)
                .SetEase(breathEasingCurve)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}