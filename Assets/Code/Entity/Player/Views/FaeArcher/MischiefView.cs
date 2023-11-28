using UnityEngine;

namespace Code.Entity.Player.Views.FaeArcher
{
    public class MischiefView : MonoBehaviour
    {
        [SerializeField]
        private WispView mischiefTemplate;

        private WispView _mischiefWispViewInstance;

        private void Awake()
        {
            _mischiefWispViewInstance = Instantiate(mischiefTemplate, transform);
        }

        public Transform GetMischiefTransform()
        {
            return _mischiefWispViewInstance.transform;
        }

        public void SetMischiefPosition(Vector3 newPos)
        {
            _mischiefWispViewInstance.transform.position = newPos;
        }

        public void SetMischiefRotation(Quaternion newRotation)
        {
            _mischiefWispViewInstance.transform.rotation = newRotation;
        }

        public void SetMischiefVisible(bool isEnabled)
        {
            _mischiefWispViewInstance.gameObject.SetActive(isEnabled);
        }
    }
}