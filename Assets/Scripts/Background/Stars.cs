using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField]
    private GameObject _stars1, _stars2, _starsBig1, _starsBig2;
    [SerializeField]
    private float _starsSpeed = 2.0f;
    [SerializeField]
    private float _starsBigSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        StarsStartPosition();
        StarsBigStartPosition();
    }

    // Update is called once per frame
    void Update()
    {
        StarsMovment();
        StarsBigMovment();
    }

    private void StarsBigStartPosition()
    {
        _starsBig1.transform.position = new Vector3(0, 12, -0.65f);
        _starsBig2.transform.position = new Vector3(0, 52, -0.65f);
    }

    private void StarsBigMovment()
    {
        _starsBig1.transform.Translate(Vector3.down * _starsBigSpeed * Time.deltaTime);

        if (_starsBig1.transform.position.y < -24.23f)
        {
            _starsBig1.transform.position = new Vector3(0, 51, -0.65f);
        }

        _starsBig2.transform.Translate(Vector3.down * _starsBigSpeed * Time.deltaTime);

        if (_starsBig2.transform.position.y < -24.23f)
        {
            _starsBig2.transform.position = new Vector3(0, 51, -0.65f);
        }
    }

    private void StarsStartPosition()
    {
        _stars1.transform.position = new Vector3(0, 12, -1);
        _stars2.transform.position = new Vector3(0, 52, -1);
    }

    private void StarsMovment()
    {
        _stars1.transform.Translate(Vector3.down * _starsSpeed * Time.deltaTime);

        if (_stars1.transform.position.y < -24.23f)
        {
            _stars1.transform.position = new Vector3(0, 51, 3);
        }

        _stars2.transform.Translate(Vector3.down * _starsSpeed * Time.deltaTime);

        if (_stars2.transform.position.y < -24.23f)
        {
            _stars2.transform.position = new Vector3(0, 51, 3);
        }
    }
}
