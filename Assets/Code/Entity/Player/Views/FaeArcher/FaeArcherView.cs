using Code.Entity.Player.Weapon;
using Code.Systems;
using UnityEngine;

namespace Code.Entity.Player.Views.FaeArcher
{
    public class FaeArcherView : MonoBehaviour
    {
        [SerializeField]
        private SpiralWispView playerSwirlingWisps;

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
    }
}