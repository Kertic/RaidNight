using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Player.StateMachines.PlayerWeaponStates
{
    public class PlayerAutoAttackView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform autoAttackChargingBar, autoAttackCooldownBar, autoAttackHaltedBar, autoAttackBackground;

        [SerializeField]
        private TextMeshProUGUI words;

        public enum ViewState
        {
            CHARGING,
            COOLINGDOWN,
            IDLE,
            NUMOFVIEWSTATES
        }

        public void SetProgress(float progress)
        {
            progress = Math.Clamp(progress, 0.0f, 1.0f);
            autoAttackCooldownBar.sizeDelta = autoAttackHaltedBar.sizeDelta = autoAttackChargingBar.sizeDelta = (autoAttackBackground.sizeDelta * Vector2.right * progress) + (Vector2.up * autoAttackBackground.sizeDelta);
            words.text = progress.ToString();
        }

        public void ChangeViewState(ViewState newState)
        {
            autoAttackChargingBar.gameObject.SetActive(false);
            autoAttackCooldownBar.gameObject.SetActive(false);
            autoAttackHaltedBar.gameObject.SetActive(false);
            switch (newState)
            {
                case ViewState.CHARGING:
                    autoAttackChargingBar.gameObject.SetActive(true);
                    break;
                case ViewState.COOLINGDOWN:
                    autoAttackCooldownBar.gameObject.SetActive(true);
                    break;
                case ViewState.IDLE:
                    autoAttackHaltedBar.gameObject.SetActive(true);
                    break;
            }

            Debug.Log("AA Entering: " + newState);
        }
    }
}