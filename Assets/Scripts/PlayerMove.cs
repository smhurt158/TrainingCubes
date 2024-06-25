using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;

    public float Score;
    [SerializeField] private GameObject _scoreBoard;
    [SerializeField] private float _fSpeed, _sSpeed, _totalSpeed;
    public bool Left, Right;
    public enum Control
    {
        AI,
        Player,
    }
    [SerializeField] public Control control;

    private float _sideMove;
    public event Action OnPreAIMove;   
    


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(control == Control.Player)
        {
            _sideMove = 0;
            if (Input.GetKey("left")) _sideMove -= _sSpeed * _totalSpeed;
            if (Input.GetKey("right")) _sideMove += _sSpeed * _totalSpeed;
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = Vector3.zero;

        if (control == Control.AI)
        {
            if (OnPreAIMove != null) OnPreAIMove();
            _sideMove = 0;
            if (Left) _sideMove -= _sSpeed * _totalSpeed;
            if (Right) _sideMove += _sSpeed * _totalSpeed;
        }

        float speedMult = _fSpeed * Score / 200000 + _fSpeed * 3/4 ;
        float trueSpeed = _fSpeed > speedMult ? _fSpeed : speedMult;
        //Debug.Log("Mult: " + speedMult + ", fspeed: " + _fSpeed + ", %: " + (trueSpeed - _fSpeed) * 100 / _fSpeed + "%");

        rb.velocity += new Vector3(_sideMove, 0, trueSpeed * _totalSpeed);
        
        Score += 1 * _fSpeed * _totalSpeed / 100;
        //if(rb.position.x < -15 || rb.position.x > 15 || Math.Abs(rb.position.x) < .0001 ) Score -= 1 * _fSpeed * _totalSpeed / 200;

        _scoreBoard.GetComponent<Text>().text = ((int)Score).ToString();
        if (Score >= 100000)
            {
                if(transform.position.z < 160000)
                {
                    GetComponent<PlayerHealth>().TakeDamage();
                    GetComponent<PlayerHealth>().TakeDamage();
                    GetComponent<PlayerHealth>().TakeDamage();
                    GetComponent<PlayerHealth>().TakeDamage();

            }
            Application.Quit();
            }

    }
}
