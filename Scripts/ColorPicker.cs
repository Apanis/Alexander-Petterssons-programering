using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

[Serializable]
public class ColorEvent : UnityEvent<Color>{}
public class ColorPicker : MonoBehaviour
{
    RectTransform Rect;
    Texture2D ColorTexture;
    public ColorEvent OnColorPreview;
    public ColorEvent OnColorSelect;
    public static ColorPicker instance;
    public Color color;
    public Outline PreviewOutline;
    public Outline PreviewOutline2;
    public Image PreviewCrosshair;
    public Image PreviewCrosshair2;
    public static bool HasChosenColor;

    void Start()
    {
        HasChosenColor = false;
        PreviewOutline = PreviewCrosshair.GetComponent<Outline>();
        PreviewOutline2 = PreviewCrosshair2.GetComponent<Outline>();
        
        instance = this;
        Rect = GetComponent<RectTransform>();

        ColorTexture = GetComponent<Image>().mainTexture as Texture2D;
    }

    
    void Update()
    {
        
        if(RectTransformUtility.RectangleContainsScreenPoint(Rect, Input.mousePosition))
        {
            Vector2 delta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Rect, Input.mousePosition, null, out delta);

            float width = Rect.rect.width;
            float height = Rect.rect.height;
            delta += new Vector2(width * .5f, height * .5f);
    

            float x = Mathf.Clamp(delta.x / width, 0, 1);
            float y = Mathf.Clamp(delta.y / height, 0, 1);

            
            int texX = Mathf.RoundToInt(x * ColorTexture.width);
            int texY = Mathf.RoundToInt(y * ColorTexture.height);


            color = ColorTexture.GetPixel(texX, texY);
            

            OnColorPreview?.Invoke(color);
            PreviewOutline.effectColor = color;
            if(Input.GetMouseButtonDown(0))
            {
                OnColorSelect?.Invoke(color);
                PreviewOutline2.effectColor = color;
                HasChosenColor = true;
            }
            
        }
    }
}
