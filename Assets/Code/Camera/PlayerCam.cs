using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Camera
{
    public class PlayerCam : MonoBehaviour
    {
        [SerializeField]
        private Transform player;

        [SerializeField]
        private float zoomSpeed;

        [SerializeField]
        private float lerpPace;

        public void ZoomDistance(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            transform.position = (Vector3.forward * direction.y) + transform.position;
        }

        void FixedUpdate()
        {
            Vector3 position = transform.position;
            Vector3 deltaLerp = Vector3.Lerp(position, player.position, Time.deltaTime * lerpPace);
            deltaLerp.z = position.z; // Don't change the Z
            position = deltaLerp;
            transform.position = position;
        }
    }
}