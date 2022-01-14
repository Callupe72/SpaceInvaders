using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct LevelData
{
    public int level;
    public int xp;
    public float dommage;
    public float defense;
    public float speed;
}

public class Level_Data : MonoBehaviour
{
    public string path = "Resources/CSV/level_xp.csv";
    public List<LevelData> Data = new List<LevelData>();

    void Awake()
    {
        ReadCSVFile();
    }

    void ReadCSVFile()
    {
        StreamReader strReader = new StreamReader(Application.dataPath + "/" + "Resources/CSV/level_xp.csv");
        bool endOfFile = false;
        bool skipHeader = true;
        while (!endOfFile)
        {
            string data_String = strReader.ReadLine();
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
            LevelData data_value;
            data_value.level = int.Parse(data_string_value[0]);
            data_value.xp = int.Parse(data_string_value[1]);
            data_value.dommage = float.Parse(data_string_value[2]);
            data_value.defense = float.Parse(data_string_value[3]);
            data_value.speed = float.Parse(data_string_value[4]);
            Data.Add(data_value);
        }
    }
}
