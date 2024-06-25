using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ProjectSetts
{
    public int HiddenLayerSize, HiddenLayers, MutationsPerChild, SpecimenPerGeneration, SavesPerGeneration;
    public float MutationSize;

    public int Generation, GenerationNumber;
    public float[] Scores;
    public bool testMode = false;
    public Specimen TestSpecimen; 
    public ProjectSetts(int HiddenLayerSize, int HiddenLayers, float MutationSize, int MutationsPerChild, int SpecimenPerGeneration, int SavesPerGeneration)
    {
        this.GenerationNumber = 1;
        this.Generation = 1;
        this.HiddenLayerSize = HiddenLayerSize;
        this.HiddenLayers = HiddenLayers;
        this.MutationsPerChild = MutationsPerChild;
        this.MutationSize = MutationSize;
        this.SpecimenPerGeneration = SpecimenPerGeneration;
        this.SavesPerGeneration = SavesPerGeneration;
        this.Scores = new float[SpecimenPerGeneration];
    }
}