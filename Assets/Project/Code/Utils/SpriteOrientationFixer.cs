using System;
using Project.Code.Gameplay.Player;
using UnityEngine;

namespace Project.Code.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteOrientationFixer : MonoBehaviour
    {
        [SerializeField] private MovementComponent movementComponent;

        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            if (_spriteRenderer && movementComponent)
            {
                _spriteRenderer.flipX = movementComponent.MoveDirection.x < 0;
            }
            else
            {
                Debug.LogWarning("Something is missing!");
            }
        }
    }
    
    
}