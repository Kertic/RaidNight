using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Player.StateMachines.PlayerWeaponStates
{
    public class PlayerCastView : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform chargingBar, cooldownBar, disabledBar, background;

        [SerializeField]
        protected TextMeshProUGUI words;

        public enum ViewState
        {
            CHARGING,
            COOLINGDOWN,
            IDLE,
            HIDDEN,
            NUMOFVIEWSTATES
        }

        public void SetProgress(float progress)
        {
            progress = Math.Clamp(progress, 0.0f, 1.0f);
            cooldownBar.sizeDelta = disabledBar.sizeDelta = chargingBar.sizeDelta = (background.sizeDelta * Vector2.right * progress) + (Vector2.up * background.sizeDelta);
        }

        public void ChangeViewState(ViewState newState)
        {
            chargingBar.gameObject.SetActive(false);
            cooldownBar.gameObject.SetActive(false);
            disabledBar.gameObject.SetActive(false);
            background.gameObject.SetActive(true);
            words.gameObject.SetActive(true);
            switch (newState)
            {
                case ViewState.CHARGING:
                    chargingBar.gameObject.SetActive(true);
                    break;
                case ViewState.COOLINGDOWN:
                    cooldownBar.gameObject.SetActive(true);
                    break;
                case ViewState.IDLE:
                    disabledBar.gameObject.SetActive(true);
                    break;
                case ViewState.HIDDEN:
                    background.gameObject.SetActive(false);
                    words.gameObject.SetActive(false);
                    break;
            }
        }

        public void SetText(string text)
        {
            words.text = text;
        }
    }
}