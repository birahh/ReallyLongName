using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPCheatCodeController : MonoBehaviour
{
    public string[] CheatCodes;
    public string[] CallFunctions;

    private string[] TypedKeys;

    void Start()
    {
        ClearTypedKeys();
    }
	
	void Update ()
    {
        if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1)) {

            AddKey(Input.inputString);
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
    
    void CallFunction(int index)
    {
        //  Call funcion with the name of CallFunctions[index]
        print(CallFunctions[index]);
    }

    void SearchForMatch()
    {
        //  For each CheatCode
        for (int i = 0; i < CheatCodes.Length; i++) {

            int wordLenght = 0;

            //  Check if a recorded key
            for (int j = 0; j < TypedKeys.Length; j++) {

                //  Matches with a first letter of a word
                for (int k = 0; k < CheatCodes[i].Length; k++) {
                
                    //  If it Does
                    if (CheatCodes[i].Substring(k, 1).Equals(TypedKeys[j])) {

                        //  Check if the rest of the word matches with the following characters typed
                        for(int l = 0; l < CheatCodes[i].Length; l++) {

                            //  Then check if is not out of boundaries
                            if(k + l > CheatCodes[i].Length - 1 || j + l > TypedKeys.Length - 1) {
                                break;
                            }

                            //  If is not out of boundaries do the count of characters that matches the word
                            if(CheatCodes[i].Substring(k + l, 1).Equals(TypedKeys[j + l])) {

                                wordLenght++;

                                //  If the size of the word matches it
                                if(wordLenght == CheatCodes[i].Length) {

                                    //  You've found the word
                                    PrintCheatWorld(CheatCodes[i]);
                                    CallFunction(i);
                                    ClearTypedKeys();
                                    wordLenght = 0;

                                    return;
                                }
                            } else {
                                wordLenght = 0;
                                break;
                            }
                        }

                    } else {
                        wordLenght = 0;
                        break;
                    }      
                }
            }
        }
    }

    void PrintCheatWorld(string word)
    {
        //print(word);
    }
}
