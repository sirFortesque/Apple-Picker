using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class example : MonoBehaviour {

    public Dictionary<string,string> statesDict;

    void Start()
    {
        statesDict = new Dictionary<string, string>();

        statesDict.Add("MD", "Maryland");
        statesDict.Add("TX", "Texas");
        statesDict.Add("PA", "Pennsylvania");
        statesDict.Add("CA", "California");
        statesDict.Add("MI", "Michigan");
        print("There are " + statesDict.Count + " elements in statesDict.");

        foreach (KeyValuePair<string, string> kvp in statesDict)
        {
            print(kvp.Key + ": " + kvp.Value);
        }

        print("MI is " + statesDict["MI"]);

        statesDict["BC"] = "British Columbia";
        foreach (string k in statesDict.Keys)
        {
            print(k + " is " + statesDict[k]);
        }
    }
}
