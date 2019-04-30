using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrontDisplayController : MonoBehaviour
{
    // Group the csv info to array.
    // Start with arrray first.
    // string [][] PhraseList = new string[4][]; 
    string[] PhraseStepped = new string[4];
    public List<string[]> PhraseList = new List<string[]>();
    public List<GameObject> PhraseObjects = new List<GameObject>();
    List<GameObject> DispPhraseObjects = new List<GameObject>();

    public TextAsset csvFile;//csvファイル
    public TextAsset csvFile2;
    public GameObject PhraseTemplate;
    GameObject phraseObject;

    // Phrase Properties
    string c_kana;
    string c_kanji;
    string c_author;
    string c_title;

    // Rhyme Positions
    string rhymePos_c;
    string rhymePos_p;

    // Phrase Display Positions
    int xPos = 978;
    int yPos = -300;
    int xSpan = 32;

    // Movement Parameters
    float xMovement = 10.0f;
    Coroutine coroutine;
    public AnimationCurve AnimationCurve;
    // Highlight Parameters
    bool isHighlighted = false;
    public Color32 HighlightColor;
    [HideInInspector] public bool isBusy = false;
    [SerializeField] Color32[] HighLightColors = new Color32[4];
    int count = 0;


    private void Awake()
    {
        // Assign 4 indices to each array.

    }
    // public void AcceptPhrase(string kana, string kanji, string author, string title)
    public void AcceptPhrase(string kana, string kanji, string author, string title, string rhyme_pos_c, string rhyme_pos_p)
    {
        isBusy = true;
        isHighlighted = true;
        // Save values
        c_kana = kana;
        c_kanji = kanji;
        c_author = author;
        c_title = title;

        // Assign Rhyme Positions
        rhymePos_c = rhyme_pos_c;
        rhymePos_p = rhyme_pos_p;

        // Slide phrases
        Debug.Log("PhraseObjects length = " + PhraseObjects.Count);
        if (PhraseObjects.Count >= 4) coroutine = StartCoroutine(SlidePhrases(0.25f));
        else coroutine = StartCoroutine(UpdateTexts());

        // Make IEnumerator to display the list, find a way to sync the list and the instantiated obejcts

        // Save info to GameObject Children
    }

    public void AcceptPhrase(string kana, string kanji, string author, string title)
    {
        isBusy = true;
        isHighlighted = false;

        // Save values
        c_kana = kana;
        c_kanji = kanji;
        c_author = author;
        c_title = title;

        // Assign Rhyme Positions
        // rhymePos_c = rhyme_pos_c;
        // rhymePos_p = rhyme_pos_p;

        // Slide phrases
        Debug.Log("PhraseObjects length = " + PhraseObjects.Count);
        if (PhraseObjects.Count >= 4) coroutine = StartCoroutine(SlidePhrases(0.1f));
        else coroutine = StartCoroutine(UpdateTexts());

        // Make IEnumerator to display the list, find a way to sync the list and the instantiated obejcts

        // Save info to GameObject Children
    }

    void HighLightPhrases(string pos, string pos_p)
    {


    }

    IEnumerator SlidePhrases(float duration)
    {
        Debug.Log("Sliding Phrase");
        float t = 0;
        float curvePercent = 0;
        float res = 0.01f;
        while (t < duration)
        {
            t += res;
            for (int i = 0; i < PhraseObjects.Count; i++)
            {
                // Slide with curve speed
                // Refer to https://medium.com/@rhysp/lerping-with-coroutines-and-animation-curves-4185b30f6002

                float percent = Mathf.Clamp01(t / duration);
                curvePercent = AnimationCurve.Evaluate(percent);

                // Slide by x amounts from current position
                Vector2 origin = PhraseObjects[i].GetComponent<RectTransform>().anchoredPosition;
                Vector2 target = PhraseObjects[i].GetComponent<RectTransform>().anchoredPosition + new Vector2(xSpan, 0);
                PhraseObjects[i].GetComponent<RectTransform>().anchoredPosition = Vector2.LerpUnclamped(origin, target, curvePercent);
            }
            yield return new WaitForSeconds(res);
        }
        coroutine = StartCoroutine(UpdateTexts());
        yield return null;
    }

    IEnumerator UpdateTexts()
    {
        // Slide
        // Delete excess
        if (PhraseObjects.Count == 4)
        {
            Destroy(PhraseObjects[0]);
            PhraseObjects.Remove(PhraseObjects[0]);
        }

        // Add text
        PhraseObjects.Add(Instantiate(PhraseTemplate, transform));
        PhraseObjects[PhraseObjects.Count - 1].transform.SetAsFirstSibling();
        PhraseObjects[PhraseObjects.Count - 1].name = c_kanji;

        PhraseObjects[PhraseObjects.Count - 1].GetComponent<RectTransform>().anchoredPosition = new Vector2(
            PhraseObjects[PhraseObjects.Count - 1].GetComponent<RectTransform>().anchoredPosition.x - (xSpan * 8 * (PhraseObjects.Count-1)),
            PhraseObjects[PhraseObjects.Count - 1].GetComponent<RectTransform>().anchoredPosition.y
        );

        for (int i = 0; i < PhraseObjects[PhraseObjects.Count - 1].transform.childCount; i++)
        {
            if (i == 0) PhraseObjects[PhraseObjects.Count - 1].transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = c_kana;
            // Make special command for kanji.
            else if (i == 1)
            {
                ExtendText(c_kanji, PhraseObjects[PhraseObjects.Count - 1].transform.GetChild(i).gameObject, 20);
                // PhraseObjects[PhraseObjects.Count-1].transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = c_kanji;
            }
            else if (i == 2) PhraseObjects[PhraseObjects.Count - 1].transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = c_author;
            else if (i == 3) 
            {
                ExtendText(c_title, PhraseObjects[PhraseObjects.Count - 1].transform.GetChild(i).gameObject, 6);
                // PhraseObjects[PhraseObjects.Count - 1].transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = c_title;
            }
        }
        // Make one Phrase GameObject and place values inside.
        // phraseObject = Instantiate(PhraseTemplate, transform);
        // phraseObject.transform.SetAsFirstSibling();// Maybe need to instantiate?
        // phraseObject.GetComponent<RectTransform>().position = new Vector2(xPos, yPos);
        // for (int i = 0; i < phraseObject.transform.childCount; i++)
        // {
        //     if (i == 0) phraseObject.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = c_kana;
        //     else if (i == 1) phraseObject.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = c_kanji;
        //     else if (i == 2) phraseObject.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = c_author;
        //     else if (i == 3) phraseObject.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = c_title;
        // }
        // Removes content if list is 4 or more

        // Save to List of GameObjects
        // PhraseObjects.Add(phraseObject);
        // PhraseObjects.Insert(0, phraseObject);

        // Bool Highlight
        if (isHighlighted)
        {
            coroutine = StartCoroutine(HighLightPhrasesI(rhymePos_c, rhymePos_p));
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            isBusy = false;
        }
        yield return null;
    }

    void ExtendText(string s, GameObject textObject, int maxChar)
    {
        int charCount = s.Length;
        Debug.Log("There are " + charCount + " characters in the sentence");
        int arrIndexCount = charCount / maxChar;
        string[] sArr = new string[arrIndexCount+1];
        GameObject cloneObject = textObject;

        // Divide string per 20 characters.
        for (int i = 0; i < sArr.Length; i++)
        {
            if (s.Length - ((i) * maxChar) > maxChar) sArr[i] = s.Substring(i * maxChar, maxChar);
            else sArr[i] = s.Substring(i * maxChar, s.Length - ((i) * maxChar));
            Debug.Log("string sArr index value: " + sArr[i]);
        }

        textObject.GetComponent<TextMeshProUGUI>().text = sArr[0];

        if (sArr.Length > 1)
        {
            for (int i = 1; i < sArr.Length; i++)
            {
                cloneObject = Instantiate(textObject, textObject.transform.position, Quaternion.identity, textObject.transform.parent);
                cloneObject.GetComponent<TextMeshProUGUI>().text = sArr[i];
                cloneObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    textObject.GetComponent<RectTransform>().anchoredPosition.x - ((i) * textObject.GetComponent<RectTransform>().sizeDelta.x),
                    textObject.GetComponent<RectTransform>().anchoredPosition.y
                );
            }
        }
        return;
    }

    IEnumerator HighLightPhrasesI(string pos, string pos_p)
    {

        string[] pos_Index = pos.Split('-');
        string[] posp_Index = pos_p.Split('-');
        int[] pos_i = new int[2];
        int[] posp_i = new int[2];

        pos_i[0] = int.Parse(pos_Index[0]);
        pos_i[1] = int.Parse(pos_Index[1]);
        posp_i[0] = int.Parse(posp_Index[0]);
        posp_i[1] = int.Parse(posp_Index[1]);

        // Remove highlight of the previous pair


        // Highlight the latest phrase and one phrase before it.
        // Random color but the bright ones. Need to use a preset?
        Color32 c0 = GetColorPreset();
        yield return new WaitForSeconds(0.1f);
        // Reason: color changes immediately if moved.
        ColorText(PhraseObjects[PhraseObjects.Count - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>(), pos_i[0], pos_i[1], c0);
        ColorText(PhraseObjects[PhraseObjects.Count - 2].transform.GetChild(0).GetComponent<TextMeshProUGUI>(), posp_i[0], posp_i[1], c0);


        Debug.Log("Done animating the phrase");
        yield return new WaitForSeconds(0.2f);
        isBusy = false;
        StopCoroutine(coroutine);
        yield return null;
    }

    void MoveTexts()
    {
        return;
    }

    Color32 GetColorPreset()
    {
        Color32 c;

        // c = new Color32((byte)Random.Range(25, 255), (byte)Random.Range(25, 255), (byte)Random.Range(25, 255), 255);
        c = HighLightColors[count % 4];
        count++;

        return c;
    }

    void ColorText(TextMeshProUGUI tm, int from, int to, Color32 c)
    {
        Debug.Log("Coloring from: " + from + " to " + to);
        TMP_TextInfo textInfo = tm.textInfo;
        int currentCharacter = 0;

        Color32[] newVertexColors;


        for (int i = from; i <= to; i++)
        {
            currentCharacter = i;

            int characterCount = textInfo.characterCount;


            // Get the index of the material used by the current character.
            int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

            // Get the vertex colors of the mesh used by this text element (character or sprite).
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

            // Only change the vertex color if the text element is visible.
            if (textInfo.characterInfo[currentCharacter].isVisible)
            {
                newVertexColors[vertexIndex + 0] = c;
                newVertexColors[vertexIndex + 1] = c;
                newVertexColors[vertexIndex + 2] = c;
                newVertexColors[vertexIndex + 3] = c;

                // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
                tm.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                // This last process could be done to only update the vertex data that has changed as opposed to all of the vertex data but it would require extra steps and knowing what type of renderer is used.
                // These extra steps would be a performance optimization but it is unlikely that such optimization will be necessary.
            }
        }
        Debug.Log("Done Coloring " + tm.text);
        return;
    }
}
