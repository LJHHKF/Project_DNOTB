using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShaking : MonoBehaviour
{
    [SerializeField] private float shakePower = 0.05f;
    [SerializeField] private float shakeTime = 1.0f;
    private Vector3 InitPos;
    // Start is called before the first frame update
    void Start()
    {
        ResetInitPos();
    }

    public float ShakeOn()
    {
        StartCoroutine(Shake());
        return shakeTime;
    }

    public void ResetInitPos()
    {
        InitPos = gameObject.transform.position;
    }

    private IEnumerator Shake()
    {
        float _time = shakeTime;
        while(_time > 0)
        {
            Vector3 shakePos = Random.insideUnitCircle * shakePower;

            transform.position = InitPos + shakePos;
            _time -= Time.deltaTime;
            yield return null;
        }
        transform.position = InitPos;
        yield break;
    }
}
