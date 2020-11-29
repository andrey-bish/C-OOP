using UnityEngine;

namespace RollingCube
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _acceleration;
        private Rigidbody _rb;
        [SerializeField] private Vector3 _moveVector;
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            _moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            _rb.AddForce(_moveVector * _acceleration, ForceMode.Impulse);

        }
    }
}

