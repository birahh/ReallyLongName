using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPCheatCodeController : MonoBehaviour
{
    public string[] CheatCodes;

    private string[] TypedKeys;

    void Start()
    {
        ClearTypedKeys();
    }
	
	void Update ()
    {
        if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1)) {

            AddKey(Input.inputString);
            PrintKeyArray();
        }	    	
	}

    void AddKey(string keyPressed)
    {
        if (TypedKeys.Length > 0) {

            string[] tempTypedKeys = TypedKeys;

            TypedKeys = new string[tempTypedKeys.Length + 1];

            for (int i = 0; i < tempTypedKeys.Length; i++) {
                TypedKeys[i] = tempTypedKeys[i];
            }

            TypedKeys[tempTypedKeys.Length] = keyPressed;

            SearchForMatch();
        }
    }
    
    void ClearTypedKeys()
    {
        TypedKeys = new string[1];
        TypedKeys[0] = "Begin--";
    }

    void SearchForMatch()
    {
        int wordLenght = 0;

        for (int i = 0; i < CheatCodes.Length; i++) {

            for (int j = 0; j < TypedKeys.Length; j++) {

                for (int k = 0; k < CheatCodes[i].Length; k++) {
                
                    if (CheatCodes[i].Substring(k, 1).Equals(TypedKeys[j])) {
                        
                        wordLenght++;

                        j = k + 1;

                        print(wordLenght+"@");

                        if (k == CheatCodes[i].Length - 1) {

                            //  Found Word
                            PrintCheatWorld(CheatCodes[i]);
                            ClearTypedKeys();
                            wordLenght = 0;
                        }

                    } else {
                        wordLenght = 0;
                    }                        
                }
            }
        }
    }

    void PrintKeyArray()
    {
        Debug.ClearDeveloperConsole();

        foreach (string key in TypedKeys) {
            Debug.LogError(key);
        }
    }

    void PrintCheatWorld(string word)
    {
        print(word);
    }
}
