using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Code.Gameplay.Projectile
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        public event Action<GameObject> OnProjectileHit;

        #region Fields

        [NonSerialized] private float _speed;
        [NonSerialized] private float _radius;
        [NonSerialized] private LayerMask _collisionsMask;
        [NonSerialized] private Vector3 _forwardVector;
        [NonSerialized] private float _lifeSpan;

        #endregion

        #region Properties

        private Collider _projectileCollider;
        private Rigidbody _projectileRigidbody;
        private Coroutine _projectileRoutine;

        #endregion

        #region Unity methods

        private void Awake()
        {
            _projectileCollider = GetComponent<Collider>();
            _projectileRigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            //TODO: Fix the moving
            _projectileRigidbody.MovePosition(_projectileRigidbody.position +
                                              _forwardVector.normalized * (_speed * Time.fixedDeltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            var otherLayer = other.gameObject.layer;

            if ((_collisionsMask.value & (1 << otherLayer)) == 0)
                return;

            OnProjectileHit?.Invoke(other.gameObject);
            Destroy(gameObject);
        }


        private void Start()
        {
            if (_radius == 0) _radius = ((SphereCollider)_projectileCollider)?.radius ?? 0;
            _projectileRoutine = StartCoroutine(LifeSpanCoroutine());
        }

        #endregion

        #region Public methods

        public void InitializeData(float newSpeed, float newRadius, LayerMask newCollisionLayers,
            Vector3 newForwardVector, float newProjectileLifeSpan)
        {
            _speed = newSpeed;
            _radius = newRadius;
            _collisionsMask = newCollisionLayers;
            _forwardVector = newForwardVector;
            _lifeSpan = newProjectileLifeSpan;

            if (_projectileRoutine != null) StopCoroutine(_projectileRoutine);
            _projectileRoutine = StartCoroutine(LifeSpanCoroutine());
        }

        #endregion

        #region Private methods

        private IEnumerator LifeSpanCoroutine()
        {
            yield return new WaitForSeconds(_lifeSpan);
            Destroy(gameObject);
        }

        #endregion
    }
}