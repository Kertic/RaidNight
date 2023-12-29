using System;
using UnityEngine;

namespace Code.Entity.Buffs
{
    public class Buff
    {
        public Action m_onBuffExpire;

        protected Entity m_entity;
        protected Sprite m_buffIcon;
        protected float m_duration;

        private float _timeStarted;

        public Buff(Entity affectedEntity, float duration, Sprite buffIcon)
        {
            m_entity = affectedEntity;
            m_duration = duration;
            m_buffIcon = buffIcon;
        }

        public virtual void OnBuffEnter(BuffView[] buffViews)
        {
            foreach (BuffView buffView in buffViews)
            {
                _timeStarted = Time.time;
                buffView.SetImage(m_buffIcon);
                buffView.SetIconText(m_duration.ToString("0.0"));
            }
        }

        public virtual void OnBuffExit(BuffView[] buffViews)
        {
            foreach (BuffView buffView in buffViews)
            {
                buffView.SetImage(null);
                buffView.SetIconText("");
            }

            m_onBuffExpire?.Invoke();
        }

        public virtual void OnBuffUpdate(BuffView[] buffViews)
        {
            float timeRemaining = GetTimeRemaining();

            foreach (BuffView buffView in buffViews)
            {
                buffView.SetIconText(timeRemaining <= 30.0f ? GetTimeRemaining().ToString("0.0") : "");
            }

            if (GetTimeRemaining() <= 0.0f)
            {
                OnBuffExit(buffViews);
            }
        }

        public float GetTimeRemaining()
        {
            return (_timeStarted + m_duration) - Time.time;
        }
    }
}