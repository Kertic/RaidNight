using System;
using Unity.VisualScripting;
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

        public virtual void OnBuffEnter(BuffView buffview)
        {
            _timeStarted = Time.time;
            buffview.SetImage(m_buffIcon);
            buffview.SetIconText(m_duration.ToString("0.0"));
        }

        public virtual void OnBuffExit(BuffView buffview)
        {
            buffview.SetImage(null);
            buffview.SetIconText("");
            m_onBuffExpire?.Invoke();
        }

        public virtual void OnBuffUpdate(BuffView buffview)
        {
            float timeRemaining = GetTimeRemaining();
            buffview.SetIconText(timeRemaining <= 30.0f ? GetTimeRemaining().ToString("0.0") : "");
            if (GetTimeRemaining() <= 0.0f)
            {
                OnBuffExit(buffview);
            }
        }

        public float GetTimeRemaining()
        {
            return (_timeStarted + m_duration) - Time.time;
        }
    }
}