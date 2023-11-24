using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconUIView : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private TextMeshPro cooldownText;

    [SerializeField]
    private RectTransform cooldownOverlay;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetProgress(float progressRatio)
    {
        cooldownOverlay.sizeDelta = iconImage.rectTransform.rect.size;
    }

    public void SetSprite(Sprite newSprite)
    {
        iconImage.sprite = newSprite;
    }
}
