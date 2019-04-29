using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderController : MonoBehaviour
{
    public GameObject GameMat01;
    float mTIntensity = 0;
    bool keyPressed = false;
    Coroutine coroutine;
    public AnimationCurve AnimationCurve;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mat02Behavior();
        Mat03Behavior();
        
    }

    void Mat02Behavior()
    {
        if(Input.GetKey(KeyCode.K))TriggerMaterial02();
        ResetMaterial02();
    }

    void Mat03Behavior()
    {
        if(Input.GetKey(KeyCode.K)){
            if(coroutine!=null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(TriggerTouch(0.3f));
        }
    }

    void TriggerMaterial02()
    {
        mTIntensity = Mathf.Clamp(mTIntensity + 0.4f, 0, 2);
        GameMat01.GetComponent<MeshRenderer>().material.SetFloat("_TouchIntensity", mTIntensity);

    }

    void ResetMaterial02()
    {   
        if(mTIntensity > 0) 
        {
            mTIntensity = Mathf.Clamp(mTIntensity - 0.1f, 0, 2);
            GameMat01.GetComponent<MeshRenderer>().material.SetFloat("_TouchIntensity", mTIntensity);  
        } 
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

            GameMat01.GetComponent<Image>().material.SetFloat("_PulseValue", curvePercent); 
            yield return new WaitForSeconds(res);
        }
    }
}
