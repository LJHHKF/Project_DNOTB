using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public partial class DataRWManager : MonoBehaviour
{
    //���� ���ӱ������� �� ������ '������' ������� ������ �ڵ带 ����� �Ͽ� �ۼ�. �����Ϸ� ������ ũ�� ������ �κ��� ��� ����.
    //�������� ���� ���̺� ���� ���: C:/User/User(=����ڸ�)/Appdata(��������)/LocalLow/SogandEduGames(=�� ������Ʈ ȸ���)/DNOTB(=��ǰ��)

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

    public Dictionary<string, int> mySaveData_event = new Dictionary<string, int>(); //�ʿ��� ��� �ۿ����� Add �ϸ� ��.
    public Dictionary<string, int> mySaveData_option = new Dictionary<string, int>();
    private List<string> readList;

    [SerializeField] private bool saveReset = false;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        if (!saveReset)
        {
            ReadData("DNOTB_save_event.csv", mySaveData_event);
            ReadData("DNOTB_save_option.csv", mySaveData_option);
        }
        else
        {
            readList = CreateNewSaveData("DNOTB_save_event.csv");
            for (int i = 0; i < readList.Count; i += 2)
                mySaveData_event.Add(readList[i].ToString(), int.Parse(readList[i + 1]));
            WriteData("DNOTB_save_event.csv", mySaveData_event);

            readList = CreateNewSaveData("DNOTB_save_option.csv");
            for (int i = 0; i < readList.Count; i += 2)
                mySaveData_option.Add(readList[i].ToString(), int.Parse(readList[i + 1]));
            WriteData("DNOTB_save_option.csv", mySaveData_option);
        }
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
            Debug.LogError("���̺� �ʱ� ������ ã�� ����");
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
    // �� ��ųʸ� ������ ������ �Լ��� �߰� �κ�

    public void InputDataValue(string _key, int _value, Dictionary<string, int> _writeData)
    {
        if (_writeData.ContainsKey(_key))
            _writeData[_key] = _value;
        else
            _writeData.Add(_key, _value);
    }

    public bool CheckDoneEndingCount_each(int _min)
    {
        int hit = 0;
        bool result = false;
        foreach (KeyValuePair<string, int> items in mySaveData_event)
        {
            if (items.Value > 0) hit += 1;
            if (hit >= _min)
            {
                result = true;
                break;
            }
        }
        return result;
    }

    public int CheckDoneEndingCount_all()
    {
        int hit = 0;
        foreach (KeyValuePair<string, int> items in mySaveData_event)
        {
            if (items.Value > 0) hit += 1;
        }
        return hit;
    }
}