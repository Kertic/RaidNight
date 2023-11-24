using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Systems.Views
{
    public class SkillBarUIView : MonoBehaviour
    {
        [SerializeField]
        private List<SkillIconUIView> skillImages;


        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }

        public void SetImages(Sprite[] sprites)
        {
            for (int i = 0; i < skillImages.Count && i < sprites.Length; i++)
            {
                skillImages[i].SetSprite(sprites[i]);
            }
        }
    }
}