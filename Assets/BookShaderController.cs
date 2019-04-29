using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookShaderController : MonoBehaviour
{
    public int index = 0;
    Material mat;
    Coroutine coroutine;
    public AnimationCurve AnimationCurve;
    public float BeatIntensity = 1.2f;
    float defBrightness = 0.0f;
    float b = 0;
    void Start()
    {
        mat = GetComponent<Image>().material;
        mat.SetFloat("_OverallBrightness", defBrightness);
        b = defBrightness;
    }

    void Update()
    {
        Mat03Beat();
    }

    void Mat03Beat()
    {
        b = defBrightness + AudioPeer._freqBand[index] * BeatIntensity;
        mat.SetFloat("_OverallBrightness", b);
    }

    public void Mat03Behavior()
    {
        if(coroutine!=null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(TriggerTouch(0.3f));
    }

    IEnumerator TriggerTouch(float duration)
    {
        float t= 0;
        float pulseValue = 0;
        float percent = 0;
        float speed = (1 / duration);
        float curvePercent = 0;
        float res = 0.01f;

        while(t <= duration)
        {
            t += res;
            pulseValue += res * speed;
            percent = Mathf.Clamp01(pulseValue / 1);
            curvePercent = AnimationCurve.Evaluate(percent);

            mat.SetFloat("_PulseValue", curvePercent); 
            yield return new WaitForSeconds(res);
        }
    }
}
