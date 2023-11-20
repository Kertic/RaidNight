using JetBrains.Annotations;
using UnityEngine;

namespace Code.Entity
{
    public class TrainingDummy : Entity
    {
        [SerializeField, NotNull]
        private TrainingDummyEntityView dummyEntityView;

        // Start is called before the first frame update
        void Start()
        {
            _view = dummyEntityView;
        }

        // Update is called once per frame
        void Update() { }

        protected override void OnDeath()
        {
            _view.SetHealthbarText("I have died.");
        }

        public override void TakeDamage(float damage)
        {
            if (currentHealth <= 0)
            {
                damage = -maxHealth;
                _view.SetHealthbarText("");
            }

            base.TakeDamage(damage);
        }
    }
}