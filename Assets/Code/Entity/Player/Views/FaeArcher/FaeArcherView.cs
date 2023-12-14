using Code.Camera;
using Code.Entity.Player.Weapon;
using Code.Systems;
using UnityEngine;

namespace Code.Entity.Player.Views.FaeArcher
{
    public class FaeArcherView : MonoBehaviour
    {
        [SerializeField]
        private SpiralWispView playerSwirlingWisps;

        [SerializeField]
        private SpriteRenderer chargeUpIndicator;

        [SerializeField]
        private float chargeIndicatorMaxDistance;

        void Start() { }

        void Update() { }

        public void AttachWispsToProjectile(TrackingProjectile projectile)
        {
            playerSwirlingWisps.AttachWispsToProjectile(projectile);
        }

        public void AddWispToSwirlingWisps()
        {
            playerSwirlingWisps.AddWisp();
        }

        public void RemoveWispFromSwirlingWisps()
        {
            playerSwirlingWisps.RemoveWisp();
        }

        public void SetChargeProgress(float progress)
        {
            if (progress == 0.0f)
            {
                chargeUpIndicator.enabled = false;
                return;
            }

            chargeUpIndicator.enabled = true;
            chargeUpIndicator.size = new Vector2(chargeUpIndicator.size.x, progress * chargeIndicatorMaxDistance);
            chargeUpIndicator.transform.rotation = Quaternion.Euler(0, 0,
                Utils.Vector2.GetRotationOfObjectOnCircle(PlayerCam.mousePosition, transform.position) - 90.0f);
        }
    }
}