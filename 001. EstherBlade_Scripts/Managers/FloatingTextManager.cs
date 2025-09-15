using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextData
{
    public GameObject GameObject;
    public RectTransform RectTransform;
    public CanvasGroup CanvasGroup;
    public float ElapsedTime;
}

public class FloatingTextManager : MonoBehaviour
{
    public Canvas canvas;
    public GameObject textPrefab;
    public float floatSpeed = 30f;
    public float duration = 2f;

    private List<FloatingTextData> floatingTexts = new List<FloatingTextData>();

    private void Update()
    {
        for (int i = floatingTexts.Count - 1; i >= 0; i--)
        {
            FloatingTextData textData = floatingTexts[i];
            textData.ElapsedTime += Time.deltaTime;

            textData.RectTransform.anchoredPosition += Vector2.up * floatSpeed * Time.deltaTime;

            textData.CanvasGroup.alpha = Mathf.Lerp(1f, 0f, textData.ElapsedTime / duration);

            if (textData.ElapsedTime >= duration)
            {
                Destroy(textData.GameObject);
                floatingTexts.RemoveAt(i);
            }
        }
    }


    public void CreateFloatingText(string message, Vector3 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        GameObject floatingText = Instantiate(textPrefab, canvas.transform);
        floatingText.transform.position = screenPosition;

        TextMeshProUGUI text = floatingText.GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = message;
        }

        RectTransform rectTransform = floatingText.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = floatingText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = floatingText.AddComponent<CanvasGroup>();
        }

        floatingTexts.Add(new FloatingTextData
        {
            GameObject = floatingText,
            RectTransform = rectTransform,
            CanvasGroup = canvasGroup,
            ElapsedTime = 0f,
        });
    }

}
