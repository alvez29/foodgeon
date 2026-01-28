using System;
using Project.Code.Core;
using Project.Code.Core.Interfaces;
using Project.Code.Utils;
using UnityEngine;

namespace Project.Code.Gameplay.Enemies.Behavior.Chili.Components
{
    public class FireTrail : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float damage = 10f;
        [SerializeField] private float duration = 2f;

        private SphereCollider _sphereCollider;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
        }

        private void Start()
        {
            Destroy(gameObject, duration);
            HitboxDebugger.Instance.DrawSphere(transform.position, _sphereCollider?.radius ?? 0f, Color.softRed, 3f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Tags.Player) && other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage, 0f, gameObject);
            }
        }
    }
}
