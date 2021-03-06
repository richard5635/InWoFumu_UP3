﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Linq;

public class SerialLight : MonoBehaviour
{

    public SerialHandler serialHandler;//シリアルハンドラを指定
    public int message_int = 0;
                                       //public Text text;
    bool ReadyToPush = true;

    // Image Resources
    public Sprite Square;//背景のしかく

    // Start with a number, assign keyboard numbers to the board / book
    // Start is called before the first frame update
    //csv処理
    public string phrase_csv;//csvの名前（個々のデータ）
    public string phrase2phrase;//csvの名前（対応)
    public TextAsset csvFile;//csvファイル
    public TextAsset csvFile2;
    public List<string[]> csvDatas = new List<string[]>();//CSVの中身をいれるリスト
    public List<string[]> csvDatas2 = new List<string[]>();//CSVの中身をいれるリスト

    public int height = 0;//CSVの行数の初期化

    // Audio Material
    public AudioSource AS;
    public List<AudioClip> sounds = new List<AudioClip>();

    // InWoFumu mats
    [SerializeField] List<int> idForEach = new List<int> { 0, 2, 0, 0, 0, 0, 0 };
    [SerializeField] List<int> linkIdForEach = new List<int> { 0, 4, 0, 0, 0, 0, 0 };//params[9]候補になるフレーズがその本に描かれる

    int steppedPhraseID;
    int steppedTableID;
    List<int> previousId = new List<int> { 0, 0, 0, 4 };

    public List<List<int>> place_dict = new List<List<int>> {
        new List<int>{1,2,3},
        new List<int>{0,3,4},
        new List<int>{0,3,5},
        new List<int>{0,1,2,4,5,6},
        new List<int>{1,3,6},
        new List<int>{2,3,6},
        new List<int>{3,4,5},
        };

    public FrontDisplayController FrontDisplay;

    // Interaction Variables
    int[] linked_phrase_FD = new int[2]; // Linked phrase info for front display
    int prevID = 999;
    // List<int> linked_phrase_FD = new List<int>();
    public GameObject[] Books = new GameObject[7];

    // Use this for initialization
    void Awake()
    {
        serialHandler.OnDataReceived += OnDataReceived;
        // Do reading of csv files
        // Define csv file names
        // phrase_csv = "phrase_csv";
        phrase2phrase = "phrase2phrase_ver10_kai";

        // Process of moving csv data into List
        //phrase_csv to csvDatas
        // csvFile = Resources.Load("CSV/" + phrase_csv) as TextAsset;
        // StringReader reader = new StringReader(csvFile.text);
        // while (reader.Peek() > -1)
        // {
        //     string line = reader.ReadLine();
        //     csvDatas.Add(line.Split(','));
        //     height++;
        // }
        // Debug.Log("csvDatas.Count is" + csvDatas.Count);

        //phrase2phrase to csvDatas2
        csvFile2 = Resources.Load("CSV/" + phrase2phrase) as TextAsset;
        StringReader reader2 = new StringReader(csvFile2.text);
        while (reader2.Peek() > -1)
        {
            string line = reader2.ReadLine();
            csvDatas2.Add(line.Split(','));
            height++;
        }

        csvDatas2.RemoveAt(0);
        Debug.Log("csvDatas2[0][0]=" + csvDatas2[0][0]);

        // Test if the table has contents
        Debug.Log(csvDatas2[9][6]);

        //音処理
        AS = gameObject.AddComponent<AudioSource>();
        //csvからsoundsに格納
        Debug.Log("Audio file name: " + csvDatas2[1][16]);
        for (int i = 1; i < csvDatas2.Count; i++)
        {
            // Debug.Log("sound name is" + csvDatas[i][16]);            
            // AudioClip audio = Resources.Load("Sound/" + csvDatas[i][6].Remove(csvDatas[i][6].Length - 4), typeof(AudioClip)) as AudioClip;
            AudioClip audio = Resources.Load("Sound/" + csvDatas2[i][17], typeof(AudioClip)) as AudioClip;//phrase2phraseのidから音源をとった場合
            sounds.Add(audio);
            // Debug.Log(sounds[i-1]);
        }
    }

    void Start() // Might change to onEnable
    {
        // Start the initial level. 2?
        // Display First Phrase
        
        for (int i = 0; i < linkIdForEach.Count; i++)
        {
            Books[i].GetComponent<BookShaderController>().InitializeMat03Bg();
            if (linkIdForEach[i] != 0)
            {
                steppedPhraseID = linkIdForEach[i];
                transform.GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = Square;
                transform.GetChild(0).GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State", 2);
                Books[i].GetComponent<BookShaderController>().Mat03State(2);
                transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = csvDatas2[linkIdForEach[i]][10];
                FrontDisplay.AcceptPhrase(csvDatas2[steppedPhraseID][3], csvDatas2[steppedPhraseID][6], csvDatas2[steppedPhraseID][7], csvDatas2[steppedPhraseID][8]);
            
                // AudioClip audio = Resources.Load("Sound/" + csvDatas2[i][0], typeof(AudioClip)) as AudioClip;//phrase2phraseのidから音源をとった場合
                // sounds.Add(audio);
        
            }
        }
    }

    /*
	 * シリアルを受け取った時の処理
	 */
    void OnDataReceived(string message)
    {
        try
        {
            //シリアルで送られてきた番号
            message_int = int.Parse(message);
            if(!FrontDisplay.isBusy)SteppedOn(message_int);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    void SteppedOn(int num)
    {
        List<int> linkTableId = new List<int>();

        Debug.Log("Stepped on book" + num + ".");

        // Check the number on the book
        if (linkIdForEach[num] == 0) return;

        // steppedID = idForEach[num];
        steppedPhraseID = linkIdForEach[num];//phrase2phraseのidから取得する場合
        Debug.Log("Stepped phrase ID = " + steppedPhraseID);

        // Make sure the objects are designated previously thru inspector
        Books[num].GetComponent<BookShaderController>().Mat03Behavior();

        // Play sound
        AS.PlayOneShot(sounds[steppedPhraseID - 1]); // maybe remove -1?
        // AS.PlayOneShot(sounds[0]); // maybe remove -1?

        // Check related phrases, access 
        // Makes a link between stepped phrase and candidates
        // List<List<int>> next_phrases = new List<List<int>>();//{{次表示するフレーズのID(phrase_csv),それを出すために参照したphrase2phrase_csvのID},{,},,,}
        string next_string = csvDatas2[steppedPhraseID][22].Trim(new char[] { '"' });
        List<int> nextPhraseIds = new List<int>();
        Debug.Log("next_string=" + next_string);
        nextPhraseIds = next_string.Split(':').ToList().ConvertAll(int.Parse);
        //nextPhraseIds.Add(int.Parse(next_string.Trim);
        HashSet<int> hsPrev = new HashSet<int>(previousId);
        for (int i = nextPhraseIds.Count - 1; i >= 0; i--)
        {
            if (previousId.Contains(int.Parse(csvDatas2[nextPhraseIds[i]][9])))
            {
                nextPhraseIds.Remove(nextPhraseIds[i]);
            }
        }
        //nextPhraseIds.RemoveAll(s=>csvDatas2[s][10].Exists(p => previousId[p].Contains));

        //ランダム抽出の準備
        // System.Random r = new System.Random((int)DateTime.Now.Ticks);
        // int tableID;
        //本1冊ずつ順番に
        // for (int i = 0; i < idForEach.Count; i++)
        for (int i = 0; i < linkIdForEach.Count; i++)
        {
            string displayText = "";
            //背景のアニメーション
            // if (idForEach[i] != 0)//さっき表示されてた
            if (linkIdForEach[i] != 0)//さっき表示されてた
            {
                if (place_dict[num].Contains(i))//次もだす, does processing to book locations that are designated by the list place_dict. If 1 is stepped, do something on surrounding mats
                {
                    //idForEachの更新
                    //idForEach[i] = next_phrases[r.Next(1, next_phrases.Count)][0];
                    if (nextPhraseIds.Count > 0)
                    {
                        int j = Random.Range(0, nextPhraseIds.Count);
                        // tableID = FindTableID(next_phrases[j][0]);
                        displayText = csvDatas2[nextPhraseIds[j]][10]; // should get the ID of the next phrase
                        // idForEach[i] = next_phrases[j][0];
                        Debug.Log("Displaying the ID: " + nextPhraseIds[j]);
                        linkIdForEach[i] = nextPhraseIds[j];
                        nextPhraseIds.RemoveAt(j);

                        transform.GetChild(0).GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State", 3);
                        Books[i].GetComponent<BookShaderController>().Mat03State(3);
                        Debug.Log(i + ":change");

                        Debug.Log("rand:" + j);
                    }
                    else
                    {
                        transform.GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = Square;
                        transform.GetChild(0).GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State", 4);
                        Books[i].GetComponent<BookShaderController>().Mat03State(4);
                        Debug.Log(i + ":dissappear");
                        // idForEach[i] = 0;
                        linkIdForEach[i] = 0;
                    }

                    Debug.Log("rand : " + Random.Range(0, nextPhraseIds.Count));
                }
                else//次はださない、つまり消す
                {
                    transform.GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = Square;
                    transform.GetChild(0).GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State", 4);
                    Books[i].GetComponent<BookShaderController>().Mat03State(4);
                    Debug.Log(i + ":dissappear");
                    // idForEach[i] = 0;
                    linkIdForEach[i] = 0;
                }

            }
            else if (place_dict[num].Contains(i))//さっきなくて新しく出す
            {
                if (nextPhraseIds.Count > 0)
                {
                    int j = Random.Range(0, nextPhraseIds.Count);
                    // tableID = FindTableID(next_phrases[j][0]);
                    displayText = csvDatas2[nextPhraseIds[j]][10];
                    // Debug.Log("Table ID: "+ tableID);
                    // displayText = csvDatas2[tableID][9];
                    // idForEach[i] = next_phrases[j][0];
                    linkIdForEach[i] = nextPhraseIds[j];
                    nextPhraseIds.RemoveAt(j);

                    transform.GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = Square;
                    transform.GetChild(0).GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State", 2);
                    Books[i].GetComponent<BookShaderController>().Mat03State(2);
                    Debug.Log(i + ":appear");

                    Debug.Log("rand:" + j);
                }

                // idForEach[i] = next_phrases[r.Next(1, next_phrases.Count)][0];
            }

            //文字の処理
            //idForEachが０じゃなければ、つまり次に出すものがあれば
            //ランダムにnext_phraseの1列目のidから
            if (linkIdForEach[i] != 0)
            {
                // this one probably must search
                // transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = csvDatas[idForEach[i]][1];
                transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = displayText;
            }
        }

        // FrontDisplay.AcceptPhrase(csvDatas[steppedID][2], csvDatas[steppedID][1], csvDatas[steppedID][5], csvDatas[steppedID][4]);

        // Handle Text Coloring

        // linked_phrase_FD[0] = steppedTableID;
        // linked_phrase_FD[1] = prevID;
        // prevID = steppedTableID;
        // Debug.Log(linked_phrase_FD[0]+ " and "+linked_phrase_FD[1]);

        //string[] rhymePos = FindRhymePositions(linked_phrase_FD[0], linked_phrase_FD[1]);

        //Definition in FrontDisplayController.cs:AcceptPhrase(string kana, string kanji, string author, string title, string rhyme_pos_c, string rhyme_pos_p)
        string[] rhymePos = { csvDatas2[steppedPhraseID][5], csvDatas2[steppedPhraseID][13] };//phrase,linked
        FrontDisplay.AcceptPhrase(csvDatas2[steppedPhraseID][11], csvDatas2[steppedPhraseID][14], csvDatas2[steppedPhraseID][15], csvDatas2[steppedPhraseID][16], rhymePos[1], rhymePos[0]);

        previousId.RemoveAt(0);
        previousId.Add(int.Parse(csvDatas2[steppedPhraseID][9]));

        // Send the signal to the FrontDisplay.
        // Need to make info about connection between current and past
        // if(rhymePos == null) {
        //     Debug.Log("No coloring");
        //     FrontDisplay.AcceptPhrase(csvDatas2[steppedTableID][2], csvDatas[steppedTableID][1], csvDatas[steppedTableID][6], csvDatas[steppedTableID][7]);
        // }
        // else {
        //     Debug.Log("With coloring.");
        //     FrontDisplay.AcceptPhrase(
        //     csvDatas[steppedTableID][2], csvDatas2[steppedTableID][1], csvDatas[steppedTableID][6], csvDatas[steppedTableID][7],
        //     rhymePos[0], rhymePos[1]
        //     );
        // }
        // Debug.Log(ShowListContentsInTheDebugLog(idForEach));
        Debug.Log(ShowListContentsInTheDebugLog(linkIdForEach));

        // Display 
        return;
    }

    void UpdateGroundDisplay()
    {
        return;
    }


    void RestartGame()
    {

        return;
    }
    //リストのデバッグ
    public string ShowListContentsInTheDebugLog<T>(List<T> list)
    {
        string log = "";

        foreach (var content in list)
        {
            log += content.ToString() + ", ";
        }

        return log;
    }
}
