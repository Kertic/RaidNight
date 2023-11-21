using Code.Camera;
using Code.Entity.Player.Views;
using Code.Entity.Player.Weapon;
using UnityEngine;

namespace Code.Entity.Player.StateMachines
{
    public class PlayerFaeAutoAttackStateMachine : PlayerAutoAttackStateMachine
    {
        public new PlayerFaeArcherStateMachine _PlayerControlsStateMachine => (PlayerFaeArcherStateMachine)playerControlsStateMachine;

        private void OnValidate()
        {
            if (playerControlsStateMachine is not PlayerFaeArcherStateMachine) Debug.Log("The assigned controls state machine is not a " + typeof(PlayerFaeArcherStateMachine) + ", please change.");
        }

        public override void FireAutoAttack()
        {
            TrackingProjectile projectile = weapon.FireProjectile(playerControlsStateMachine._AutoAttackTarget.transform, projectileSpeed);
            var targetTrans = playerControlsStateMachine._AutoAttackTarget.transform;
            BeginAttackCooldown();
            _PlayerControlsStateMachine.RemoveMischiefCharge();

            projectile.m_onHit += hit2Ds =>
            {
                foreach (RaycastHit2D hit in hit2Ds)
                {
                    Entity entity = hit.collider.gameObject.GetComponent<Entity>();
                    if (entity != null)
                    {
                        entity.TakeDamage(playerData._BaseAttackDamage);
                        _PlayerControlsStateMachine.LaunchWispAttack(targetTrans);
                    }
                }
            };
        }

        private void OnDrawGizmosSelected()
        {
            if (PlayerCam.Instance != null)
            {
                Gizmos.color = Color.blue;
                Vector3 mousePos = PlayerCam.mousePosition;
                Gizmos.DrawWireCube(mousePos, Vector3.one);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(mousePos, transform.position);
            }
        }
    }
}