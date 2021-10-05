using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayUIManager : MonoBehaviour
{
    [SerializeField]
    private Dropdown commandselect;
    [SerializeField]
    private InputField input_argument;
    [SerializeField]
    private List<string> commandlist;

    public string GetcommandfromUI() => commandlist[commandselect.value];

    public string GetInputdata() => input_argument.text;
}
