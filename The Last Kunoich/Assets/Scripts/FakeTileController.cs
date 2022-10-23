using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTileController : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0,1)]
    public float transparency;
    public Renderer targetRenderer;
    public LayerMask layerMask;
    public float fadeDuration;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(layerMask == (layerMask | (1 << collider.gameObject.layer)))
        {
            SetTransparency(transparency);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(layerMask == (layerMask | (1 << collider.gameObject.layer)))
        {
            SetTransparency(1);
        }
    }

    void SetTransparency(float alpha)
    {
        StopCoroutine("FadeCoroutine");
        StartCoroutine("FadeCoroutine", alpha);

    }

    private IEnumerator FadeCoroutine(float fadeTo)
    {
        float time = 0;
        Color currentColor = targetRenderer.material.color;
        float startAlpha = targetRenderer.material.color.a;

        while (time < 1)
        {
            yield return new WaitForEndOfFrame();

            time += Time.deltaTime / fadeDuration;

            currentColor.a = Mathf.Lerp(startAlpha, fadeTo, time);
            targetRenderer.material.color = currentColor;

        }
    }
    
}
