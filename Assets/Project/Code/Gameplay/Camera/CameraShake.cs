using UnityEngine;

namespace Project.Code.Gameplay.Camera
{
    public class CameraShake : MonoBehaviour
    {
        [Header("Shake Settings")]
        [Tooltip("Maximum angle (in degrees) the camera can rotate during extreme trauma.")]
        [SerializeField]
        private float maxAngle = 5f;
        
        [Tooltip("Maximum offset (in units) the camera can move during extreme trauma.")]
        [SerializeField]
        private float maxOffset = 0.5f;
        
        [Tooltip("How fast the shake oscillates.")]
        [SerializeField]
        private float frequency = 15f;
        
        [Tooltip("How fast the trauma decreases per second (e.g., 1.0 means trauma goes from 1 to 0 in 1 second).")]
        [SerializeField]
        private float traumaDecay = 1.5f;

        [Header("Debug")]
        [Range(0, 1)] [SerializeField]
        private float trauma;

        private float _seed;

        private void Awake()
        {
            _seed = Random.value;
        }

        private void Update()
        {
            if (trauma <= 0)
            {
                if (transform.localPosition == Vector3.zero && transform.localRotation == Quaternion.identity) return;
                
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                return;
            }

            trauma = Mathf.Clamp01(trauma - Time.unscaledDeltaTime * traumaDecay);

            var shake = trauma * trauma;
            var noiseTime = Time.time * frequency;
            
            var yaw = (Mathf.PerlinNoise(_seed, noiseTime) * 2 - 1) * maxAngle * shake;
            var pitch = (Mathf.PerlinNoise(_seed + 1, noiseTime) * 2 - 1) * maxAngle * shake;
            var roll = (Mathf.PerlinNoise(_seed + 2, noiseTime) * 2 - 1) * maxAngle * shake;

            var offsetX = (Mathf.PerlinNoise(_seed + 3, noiseTime) * 2 - 1) * maxOffset * shake;
            var offsetY = (Mathf.PerlinNoise(_seed + 4, noiseTime) * 2 - 1) * maxOffset * shake;

            transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
            transform.localPosition = new Vector3(offsetX, offsetY, 0);
        }

        public void AddTrauma(float amount)
        {
            trauma = Mathf.Clamp01(trauma + amount);
        }

        [ContextMenu("Test Minor Shake (0.3)")]
        private void TestMinorShake() => AddTrauma(0.3f);

        [ContextMenu("Test Major Shake (0.7)")]
        private void TestMajorShake() => AddTrauma(0.7f);
        
        [ContextMenu("Test Extreme Shake (1.0)")]
        private void TestExtremeShake() => AddTrauma(1.0f);
    }
}
