using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
//    [SerializeField] private Text[] arTxtRecItems;
    [SerializeField] private GameObject[] arRecItems;
    [SerializeField] private RawImage riAvatar;
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtRecord;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        txtName.text = "-----";
        txtRecord.text = "-----   -----";
        ViewLeaderboard("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ViewAvatar()
    {
        txtName.text = GameManager.Instance.currentPlayer.playerName;
        riAvatar.texture = GameManager.Instance.currentPlayer.photo;
        Debug.Log($"ViewAvatar => name={GameManager.Instance.currentPlayer.playerName}");
        ViewRecord();
    }

    private void ViewRecord()
    {
        if (Language.Instance.CurrentLanguage == "ru")
        {
            txtRecord.text = $"Óð.{GameManager.Instance.currentPlayer.maxLevel} Î÷:{GameManager.Instance.currentPlayer.totalScore}";
        }
        else
        {
            txtRecord.text = $"Lv.{GameManager.Instance.currentPlayer.maxLevel} Sc:{GameManager.Instance.currentPlayer.totalScore}";
        }
    }

    public void ViewLeaderboard(string strJson)
    {
        if (strJson == "")
        {
            Debug.Log("ViewLeaderboard strJson= <" + strJson + ">");
            for (int i = 0; i < arRecItems.Length; i++)
            {
                Text txtRecName = arRecItems[i].transform.GetChild(1).gameObject.GetComponent<Text>();
                Text txtRecScore = arRecItems[i].transform.GetChild(2).gameObject.GetComponent<Text>();
                txtRecName.text = "..............";
                txtRecScore.text = "";
            }
            return;
        }
        try
        {
            //Debug.Log("ViewLeaderboard => " + strJson);
            //PersonRecord[] data = JsonConvert.DeserializeObject<PersonRecord[]>(strJson);
            //PersonRecord[] data = JsonUtility.FromJson<PersonRecord[]>(strJson);
            PersonRecord[] data = GetDataFromJson(strJson);
            //Debug.Log("data=>" + data);
            //StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length && i < arRecItems.Length; i++)
            {
                Text txtRecName = arRecItems[i].transform.GetChild(1).gameObject.GetComponent<Text>();
                Text txtRecScore = arRecItems[i].transform.GetChild(2).gameObject.GetComponent<Text>();
                txtRecName.text = data[i].Name;
                txtRecScore.text = $"{data[i].Score}";

                //arTxtRecItems[i].text = $"{data[i]}";
                //Debug.Log("VL => " + data[i].ToString());
                //sb.Append($"{data[i]}\n");
            }
            //txtDescrLeader.text = sb.ToString();
            //Debug.Log("VL sb=" + sb.ToString());
        }
        catch
        {
            Text txtRecName = arRecItems[0].transform.GetChild(1).gameObject.GetComponent<Text>();
            txtRecName.text = Language.Instance.CurrentLanguage == "ru" ? "Îøèáêà" : "Error";
        }
        //panelLiders.SetActive(true);
    }

    private PersonRecord[] GetDataFromJson(string s)
    {
        List<PersonRecord> arr = new List<PersonRecord>();
        string[] ss = s.Split("{");
        for (int i = 1; i < ss.Length; i++)
        {
            int end = ss[i].LastIndexOf('}');
            //Debug.Log($"ss[i]={ss[i]} end={end}");
            string strJson = $"{ss[i].Substring(0, end)}";
            strJson = "{" + strJson + "}";
            //Debug.Log($"strJson={strJson}");
            PersonRecord pr = JsonUtility.FromJson<PersonRecord>(strJson);
            //Debug.Log($"pr={pr}");
            arr.Add(pr);
        }

        return arr.ToArray();
    }
}

[Serializable]
public class MyArrRecords
{
    public PersonRecord[] records { get; set; }
    public MyArrRecords() { }
    public override string ToString()
    {
        return $"Counts={records.Length}";
    }
}

[Serializable]
public class PersonRecord
{
    //public int Rank { get; set; }
    public int Rank;
    //public int Score { get; set; }
    public int Score;
    //public string Name { get; set; }
    public string Name;

    public PersonRecord() { }
    public PersonRecord(int r, int sc, string nm)
    {
        Rank = r;
        Score = sc;
        Name = nm;
    }
    public override string ToString()
    {
        //string nm = String.Format("{0,-25}", Name);
        //return $"{Rank:00} {nm} {Score}";
        return $"{Rank:00} {Name} {Score}";
    }
}

