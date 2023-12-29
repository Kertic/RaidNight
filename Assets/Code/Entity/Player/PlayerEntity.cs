using Code.Entity.Player.StateMachines;
using Code.Entity.Player.Views;
using UnityEngine;

namespace Code.Entity.Player
{
    [RequireComponent(typeof(PlayerControlsStateMachine), typeof(PlayerData))]
    public class PlayerEntity : Entity
    {
        [SerializeField]
        private PlayerHealthView playerHealthView;

        protected PlayerControlsStateMachine m_playerControlsStateMachine;
        protected PlayerData m_playerData;

        public PlayerControlsStateMachine _PlayerControlsStateMachine => m_playerControlsStateMachine;
        public PlayerData _PlayerData => m_playerData;

        protected override void Awake()
        {
            base.Awake();
            m_playerControlsStateMachine = GetComponent<PlayerControlsStateMachine>();
            m_playerData = GetComponent<PlayerData>();
        }

        private void Start()
        {
            m_view = playerHealthView;
        }

        protected override void OnDeath()
        {
            m_view.SetHealthbarText("I have died.");
        }
    }
}