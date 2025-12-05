using System.Collections;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Combat.HitFlash
{
    [RequireComponent(typeof(BaseStats))]
    public class HitFlashComponent : MonoBehaviour
    {
        [Header("Flash Settings")]
        [SerializeField] private Material flashMaterial;
        [SerializeField] private float flashDuration = 0.1f;
        [SerializeField] private Renderer targetRenderer;

        private BaseStats _stats;
        private Material _originalMaterial;
        private Coroutine _flashCoroutine;

        private void Awake()
        {
            _stats = GetComponent<BaseStats>();
            
            if (targetRenderer == null)
            {
                targetRenderer = GetComponentInChildren<Renderer>();
            }
        }

        private void OnEnable()
        {
            if (_stats != null)
            {
                _stats.OnDamageTaken += HandleDamageTaken;
            }
        }

        private void OnDisable()
        {
            if (_stats != null)
            {
                _stats.OnDamageTaken -= HandleDamageTaken;
            }
        }

        private void HandleDamageTaken(float current, float max, float damage, GameObject source)
        {
            if (targetRenderer == null || flashMaterial == null) return;

            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
            }
            
            _flashCoroutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            _originalMaterial = targetRenderer.material;
            targetRenderer.material = flashMaterial;

            yield return new WaitForSeconds(flashDuration);

            targetRenderer.material = _originalMaterial;
            _flashCoroutine = null;
        }
    }
}
