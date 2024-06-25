using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Specimen
{
    public int GenBorn;
    public float[][,] Weights;
    public string Name;
    public Specimen Parent;
    public int Parents;
    public float Score;
    public Specimen(float[][,] Weights, int GenBorn, string name)
    {
        this.Weights = Weights;
        this.GenBorn = GenBorn;
        this.Name = name;
        this.Score = 0;
    }
    public Specimen(float[][,] Weights, int GenBorn, string name, float Score)
    {
        this.Weights = Weights;
        this.GenBorn = GenBorn;
        this.Name = name;
        this.Score = Score;
    }
    public string ToString()
    {
        if (Parent == null) Parent = new Specimen(new float[0][,], 0, "N/A");
        return "Name: " + Name + "\nScore: " + (int)Score + "\nGeneration Born: " + GenBorn + "\nParent:" + Parent.Name
            + "\nParents: " + Parents;
    }
}