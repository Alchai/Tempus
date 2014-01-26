using UnityEngine;
using System.Collections;

public class AlphaFade : MonoBehaviour
{


    public void FadeOut()
    {
        StopCoroutine("fadeOut");
        StartCoroutine("fadeOut");
    }

    private IEnumerator fadeOut()
    {
        print("fading out");
        while (renderer.material.color.a > .01f)
        {
            Color newcol = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, renderer.material.color.a - .05f);
            renderer.material.color = newcol;
            yield return new WaitForEndOfFrame();
        }

        renderer.material.color = Color.clear;
    }
}
