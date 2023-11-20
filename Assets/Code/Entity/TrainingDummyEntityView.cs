using System;
using Code.Entity.Player.Views;

namespace Code.Entity
{
    public class TrainingDummyEntityView : PlayerCastView, IEntityView
    {
        private void Awake()
        {
            ChangeViewState(ViewState.COOLINGDOWN);
        }

        public void SetHealthPercent(float currentPercent)
        {
            SetProgress(currentPercent);
        }

        public void SetHealthbarText(string newText)
        {
            SetText(newText);
        }
    }
}
