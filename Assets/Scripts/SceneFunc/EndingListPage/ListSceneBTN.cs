using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListSceneBTN : MonoBehaviour
{
    [SerializeField] private MyEndings.EndingIndex endingIndex;
    [SerializeField] private Image myImage;
    
    private bool isOn;
    private Button m_button;
    // Start is called before the first frame update
    void Start()
    {
        m_button = GetComponent<Button>();

        int temp = 0;
        switch(endingIndex)
        {
            case MyEndings.EndingIndex.first:
                if (DataRWManager.instance.mySaveData_event.TryGetValue("end01", out temp))
                {
                    if (temp == 0) isOn = false;
                    else isOn = true;
                }
                else
                {
                    Debug.LogError("읽으려는 엔딩 파일에 대한 키가 없습니다.");
                }
                break;
            case MyEndings.EndingIndex.second:
                if (DataRWManager.instance.mySaveData_event.TryGetValue("end02", out temp))
                {
                    if (temp == 0) isOn = false;
                    else isOn = true;
                }
                else
                {
                    Debug.LogError("읽으려는 엔딩 파일에 대한 키가 없습니다.");
                }
                break;
            case MyEndings.EndingIndex.third:
                if (DataRWManager.instance.mySaveData_event.TryGetValue("end03", out temp))
                {
                    if (temp == 0) isOn = false;
                    else isOn = true;
                }
                else
                {
                    Debug.LogError("읽으려는 엔딩 파일에 대한 키가 없습니다.");
                }
                break;
            case MyEndings.EndingIndex.fourth:
                if (DataRWManager.instance.mySaveData_event.TryGetValue("end01", out temp))
                {
                    if (temp == 0) isOn = false;
                    else isOn = true;
                }
                else
                {
                    Debug.LogError("읽으려는 엔딩 파일에 대한 키가 없습니다.");
                }
                break;
        }


        if (isOn)
        {
            m_button.enabled = true;
            if (myImage != null)
            {
                Sprite temp2 = EndingListPageScene.instance.GetEndImageSprite(endingIndex);
                if (temp2 != null)
                    myImage.sprite = temp2;
            }
        }
        else
        {
            m_button.enabled = false;
        }
    }

    public void OnClick()
    {
        EndingListPageScene.instance.EndingRepeatWindowSetActive(true);
        ListSceneBoxMain.child_instance.SetEndingState(endingIndex);
    }
}
