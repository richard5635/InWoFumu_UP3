using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

// Steps:
/* 
    Reference to csv: check CSV folder
    1. Decide the stepping
    2. Find how to access csv then try to just copy
    3. Insert change text function
    4. Insert Front Display Commands
*/

public class DebugModeController : MonoBehaviour
{
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
    int steppedID;
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

    void Awake()
    {
        // Do reading of csv files
        // Define csv file names
        phrase_csv = "phrase_csv";
        phrase2phrase = "phrase2phrase_csv";

        // Process of moving csv data into List
        //phrase_csv to csvDatas
        csvFile = Resources.Load("CSV/" + phrase_csv) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
            height++;
        }
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

        //音処理
        AS = gameObject.AddComponent<AudioSource>();
        //csvからsoundsに格納
        for (int i = 1; i < csvDatas.Count; i++)
        {
            // Debug.Log("sound name is" + csvDatas[i][6]);            
            AudioClip audio = Resources.Load("Sound/" + csvDatas[i][6].Remove(csvDatas[i][6].Length - 4), typeof(AudioClip)) as AudioClip;
            sounds.Add(audio);
            // Debug.Log(sounds[i-1]);
        }
    }

    void Start() // Might change to onEnable
    {
        // Start the initial level. 2?

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if(!FrontDisplay.isBusy)SteppedOn(0);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            if(!FrontDisplay.isBusy)SteppedOn(1);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            if(!FrontDisplay.isBusy)SteppedOn(2);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            if(!FrontDisplay.isBusy)SteppedOn(3);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            if(!FrontDisplay.isBusy)SteppedOn(4);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            if(!FrontDisplay.isBusy)SteppedOn(5);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if(!FrontDisplay.isBusy)SteppedOn(6);
        }
    }

    void SteppedOn(int num)
    {
        Debug.Log("Stepped on book" + num + ".");

        // Check the number on the book
        if(idForEach[num] == 0) return;
        steppedID = idForEach[num];

        // Play sound
        AS.PlayOneShot(sounds[steppedID - 1]); // maybe remove -1?

        // Check related phrases, access 
        // Makes a link between stepped phrase and candidates
        List<List<int>> next_phrases = new List<List<int>>();//{{次表示するフレーズのID(phrase_csv),それを出すために参照したphrase2phrase_csvのID},{,},,,}
        for (int i = 1; i < csvDatas2.Count; i++)
        {
            //phrase2phraseの2行目phrase_idに今踏まれたものがあったら
            if (int.Parse(csvDatas2[i][1]) == steppedID)
            {
                List<int> id_phrase_link = new List<int> { int.Parse(csvDatas2[i][6]), int.Parse(csvDatas2[i][0]) };
                next_phrases.Add(id_phrase_link);

                // Send signal to FrontDisplayController

            }
        }

        Debug.Log(ShowListContentsInTheDebugLog(next_phrases));
        //ランダム抽出の準備
        // System.Random r = new System.Random((int)DateTime.Now.Ticks);

        //本1冊ずつ順番に
        for (int i = 0; i < idForEach.Count; i++)
        {

            //背景のアニメーション
            if (idForEach[i] != 0)//さっき表示されてた
            {
                if (place_dict[num].Contains(i))//次もだす
                {
                    //idForEachの更新
                    //idForEach[i] = next_phrases[r.Next(1, next_phrases.Count)][0];
                    if (next_phrases.Count > 0)
                    {
                        int j = Random.Range(0, next_phrases.Count);
                        idForEach[i] = next_phrases[j][0];
                        next_phrases.RemoveAt(j);

                        transform.GetChild(0).GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State", 3);
                        Debug.Log(i + ":change");

                        Debug.Log("rand:" + j);
                    }
                    else
                    {
                        transform.GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = Square;
                        transform.GetChild(0).GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State", 4);
                        Debug.Log(i + ":dissappear");
                        idForEach[i] = 0;
                    }

                    // Debug.Log("rand : " + Random.Range(0,next_phrases.Count));
                }
                else//次はださない、つまり消す
                {
                    transform.GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = Square;
                    transform.GetChild(0).GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State", 4);
                    Debug.Log(i + ":dissappear");
                    idForEach[i] = 0;
                }

            }
            else if (place_dict[num].Contains(i))//さっきなくて新しく出す
            {
                if (next_phrases.Count > 0)
                {
                    int j = Random.Range(0, next_phrases.Count);
                    idForEach[i] = next_phrases[j][0];
                    next_phrases.RemoveAt(j);

                    transform.GetChild(0).GetChild(i).gameObject.GetComponent<Image>().sprite = Square;
                    transform.GetChild(0).GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State", 2);
                    Debug.Log(i + ":appear");

                    Debug.Log("rand:" + j);
                }

                // idForEach[i] = next_phrases[r.Next(1, next_phrases.Count)][0];
            }

            //文字の処理
            //idForEachが０じゃなければ、つまり次に出すものがあれば
            //ランダムにnext_phraseの1列目のidから
            if (idForEach[i] != 0)
            {
                transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = csvDatas[idForEach[i]][1];
            }
        }

        // FrontDisplay.AcceptPhrase(csvDatas[steppedID][2], csvDatas[steppedID][1], csvDatas[steppedID][5], csvDatas[steppedID][4]);

        // Handle Text Coloring

        linked_phrase_FD[0] = steppedID;
        linked_phrase_FD[1] = prevID;
        prevID = steppedID;
        Debug.Log(linked_phrase_FD[0]+ " and "+linked_phrase_FD[1]);

        string[] rhymePos = FindRhymePositions(linked_phrase_FD[0], linked_phrase_FD[1]);

        // Send the signal to the FrontDisplay.
        // Need to make info about connection between current and past
        if(rhymePos == null) {
            Debug.Log("No coloring");
            FrontDisplay.AcceptPhrase(csvDatas[steppedID][2], csvDatas[steppedID][1], csvDatas[steppedID][5], csvDatas[steppedID][4]);
        }
        else {
            Debug.Log("With coloring.");
            FrontDisplay.AcceptPhrase(
            csvDatas[steppedID][2], csvDatas[steppedID][1], csvDatas[steppedID][5], csvDatas[steppedID][4],
            rhymePos[0], rhymePos[1]
            );
        }
        Debug.Log(ShowListContentsInTheDebugLog(idForEach));

        // Display 
        return;
    }

    void UpdateGroundDisplay()
    {
        return;
    }

    public string ShowListContentsInTheDebugLog<T>(List<T> list)
    {
        string log = "";

        foreach (var content in list)
        {
            log += content.ToString() + ", ";
        }

        return log;
    }

    string[] FindRhymePositions(int curr, int prev)
    {
        // List search agorithm, can be improved if slow.
        if (curr == 999 || prev == 999) return null; // 999 is the initial value, meaning that it is empty

        string[] pos = new string[2];
        // Debug.Log("Test: " + int.Parse(csvDatas2[4][1]) + " and " + int.Parse(csvDatas2[5][6]));
        for (int i = 1; i < csvDatas2.Count; i++)
        {
            for (int j = 1; j < csvDatas2.Count; j++)
            {
                // Debug.Log("Compare: " + int.Parse(csvDatas2[i][1])+ "|" + curr + " and " + int.Parse(csvDatas2[j][6])+ "|" + prev);
                if (int.Parse(csvDatas2[i][1]) == curr)
                {
                    if (int.Parse(csvDatas2[j][6]) == prev)
                    {
                        pos[0] = csvDatas2[i][5];
                        pos[1] = csvDatas2[i][10];
                        Debug.Log("Found: " + pos[0] + " and " + pos[1]);
                        return pos;
                    }
                }
            }
        }
        Debug.Log("No rhyme pos found");
        return null;
    }

    void RestartGame()
    {

        return;
    }
}
