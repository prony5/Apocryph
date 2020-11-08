using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class MessageAutoSize : MonoBehaviour
{
    public RectTransform container;
    public Text text;
    public float maxWidth = 1000;
    public Vector2 padding;

    private RectTransform _rt;

    private void Start()
    {
        _rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        var lines = text.text.Split('\n');
        var lineCount = lines.Length;
        float width = 0;
        float height = 0;

        foreach (var line in lines)
        {
            var h = CalculateLineHeight(line);
            if (height < h) height = h;

            float w = CalculateLineWidht(line);
            if (w > maxWidth)
            {
                lineCount += Mathf.FloorToInt(w / maxWidth);
                w = maxWidth;
            }

            if (width < w) width = w;
        }

        _rt.sizeDelta = new Vector2(width, lineCount * height * text.lineSpacing) + padding;
        if (container != null) container.sizeDelta = new Vector2(container.sizeDelta.x, _rt.sizeDelta.y);
    }

    int CalculateLineWidht(string line)
    {
        int totalLength = 0;
        foreach (var c in line)
        {
            text.font.GetCharacterInfo(c, out CharacterInfo characterInfo, text.fontSize);
            totalLength += characterInfo.advance;
        }

        return totalLength;
    }

    public float CalculateLineHeight(string line)
    {
        return text.cachedTextGeneratorForLayout.GetPreferredHeight(line, text.GetGenerationSettings(new Vector2(float.MaxValue, 0)));
    }
}
