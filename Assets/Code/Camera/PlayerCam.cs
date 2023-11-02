using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Camera
{
    public class PlayerCam : MonoBehaviour
    {
        private static Controls _controls;

        [SerializeField]
        private Transform player;

        [SerializeField]
        private float zoomSpeed;

        [SerializeField]
        private float lerpPace;

        private void Awake()
        {
            _controls = new Controls();
            _controls.Gameplay.Zoom.performed += ZoomDistance;
        }

        private void Update()
        {
            
        }

        private void ZoomDistance(InputAction.CallbackContext context)
        {
            Debug.Log(context);
        }

        void FixedUpdate()
        {
            Vector3 position = transform.position;
            Vector3 deltalerp = Vector3.Lerp(position, player.position, Time.deltaTime * lerpPace);
            deltalerp.z = position.z; // Don't change the Z
            position = deltalerp;
            transform.position = position;
        }
    }
}