using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ProjectSettings : MonoBehaviour
{
    [SerializeField] private GameObject HiddenLayerSizeText, HiddenLayersText, MutationSizeText, MutationsPerChildText, SpecimenPerGenerationText, SavesPerGenerationText;
    
    public void CreateProjectSettsFile()
    {
        int a = (int)HiddenLayerSizeText.GetComponent<UpdateText>().Value;
        int b = (int)HiddenLayersText.GetComponent<UpdateText>().Value;
        float c = MutationSizeText.GetComponent<UpdateText>().Value;
        int d = (int)MutationsPerChildText.GetComponent<UpdateText>().Value;
        int e = (int)SpecimenPerGenerationText.GetComponent<UpdateText>().Value;
        int f = (int)SavesPerGenerationText.GetComponent<UpdateText>().Value;

        CreateProjectSettsFile("/ps", a, b, c, d, e, f);
    }
    public static void CreateProjectSettsFile(string filePath, int HiddenLayerSize, int HiddenLayers, float MutationSize, int MutationsPerChild, int SpecimenPerGeneration, int SavesPerGeneration)
    {
        CreateProjectSettsFile(filePath, new ProjectSetts(HiddenLayerSize, HiddenLayers, MutationSize, MutationsPerChild, SpecimenPerGeneration, SavesPerGeneration));
    }
    public static void CreateProjectSettsFile(string filePath, ProjectSetts ps)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            //Debug.Log("File Saved at " + Application.persistentDataPath + filePath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + filePath);
            bf.Serialize(file, ps);
            file.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        if (File.Exists(Application.persistentDataPath + filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filePath, FileMode.Open);
            ProjectSetts specimen = (ProjectSetts)bf.Deserialize(file);
            file.Close();
        }

        
    }
}
