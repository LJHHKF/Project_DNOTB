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
        InitPos = gameObject.transform.position;
    }

    public void ShakeOn()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        while(shakeTime > 0)
        {
            Vector3 shakePos = Random.insideUnitCircle * shakePower;

            transform.position = InitPos + shakePos;
            shakeTime -= Time.deltaTime;
            yield return null;
        }
        transform.position = InitPos;
        yield break;
    }
}
