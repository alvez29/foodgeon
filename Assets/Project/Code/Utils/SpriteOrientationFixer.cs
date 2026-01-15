using System;
using Project.Code.Gameplay.Player;
using UnityEngine;

namespace Project.Code.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteOrientationFixer : MonoBehaviour
    {
        [SerializeField] private PlayerAimComponent _playerAimComponent;

        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            if (_spriteRenderer && _playerAimComponent)
            {
                print($"aim direction is {_playerAimComponent.AimDirection}");
                _spriteRenderer.flipX = _playerAimComponent.AimDirection is { x: > 0 };
            }
            else
            {
                Debug.LogWarning("Something is missing!");
            }
        }
    }
    
    
}