using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGate : MonoBehaviour
{
    private static System.Random r = new System.Random();


    [SerializeField] private GameObject Player, Obstacle1, Obstacle2, Obstacle3, Obstacle4;
    private static int NumObstacles = 2;
    private GameObject[] Obstacles = new GameObject[4];
    private bool _reset = false;
    public int[] ObstacleStates = new int[4];
    

    private void Awake()
    {
        int seed = DateTime.Now.Millisecond;
        r = new System.Random(seed);
        Obstacles[0] = Obstacle1;
        Obstacles[1] = Obstacle2;
        Obstacles[2] = Obstacle3;
        Obstacles[3] = Obstacle4;
        GetComponent<MoveWithPlayer>().OnObjectMove += ChangeGate;
    }
    // Start is called before the first frame update
    void Start()
    {

        
        ChangeGate();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeGate()
    {
        for (int i = 0; i < 4; i++)
        {
            Obstacles[i].SetActive(false);
            ObstacleStates[i] = 0;

        }
        for (int i = 0; i < NumObstacles; i++)
        {
            int obs = r.Next(4);
            Obstacles[obs].SetActive(true);
            ObstacleStates[obs] = 1;
            //Might create less than NumObstacles obstacles but not more
        }
    }
}
