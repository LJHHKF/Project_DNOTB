using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBoxImageManager : MonoBehaviour
{
    [Header("SpriteSet")]
    [SerializeField] private Sprite spr_end03;
    [SerializeField] private GameObject shaodw_end03;
    [SerializeField] private Sprite spr_end04;
    [SerializeField] private GameObject shadow_end04;
    [SerializeField] private Sprite spr_end05;

    [Header("Object Link")]
    [SerializeField] private GameObject obj_end04;
    [SerializeField] private GameObject obj_smoke;
    [SerializeField] private GameObject obj_planet;

    private SpriteRenderer m_sprR; 

    private void Awake()
    { 
        ResetEvent();

        MySceneName.SceneName _name = MySceneManager.instance.GetCurrentSceneName();
        if (_name == MySceneName.SceneName.MainGame)
            MainEventManager.instance.ev_Reset += ResetEvent;
        else if (_name == MySceneName.SceneName.EndingListPage)
            ListSceneBoxMain.instance.ev_endingListReset += ResetEvent;
    }

    private void OnDestroy()
    {
        MySceneName.SceneName _name = MySceneManager.instance.GetCurrentSceneName();
        if (_name == MySceneName.SceneName.MainGame && MainEventManager.instance != null)
            MainEventManager.instance.ev_Reset -= ResetEvent;
        else if (_name == MySceneName.SceneName.EndingListPage && ListSceneBoxMain.instance != null)
            ListSceneBoxMain.instance.ev_endingListReset -= ResetEvent;
    }

    private void ResetEvent()
    {
        GetComponent<SpriteRenderer>().sprite = null;
        obj_end04.SetActive(false);
        obj_smoke.SetActive(false);
        shaodw_end03.SetActive(false);
        shadow_end04.SetActive(false);

        StopAllCoroutines();
    }

    private void ChkAndFind_SprR() // Start나 Awake에서 찾는 방식에서 왠지 오류나서 그냥 쓸 때 마다 찾아 쓰도록 만듦.
    {
        if (m_sprR == null)
            m_sprR = GetComponent<SpriteRenderer>();
    }

    public void SetEnding(MyEndings.UnboxingType _end)
    {
        ChkAndFind_SprR();
        switch(_end)
        {
            case MyEndings.UnboxingType.first:
                m_sprR.sprite = null;
                obj_smoke.SetActive(false);
                obj_end04.SetActive(false);
                shaodw_end03.SetActive(false);
                shadow_end04.SetActive(false);
                obj_planet.SetActive(false);
                break;
            case MyEndings.UnboxingType.third_1:
                m_sprR.sprite = spr_end03;
                shaodw_end03.SetActive(true);

                obj_smoke.SetActive(false);
                obj_end04.SetActive(false);
                shadow_end04.SetActive(false);
                obj_planet.SetActive(false);
                break;
            case MyEndings.UnboxingType.third_2:
                m_sprR.sprite = spr_end03;
                shaodw_end03.SetActive(true);

                if (MySceneManager.instance.GetCurrentSceneName() == MySceneName.SceneName.EndingListPage)
                    StartCoroutine(SmokeOn(0.0f));
                else
                    obj_smoke.SetActive(false);
                obj_end04.SetActive(false);
                shadow_end04.SetActive(false);
                obj_planet.SetActive(false);
                break;
            case MyEndings.UnboxingType.fourth:
                m_sprR.sprite = spr_end04;
                obj_end04.SetActive(true);
                shadow_end04.SetActive(true);

                shaodw_end03.SetActive(false);
                obj_smoke.SetActive(false);
                obj_planet.SetActive(false);
                break;
            case MyEndings.UnboxingType.fifth:
                m_sprR.sprite = spr_end05;
                obj_planet.SetActive(true);

                obj_smoke.SetActive(false);
                obj_end04.SetActive(false);
                shaodw_end03.SetActive(false);
                shadow_end04.SetActive(false);
                break;
        }
    }

    public void SmokeAnimOn(float _delayTime)
    {
        StartCoroutine(SmokeOn(_delayTime));
    }

    private IEnumerator SmokeOn(float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);
        obj_smoke.SetActive(true);
        yield return new WaitForSeconds(1.0f); // 애니메이션 플레이 시간
        obj_smoke.SetActive(false);
    }
}
