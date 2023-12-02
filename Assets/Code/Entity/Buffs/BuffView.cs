using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Entity.Buffs
{
    public class BuffView : MonoBehaviour
    {
        [SerializeField]
        private Image buffIconRenderer;

        [SerializeField]
        private TextMeshProUGUI buffIconText;

        public void SetIconText(string newText)
        {
            buffIconText.text = newText;
        }

        public void SetImage(Sprite newSprite)
        {
            buffIconRenderer.sprite = newSprite;
        }
    }
}
