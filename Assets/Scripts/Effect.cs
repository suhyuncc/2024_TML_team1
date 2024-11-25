using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    [SerializeField]
    private float _delay;

    private void OnEnable()
    {
        StartCoroutine("ON");
    }

    IEnumerator ON()
    {

        yield return new WaitForSeconds(_delay);

        StopCoroutine("ON");
        Destroy(gameObject);
    }
}
