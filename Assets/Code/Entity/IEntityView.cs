using UnityEngine;

namespace Code.Entity
{
    public interface IEntityView
    {
        public void SetHealthPercent(float currentPercent);
        public void SetHealthbarText(string newText);
    }
}
