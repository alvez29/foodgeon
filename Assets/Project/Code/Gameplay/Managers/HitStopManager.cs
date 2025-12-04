using System.Collections;
using UnityEngine;

namespace Project.Code.Gameplay.Managers
{
    public class HitStopManager : MonoBehaviour
    {
        private static HitStopManager _instance;
        public static HitStopManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                var obj = new GameObject("HitStopManager");
                _instance = obj.AddComponent<HitStopManager>();
                DontDestroyOnLoad(obj);
                return _instance;
            }
        }

        private bool _isHitStopActive = false;
        private Coroutine _hitStopRoutine;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void HitStop(float duration)
        {
            if (_hitStopRoutine != null)
                StopCoroutine(_hitStopRoutine);

            _hitStopRoutine = StartCoroutine(DoHitStop(duration));
        }

        private IEnumerator DoHitStop(float duration)
        {
            if (_isHitStopActive)
                yield break;

            _isHitStopActive = true;
            var originalTimeScale = Time.timeScale;

            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = originalTimeScale;

            _isHitStopActive = false;
        }
    }
}