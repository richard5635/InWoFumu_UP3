using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource _audioSource;
    public static float[] _samples = new float[512];
    public static float[] _freqBand = new float[8];
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
    }

    // Update is called once per frame
    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        // Seven Bands
        /*
            20 - 60 Hz
            60 - 250 Hz
            250 - 500 Hz
            500 - 2000 Hz
            2000 - 4000 Hz
            4000 - 6000 Hz
            6000 - 20000 Hz
        */

        /*
            0 - 2 = 86 Hz
            1 - 4 = 172 Hz : 87 - 258
            2 - 8 = 344 Hz : 259 - 602
            3 - 16 = 688 Hz : 603 - 1290
            4 - 32 = 1376 Hz : 1291 - 2666
            5 - 64 = 2752 Hz : 2667 - 5418
            6 - 128 = 5504 Hz : 5419 - 10922
            7 - 256 = 1108 Hz - 10923 - 21930
         */

        int count = 0;

        for (int i = 0; i < 7; i++)
        {
            float average = 0;

            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }

            average /= count;

            _freqBand[i] = average;
        }
        // Debug.Log(string.Format("Values: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", _freqBand[0], _freqBand[1], _freqBand[2], _freqBand[3], _freqBand[4], _freqBand[5], _freqBand[6], _freqBand[7]));
    }

    float MapInterval(float val, float srcMin, float srcMax, float dstMin, float dstMax)
    {
        if (val >= srcMax) return dstMax;
        if (val <= srcMin) return dstMin;
        return dstMin + (val - srcMin) / (srcMax - srcMin) * (dstMax - dstMin);
    }


}
