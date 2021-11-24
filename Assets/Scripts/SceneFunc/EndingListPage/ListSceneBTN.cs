using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListSceneBTN : MonoBehaviour
{
    [SerializeField] private int buttonIndex;
    private MyEndings.EndingIndex endingIndex;
    private readonly int onePageNum = 4;
    [SerializeField] private Image myImage;
    
    private bool isOn;
    private Button m_button;
    // Start is called before the first frame update
    void Start()
    {
        m_button = GetComponent<Button>();

        EndingListPageScene.instance.ev_pageChange += MatchingUpdate; // 싱글턴이지만 전역 유지되는 놈은 아니고 같은 씬일때만 유지되니 해제해줄 필요는 따로 없음.

        MatchingUpdate();
    }

    public void OnClick()
    {
        EndingListPageScene.instance.EndingRepeatWindowSetActive(true);
        ListSceneBoxMain.instance.SetEndingState(endingIndex);
    }

    private void MatchingUpdate()
    {
        endingIndex = (MyEndings.EndingIndex)(buttonIndex + (EndingListPageScene.instance.curPage * onePageNum));
        int temp = 0;
        switch (endingIndex)
        {
            case MyEndings.EndingIndex.first:
                if (DataRWManager.instance.mySaveData_event.TryGetValue("end01", out temp))
                {
                    if (temp == 0) isOn = false;
                    else isOn = true;
                }
                else
                {
                    isOn = false;
                    DataRWManager.instance.InputDataValue("end01", 0, DataRWManager.instance.mySaveData_event);
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
                    isOn = false;
                    DataRWManager.instance.InputDataValue("end02", 0, DataRWManager.instance.mySaveData_event);
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
                    isOn = false;
                    DataRWManager.instance.InputDataValue("end03", 0, DataRWManager.instance.mySaveData_event);
                }
                break;
            case MyEndings.EndingIndex.fourth:
                if (DataRWManager.instance.mySaveData_event.TryGetValue("end04", out temp))
                {
                    if (temp == 0) isOn = false;
                    else isOn = true;
                }
                else
                {
                    isOn = false;
                    DataRWManager.instance.InputDataValue("end04", 0, DataRWManager.instance.mySaveData_event);
                }
                break;
            case MyEndings.EndingIndex.fifth:
                if (DataRWManager.instance.mySaveData_event.TryGetValue("end05", out temp))
                {
                    if (temp == 0) isOn = false;
                    else isOn = true;
                }
                else
                {
                    isOn = false;
                    DataRWManager.instance.InputDataValue("end05", 0, DataRWManager.instance.mySaveData_event);
                }
                break;
            case MyEndings.EndingIndex.sixth:
                if (DataRWManager.instance.mySaveData_event.TryGetValue("end06", out temp))
                {
                    if (temp == 0) isOn = false;
                    else isOn = true;
                }
                else
                {
                    isOn = false;
                    DataRWManager.instance.InputDataValue("end06", 0, DataRWManager.instance.mySaveData_event);
                }
                break;
            case MyEndings.EndingIndex.seventh:
                if (DataRWManager.instance.mySaveData_event.TryGetValue("end07", out temp))
                {
                    if (temp == 0) isOn = false;
                    else isOn = true;
                }
                else
                {
                    isOn = false;
                    DataRWManager.instance.InputDataValue("end07", 0, DataRWManager.instance.mySaveData_event);
                }
                break;
            case MyEndings.EndingIndex.eighth:
                if (DataRWManager.instance.mySaveData_event.TryGetValue("end08", out temp))
                {
                    if (temp == 0) isOn = false;
                    else isOn = true;
                }
                else
                {
                    isOn = false;
                    DataRWManager.instance.InputDataValue("end08", 0, DataRWManager.instance.mySaveData_event);
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
                else
                    myImage.sprite = null;
            }
        }
        else
        {
            myImage.sprite = null;
            m_button.enabled = false;
        }
    }
}
