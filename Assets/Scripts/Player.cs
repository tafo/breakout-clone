using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();    
        }

        private void FixedUpdate()
        {
            _rigidbody.MovePosition(new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 50)).x, -17, 0));
        }
    }
}
