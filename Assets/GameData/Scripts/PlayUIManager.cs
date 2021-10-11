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
    private GameObject content;
    [SerializeField]
    private GameObject commandUIprefab;
    [SerializeField]
    private GameObject editcursorprefab;
    [SerializeField]
    private List<Color> colorlist;

    private GameObject cursorobj;

    private List<GameObject> contentUI;

    private Dictionary<string, string> actnamedata = new Dictionary<string, string>()
    {
        {"movefront","Ç‹Ç¶Ç…Ç¢Ç¡Ç€Ç∑Ç∑Çﬁ"},
        {"turnright","Ç›Ç¨Ç…Ç‹Ç™ÇÈ"},
        {"turnleft","Ç–ÇæÇËÇ…Ç‹Ç™ÇÈ"},
        {"forStart","nâÒÇ≠ÇËÇ©Ç¶Ç∑"},
        {"forEnd","Ç±Ç±Ç‹Ç≈"},
        {"IfStart","objÇ…Ç‘Ç¬Ç©Ç¡ÇΩÇÁ"},
        {"IfEnd","Ç±Ç±Ç‹Ç≈"}
    };

    [SerializeField]
    private List<string> commandlist;

    public string GetcommandfromUI() => commandlist[commandselect.value];

    public string GetInputdata() => input_argument.text;

    private void Awake()
    {
        cursorobj = content.transform.Find("editcursor").gameObject;
    }

    public void MoveeditCursor(int cursor)
    {
        if (cursorobj == null)
        {
            cursorobj = content.transform.Find("editcursor").gameObject;
        }
        cursorobj.transform.SetSiblingIndex(cursor);
    }

    public void executeCursorOn(int cursor)
    {
        content.transform.GetChild(cursor).Find("executecursor").gameObject.SetActive(true);
    }

    public void executeCursorOff(int cursor)
    {
        content.transform.GetChild(cursor).Find("executecursor").gameObject.SetActive(false);
    }

    public void DisplayCommandlist(List<ActionCommand> commandlist)
    {
        foreach (Transform child in content.gameObject.transform)
        {
            Destroy(child.gameObject);
            Debug.Log(child);
        }
        content.gameObject.transform.DetachChildren();

        cursorobj = Instantiate(editcursorprefab, content.transform);
        cursorobj.transform.SetParent(content.transform);

        foreach (ActionCommand action in commandlist)
        {
            GameObject commandUI = Instantiate(commandUIprefab, content.transform);
            commandUI.transform.SetParent(content.transform);
            //TextÇÃê›íË
            Text actiontext = commandUI.transform.Find("Text").GetComponent<Text>();
            actiontext.text = Getname(action);
            //BackGroundÇÃê›íË
            Image background = commandUI.transform.Find("background").GetComponent<Image>();
            Debug.Log(colorlist[action.scope % colorlist.Count]);
            background.color = colorlist[action.scope % colorlist.Count];

            //scopeèÍèäÇÃê›íË
            RectTransform textrect = commandUI.transform.Find("Text").GetComponent<RectTransform>();
            RectTransform imagerect = commandUI.transform.Find("background").GetComponent<RectTransform>();
            textrect.offsetMin = new Vector2(30.0f * Displaytab(action), 0.0f);
            imagerect.offsetMin = new Vector2(30.0f * Displaytab(action) - 2.0f, 0.0f);
        }
    }

    private string Getname(ActionCommand command)
    {
        string text = actnamedata[command.commandname];
        if (command.commandname == "forStart") text = text.Replace("n",((PassCommand)command).memodata.ToString());
        if (command.commandname == "IfStart") {
            if (((FrontCheckCommand)command).Getobjname() == "wall") text = text.Replace("obj","Ç©Ç◊");
            else if (((FrontCheckCommand)command).Getobjname() == "enemy") text = text.Replace("obj", "ÇƒÇ´");
            else text = text.Replace("obj", "Ç»Ç…Ç©"); ;
        }
        return text;
    }

    private int Displaytab(ActionCommand command)
    {
        if (command.commandname == "forStart" || command.commandname == "forEnd") return command.scope;
        if (command.commandname == "IfStart" || command.commandname == "IfEnd") return command.scope;
        return command.scope + 1;
    }

    public void Deleteeditcursor()
    {
        cursorobj.transform.SetAsLastSibling();
        cursorobj.SetActive(false);
    }

    public void Appeareditcursor(int cursor)
    {
        cursorobj.SetActive(true);
        MoveeditCursor(cursor);
    }
}
