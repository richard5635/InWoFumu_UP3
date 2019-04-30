using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookShaderController : MonoBehaviour
{
    public int index = 0;
    Material mat;
    Coroutine touchCoroutine;
    Coroutine bgCoroutine;
    public AnimationCurve AnimationCurve;
    public float BeatIntensity = 1.35f;
    float defBrightness = 0f;
    float b = 0;
    void Awake()
    {
        mat = GetComponent<Image>().material; 
        mat.SetFloat("_OverallBrightness", defBrightness);
    }

    void Start()
    {
        b = defBrightness;
    }

    void Update()
    {
        Mat03Beat();
    }

    public void InitializeMat03Bg()
    {
        mat.SetFloat("_OverallBrightness", 1);
    }

    void Mat03Beat()
    {
        b = defBrightness + AudioPeer._freqBand[index] * BeatIntensity;
        mat.SetFloat("_AVBrightness", b);
    }

    public void Mat03Behavior()
    {
        if(touchCoroutine!=null) StopCoroutine(touchCoroutine);
        touchCoroutine = StartCoroutine(TriggerTouch(0.3f));
    }

    public void Mat03State(int n)
    {
        if(n == 2) // Appear
        {
            if(bgCoroutine != null) StopCoroutine(bgCoroutine);
            bgCoroutine = StartCoroutine(BgDisappear()); // Since value of shader is minus, we think oppositely
        }
        else if(n == 3) // Change
        {
            if(bgCoroutine != null) StopCoroutine(bgCoroutine);
        }
        else if(n == 4) // Disappear
        {
            if(bgCoroutine != null) StopCoroutine(bgCoroutine);
            bgCoroutine = StartCoroutine(BgAppear());
        }
    }
    

    IEnumerator BgAppear()
    {
        float b = mat.GetFloat("_OverallBrightness");
        float target = 1.0f;
        float res = 0.02f;
        while( b < target)
        {
            b += res;
            mat.SetFloat("_OverallBrightness", b);
            yield return new WaitForSeconds(res/ 2);
        }
        yield return null;
    }

    IEnumerator BgDisappear()
    {
        float b = mat.GetFloat("_OverallBrightness");
        float target = 0f;
        float res = 0.02f;
        while( b > target)
        {
            b -= res;
            mat.SetFloat("_OverallBrightness", b);
            yield return new WaitForSeconds(res/ 2);
        }
        yield return null;
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
