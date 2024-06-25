using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NueralNetwork : MonoBehaviour
{
    System.Random rand = new System.Random();

    private PlayerMove pm;

    private GameObject _nextGate;
    private int[] ObstacleStates = new int[4];
    private float PlayerX, NextGateDistance, AdjustedPlayerX, AdjustedNextGateDistance;

    [SerializeField] public int HiddenLayers, HiddenLayerSize;
    private int InputLayerSize = 6, OutputLayerSize = 3;

    float e = 2.71f;

    public Specimen specimen;
    public int specNum;
    [SerializeField] GameObject NameTextBox;
    float[,] Theta1 = { { 0.6215922f, 0.2253166f, 0.4500462f, -2.051752f }, { -1.250393f, -0.3523979f, -0.03724077f, -1.959911f }, { 0.8559551f, 2.395807f, 2.138589f, 1.336384f }, { 0.4125115f, -1.292033f, -0.9466767f, -1.569708f }, { -0.8317609f, -1.362896f, -0.2185992f, 0.1479982f }, { -0.2987048f, -0.05722442f, 0.4448341f, -0.3568401f }, { -1.534093f, 3.904994f, -1.250599f, 0.9450316f } },
            Theta2 = {{ 0.04010926f,-0.0885206f,0.8053261f},{-1.449852f,1.021719f,0.2374997f},{0.5862512f,0.5749996f,0.2579336f},{1.136671f,-0.8062833f,-2.08227f},{0.2935668f,-0.2788708f,-0.07928254f } };
    public float[][,] Theta;

    private void Awake()
    {
        pm = GetComponent<PlayerMove>();
    }


    // Start is called before the first frame update
    public void Start()
    {
        NameTextBox.GetComponent<Text>().text = specNum + ": " +specimen.Name;
        //specimen = NNManager.Instance.CurrSpecimen;
        Theta = specimen.Weights;
        pm.OnPreAIMove += CalculateMove;
        NNManager.Instance.Player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        

    }

    private void CalculateMove()
    {
        //get and adjust inputs
        PlayerX = transform.position.x;
        AdjustedPlayerX = (float)(PlayerX + 15.5) / 31;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 17, 0), new Vector3(0, 0, 1), out hit))
        {
            _nextGate = hit.collider.gameObject.transform.parent.gameObject;
        }
        NextGateDistance = _nextGate.transform.position.z - transform.position.z;
        AdjustedNextGateDistance = NextGateDistance / 100;
        ObstacleStates = _nextGate.GetComponent<ResetGate>().ObstacleStates;
        float[] inputs = { AdjustedPlayerX, AdjustedNextGateDistance, ObstacleStates[0], ObstacleStates[1], ObstacleStates[2], ObstacleStates[3] };

        //forward propagate inputs and organize output
        float[,] output = ForwardPropagation(Theta, inputs);
        float leftWeight = output[0, 0], rightWeight = output[0, 1], noneWeight = output[0, 2];

        //tell PlayerMove what to do
        pm.Left = false; pm.Right = false;
        if (leftWeight > rightWeight && leftWeight >= noneWeight) pm.Left = true;
        else if (leftWeight < rightWeight && rightWeight >= noneWeight) pm.Right = true;
       
    }

    public float[,] Sigmoid(float[,] matIn)
    {
        float[,] matOut = new float[matIn.GetLength(0), matIn.GetLength(1)];
        for (int i = 0; i < matIn.GetLength(0); i++)
        {
            for (int j = 0; j < matIn.GetLength(1); j++)
            {
                matOut[i, j] = (float)(1 / (1 + Math.Pow(e, -matIn[i, j])));
            }
        }
        return matOut;
    }

    public float[,] MatrixMultiplication(float[,] mat1, float[,] mat2)
    {
        //if (mat1.GetLength(1) != mat2.GetLength(0)) throw new Exception();
        float[,] matOut = new float[mat1.GetLength(0), mat2.GetLength(1)];
        for (int i = 0; i < mat1.GetLength(0); i++)
        {
            for (int j = 0; j < mat2.GetLength(1); j++)
            {
                float sum = 0;
                for (int k = 0; k < mat1.GetLength(1); k++)
                {
                    sum += mat1[i, k] * mat2[k, j];
                }
                matOut[i, j] = sum;
            }
        }
        return matOut;
    }

    public float[,] Transpose(float[,] matIn)
    {
        float[,] matOut = new float[matIn.GetLength(1), matIn.GetLength(0)];
        for (int i = 0; i < matIn.GetLength(0); i++)
        {
            for (int j = 0; j < matIn.GetLength(1); j++)
            {
                matOut[j, i] = matIn[i, j];
            }
        }
        return matOut;
    }
    /*
      float[,] X = new float[1, inputs.Length + 1];
        X[0,0] = 1;
        for (int i = 0; i < inputs.Length; i++) X[0, i + 1] = inputs[i];
        float[,] XTheta = MatrixMultiplication(X, weights[0]);
        float[,] Z = Sigmoid(XTheta);
        for (int i = 1; i < weights.Length - 1; i++)
        {
            X = new float[1, Z.GetLength(1) + 1];
            X[0, 0] = 1;
            for (int j = 1; j < X.GetLength(1); j++) X[0, j] = Z[0, j - 1];
            XTheta = MatrixMultiplication(X, weights[i]);
            Z = Sigmoid(XTheta);
        }
        X = new float[1, Z.GetLength(1) + 1];
        X[0, 0] = 1;
        for (int j = 1; j < X.GetLength(1); j++) X[0, j] = Z[0, j - 1];
        XTheta = new float[1, OutputLayerSize];
        XTheta = MatrixMultiplication(X, weights[weights.Length - 1]);
        Z = new float[1, OutputLayerSize];
        Z = Sigmoid(XTheta);
        return Z;
     */
    public float[,] ForwardPropagation(float[][,] weights, float[] inputs)
    {
        float[,] Z = new float[1, inputs.Length];
        for (int i = 0; i < inputs.Length; i++) Z[0, i] = inputs[i];
        float[,] X;
        float[,] XTheta;
        for (int i = 0; i < weights.Length - 1; i++)
        {
            X = new float[1, Z.GetLength(1) + 1];
            X[0, 0] = 1;
            for (int j = 1; j < X.GetLength(1); j++) X[0, j] = Z[0, j - 1];
            XTheta = MatrixMultiplication(X, weights[i]);
            Z = Sigmoid(XTheta);
        }
        X = new float[1, Z.GetLength(1) + 1];
        X[0, 0] = 1;
        for (int j = 1; j < X.GetLength(1); j++) X[0, j] = Z[0, j - 1];
        XTheta = new float[1, OutputLayerSize];
        XTheta = MatrixMultiplication(X, weights[weights.Length - 1]);
        Z = new float[1, OutputLayerSize];
        Z = Sigmoid(XTheta);
        return Z;
    }
}
