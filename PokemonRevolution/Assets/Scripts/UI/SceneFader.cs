using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image _image;

    public IEnumerator FadeIn(float durationInSeconds)
    {
        Color startColor = new Color(0, 0, 0, 0);
        Color targetColor = Color.black;
        for (float t = 0; t < durationInSeconds; t += Time.deltaTime)
        {
            float normalizedTime = t / durationInSeconds;
            _image.color = Color.Lerp(startColor, targetColor, normalizedTime);
            yield return null;
        }
        _image.color = targetColor;
    }

    public IEnumerator FadeOut(float durationInSeconds)
    {
        Color startColor = Color.black;
        Color targetColor = new Color(0, 0, 0, 0);
        for (float t = 0; t < durationInSeconds; t += Time.deltaTime)
        {
            float normalizedTime = t / durationInSeconds;
            _image.color = Color.Lerp(startColor, targetColor, normalizedTime);
            yield return null;
        }
        _image.color = targetColor;
    }
}
