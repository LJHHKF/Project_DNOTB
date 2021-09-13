using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public partial class DataRWManager : MonoBehaviour
{
    //서강 게임교육원의 한 수업서 '도관목' 선배분이 공유한 코드를 참고로 하여 작성. 수정하려 했으나 크게 수정할 부분이 없어서 유지.
    //윈도우즈 기준 세이브 파일 경로: C:/User/User(=사용자명)/Appdata(숨김폴더)/LocalLow/SogandEduGames(=본 프로젝트 회사명)/DNOTB(=작품명)

    private static DataRWManager m_instance;
    public static DataRWManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<DataRWManager>();
            return m_instance;
        }
    }

    public Dictionary<string, int> mySaveData_event = new Dictionary<string, int>(); //필요할 경우 밖에서라도 Add 하면 됨.
    private List<string> readList;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        ReadData("DNOTB_save_event.csv", mySaveData_event);
    }

    private void OnDestroy()
    {
        if(m_instance == this)
            m_instance = null;
    }

    private static string GetDataPathToFile(string _fileName)
    {
        return Application.persistentDataPath + "/" + _fileName;
    }

    public static void WriteData(string _filename, Dictionary<string, int> _saveData)
    {
        string path = GetDataPathToFile(_filename);
        FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs);

        foreach (KeyValuePair<string, int> items in _saveData)
            sw.WriteLine(items.Key + "," + items.Value);
        sw.Close();
        fs.Close();
    }

    public void ReadData(string _fileName, Dictionary<string, int> _readData)
    {
        string filePath = GetDataPathToFile(_fileName);
        bool isNew = false;

        if (File.Exists(filePath))
            readList = myReadData(filePath);
        else
        {
            readList = CreateNewSaveData(_fileName);
            isNew = true;
        }

        for (int i = 0; i < readList.Count; i+=2)
            _readData.Add(readList[i].ToString(), int.Parse(readList[i + 1]));

        if (isNew)
            WriteData(_fileName, _readData);
    }

    private List<string> myReadData(string _filePath)
    {
        FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);

        string source = "";
        string[] divsource;
        List<string> divList = new List<string>();

        source = sr.ReadLine();

        while (source != null)
        {
            divsource = source.Split(',');
            divList.Add(divsource[0]);
            divList.Add(divsource[1]);
            source = sr.ReadLine();
        }

        sr.Close();
        fs.Close();

        return divList;
    }

    private List<string> CreateNewSaveData(string _fileName)
    {
        TextAsset data = Resources.Load("SaveInitData/" + _fileName.Substring(0, _fileName.LastIndexOf('.')), typeof(TextAsset)) as TextAsset;
        if(data == null)
        {
            Debug.LogError("세이브 초기 데이터 찾지 못함");
            return null;
        }
        StringReader sr = new StringReader(data.text);

        string source = "";
        string[] divsource;
        List<string> divList = new List<string>();

        source = sr.ReadLine();

        while(source!=null)
        {
            divsource = source.Split(',');
            divList.Add(divsource[0]);
            divList.Add(divsource[1]);
            source = sr.ReadLine();
        }

        sr.Close();

        return divList;
    }
}

public partial class DataRWManager : MonoBehaviour
{
    // public Dictionary<string, int> mySaveData_event = new Dictionary<string, int>();
    // 윗 딕셔너리 변수와 관련한 함수들 추가 부분

    public void InputDataValue(string _key, int _value, Dictionary<string, int> _writeData)
    {
        if (_writeData.ContainsKey(_key))
            _writeData[_key] = _value;
        else
            _writeData.Add(_key, _value);
    }
}