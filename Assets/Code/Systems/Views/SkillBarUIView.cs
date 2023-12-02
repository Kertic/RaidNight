using System;
using System.Collections.Generic;
using Code.Entity.Buffs;
using Code.Entity.Player.Weapon;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Systems.Views
{
    public class SkillBarUIView : MonoBehaviour
    {
        [SerializeField]
        private List<SkillIconUIView> skillImages;

      

        public void SetSkillImages(Sprite[] sprites)
        {
            for (int i = 0; i < skillImages.Count && i < sprites.Length; i++)
            {
                skillImages[i].SetSprite(sprites[i]);
            }
        }

        public SkillIconUIView GetIconUIView(int index)
        {
            index = Math.Clamp(index, 0, skillImages.Count - 1);
            return skillImages?[index];
        }

        public int GetSkillCount()
        {
            return skillImages.Count;
        }
    }
}