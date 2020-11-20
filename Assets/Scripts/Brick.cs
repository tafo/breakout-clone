using UnityEngine;

namespace Assets.Scripts
{
    public class Brick : MonoBehaviour
    {
        public int hits = 1;
        public int points = 5;
        public Vector3 rotator;
        public Material hitMaterial;

        private Material _orgMaterial;
        private Renderer _renderer;

        // Start is called before the first frame update
        private void Start()
        {
            transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
            _renderer = GetComponent<Renderer>();
            _orgMaterial = _renderer.sharedMaterial;
        }

        // Update is called once per frame
        private void Update()
        {
            transform.Rotate(rotator * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            hits--;
            if(hits <= 0)
            {
                GameManager.Instance.Score += points;
                Destroy(gameObject);
            }

            _renderer.sharedMaterial = hitMaterial;
            Invoke(nameof(RestoreMaterial), 0.1f);
        }

        private void RestoreMaterial()
        {
            _renderer.sharedMaterial = _orgMaterial;
        }
    }
}
