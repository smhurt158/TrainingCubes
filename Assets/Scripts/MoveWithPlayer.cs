using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] public int ObjectNum;
    [SerializeField] private float _firstTrigger, _moveDistance, _delta;
    private float _target;

    public event Action OnObjectMove;  

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        _target = _firstTrigger + ObjectNum * _delta;
    }

    // Update is called once per frame
    void Update()
    {
        float playerZ = _player.transform.position.z;
        if(playerZ > _target)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + _moveDistance);
            _target += _moveDistance;
            if (OnObjectMove != null) OnObjectMove();

        }
    }
}
