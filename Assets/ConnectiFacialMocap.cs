using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Baku.VMagicMirrorConfig;

public class ConnectiFacialMocap : MonoBehaviour
{
    public InputField input;
    public GameObject error;
    
    public void Connect() {
        error.SetActive(!NetworkEnvironmentUtils.SendIFacialMocapDataReceiveRequest(input.text));
    }
}
