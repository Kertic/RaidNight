using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Entity.Player.Views.FaeArcher
{
    public class WispView : MonoBehaviour
    {

        [SerializeField]
        private SpriteRenderer wispSprite;

        // Start is called before the first frame update
        void Start()
        {
       
        }

        // Update is called once per frame
        void Update() { }

        public void ChangeColor(Color color)
        {
            wispSprite.color = color;
        }
    }
}