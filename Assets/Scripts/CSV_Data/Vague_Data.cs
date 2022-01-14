using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]

public struct VagueData
{
    public int vague;
    public int ennemyByLine;
    public int NbOfLine;
    public int Tank;
    public int Fire;
    public int Pinata;
    public int TotalSpec;
    public int Total;
}

public class Vague_Data : MonoBehaviour
{
    public string path = "Resources/CSV/vague.csv";
    public List<VagueData> Data = new List<VagueData>();

    void Awake()
    {
        ReadCSVFile();
    }

    void ReadCSVFile()
    {
        var myText = Resources.Load<TextAsset>("CSV/vague");
        bool endOfFile = false;
        bool skipHeader = true;
        foreach (var data_String in myText.text.Split('\n'))
        {
            if (data_String == null)
            {
                endOfFile = true;
                break;
            }
            if (skipHeader)
            {
                skipHeader = false;
                continue;
            }
            string[] data_string_value = data_String.Split(';');
            VagueData data_value;
            data_value.vague = int.Parse(data_string_value[0]);
            data_value.ennemyByLine = int.Parse(data_string_value[1]);
            data_value.NbOfLine = int.Parse(data_string_value[2]);
            data_value.Tank = int.Parse(data_string_value[3]);
            data_value.Fire = int.Parse(data_string_value[4]);
            data_value.Pinata = int.Parse(data_string_value[5]);
            data_value.TotalSpec = int.Parse(data_string_value[6]);
            data_value.Total = int.Parse(data_string_value[7]);
            Data.Add(data_value);
        }

    }
}
