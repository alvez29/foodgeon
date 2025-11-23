using UnityEngine;

namespace Project.Code.Utils
{
    public class Billboard : MonoBehaviour {
        [SerializeField] private BillboardType billboardType;

        [Header("Lock Rotation")]
        [SerializeField] private bool lockX;
        [SerializeField] private bool lockY;
        [SerializeField] private bool lockZ;
        
        private Camera _camera;

        private Vector3 _originalRotation;

        public enum BillboardType { LookAtCamera, CameraForward };

        private void Awake() 
        {
            _originalRotation = transform.rotation.eulerAngles;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void LateUpdate() 
        {
            switch (billboardType) {
                case BillboardType.LookAtCamera:
                    transform.LookAt(_camera.transform.position, Vector3.up);
                    break;
                case BillboardType.CameraForward:
                    transform.forward = _camera.transform.forward;
                    break;
                default:
                    break;
            }
            var rotation = transform.rotation.eulerAngles;
            if (lockX) { rotation.x = _originalRotation.x; }
            if (lockY) { rotation.y = _originalRotation.y; }
            if (lockZ) { rotation.z = _originalRotation.z; }
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}