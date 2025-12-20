using UnityEngine;

namespace SupplyChain.Model
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f;

        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, vertical, 0) * moveSpeed * Time.deltaTime;
            transform.position += movement;
        }

        public void SetPosition(Vector2 position)
        {
            position -= Vector2.one * 0.5f;
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }
    }
}