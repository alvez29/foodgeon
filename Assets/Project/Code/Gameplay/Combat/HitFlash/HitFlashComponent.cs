using System.Collections;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Combat.HitFlash
{
    [RequireComponent(typeof(BaseStats))]
    public class HitFlashComponent : MonoBehaviour
    {
        #region Events
        public event System.Action OnFlashFinished;
        #endregion

        #region Serialized Fields
        [Header("Flash Settings")]
        [SerializeField] private Material flashMaterial;
        [SerializeField] private float flashDuration = 0.1f;
        [SerializeField] private Renderer targetRenderer;
        #endregion

        #region Properties
        public bool isFlashing = false;

        private BaseStats _stats;
        private Coroutine _flashCoroutine;
        private Material _originalMaterial;

        #endregion

        #region Unity Functions
        private void Awake()
        {
            _stats = GetComponent<BaseStats>();

            if (targetRenderer == null)
            {
                targetRenderer = GetComponentInChildren<Renderer>();
            }
            
            _originalMaterial = targetRenderer.material;
        }

        private void OnEnable()
        {
            if (_stats != null)
                _stats.OnDamageTaken += HandleDamageTaken;
        }

        private void OnDisable()
        {
            if (_stats != null)
                _stats.OnDamageTaken -= HandleDamageTaken;
        }
        #endregion

        #region Private Functions
        private void HandleDamageTaken(float current, float max, float damage, GameObject source)
        {
            if (targetRenderer == null || flashMaterial == null) return;

            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
                targetRenderer.material = targetRenderer.material;
            }

            _flashCoroutine = StartCoroutine(FlashRoutine());
        }
        #endregion

        #region Routine
        private IEnumerator FlashRoutine()
        {
            isFlashing = true;

            targetRenderer.material = flashMaterial;

            yield return new WaitForSeconds(flashDuration);

            if (targetRenderer)
                targetRenderer.material = _originalMaterial;

            _flashCoroutine = null;
            
            OnFlashFinished?.Invoke();
            
            isFlashing = false;
        }
        #endregion
    }
}
