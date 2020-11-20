using UnityEngine;

namespace Assets.Scripts
{
    public class Ball : MonoBehaviour
    {
        private const float Speed = 20f;
        private Rigidbody _rigidbody;
        private Vector3 _velocity;
        private Renderer _renderer;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _renderer = GetComponent<Renderer>();        
            Invoke("Launch", 0.5f);
        }

        private void Launch()
        {
            _rigidbody.velocity = Vector3.up * Speed;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * Speed;
            _velocity = _rigidbody.velocity;
            if (_renderer.isVisible) return;
            GameManager.Instance.Balls--;
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            _rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);
        }
    }
}
