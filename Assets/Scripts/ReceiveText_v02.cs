// Using this script as of: 2019/04/05 - Richard

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ReceiveText_v02 : MonoBehaviour {
	public GetText Receiver_Flask02;
	// Use this for initialization
	string prevString = "";
	string[] rhymeTexts = new string[9];
	string emptyStringArr = ",,,,,,,,";
	public Sprite Square;
	public Sprite Oval;
	Transform books;
	void Awake() {
		books = transform.GetChild(0);
		Debug.Log("There are " + transform.GetChild(0).childCount + " books");
	}
	void Start () {
		for(int i = 0; i < rhymeTexts.Length; i++)
		{
			rhymeTexts[i] = "";
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Condition: do something if the python string is not empty
		if(Receiver_Flask02.PythonString != null || Receiver_Flask02.PythonString != emptyStringArr)
		{
			// Further condition: if the string is different from previous string, do something.
			if(Receiver_Flask02.PythonString != prevString)
			{
				// Do something for every text children
				for(int i = 0; i < books.childCount; i++)
					{
						// Debug.Log("child " + i);
						// Debug.Log(Receiver_Flask02.PythonStrings[i]);

						// Texts Appear
						if(rhymeTexts[i] == "" && Receiver_Flask02.PythonStrings[i] != "")
						{
							books.GetChild(i).gameObject.GetComponent<Image>().sprite = Square;
							books.GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State",2);
						}
						//Texts Change
						if(rhymeTexts[i] != "" && rhymeTexts[i] != Receiver_Flask02.PythonStrings[i])
						{
							books.GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State",3);
						}
						//Texts Disappear
						if(rhymeTexts[i] != "" && Receiver_Flask02.PythonStrings[i] == "")
						{
							books.GetChild(i).gameObject.GetComponent<Image>().sprite = Square;
							books.GetChild(i).gameObject.GetComponent<Animator>().SetInteger("State",4);
						}

						//Latter
						rhymeTexts[i] = Receiver_Flask02.PythonStrings[i];
						books.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Receiver_Flask02.PythonStrings[i];
						
					}
			}
			// Register the new string as the previous string before checking the next loop
			prevString = Receiver_Flask02.PythonString;
		}
	}

	public string[] splitPythonList(string str)
	{
		string strClean = str.Replace("[","").Replace("]","");
		string[] strS = new string[10];
		strS = strClean.Split(',');
		return strS;
	}
}