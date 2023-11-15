using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class PlayerCam : MonoBehaviour
    {
        static public PlayerCam Instance { get; private set; }
        public UnityEngine.Camera Camera { get; private set; }

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

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            Camera = GetComponent<UnityEngine.Camera>();
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