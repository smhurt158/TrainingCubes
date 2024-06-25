using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NNManager : MonoBehaviour
{
    public static NNManager Instance;

    System.Random rand = new System.Random();
    [SerializeField] public GameObject Player, GenText;
    NueralNetwork PlayerNN;
    PlayerHealth ph;
    ProjectSetts ps;

    public int SpecimenNumOverride = 0;

    public int HiddenLayers, HiddenLayerSize;
    private int InputLayerSize = 6, OutputLayerSize = 3,
        
    
        GenerationSize, Generation, SavesPerGeneration, MutationsPerChild;
    private float MutationSize;

    public Specimen CurrSpecimen;
    private int SpecimenNum;

    private float[] Scores;

    private void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }


        PlayerNN = Player.GetComponent<NueralNetwork>();
        ph = Player.GetComponent<PlayerHealth>();

       


        //load the project settings
        string filePath = "/ps";
        if (File.Exists(Application.persistentDataPath + filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filePath, FileMode.Open);
            ps = (ProjectSetts)bf.Deserialize(file);
            file.Close();
            HiddenLayers = ps.HiddenLayers;
            HiddenLayerSize = ps.HiddenLayerSize;


            MutationSize = ps.MutationSize;
            MutationsPerChild = ps.MutationsPerChild;
            GenerationSize = ps.SpecimenPerGeneration;
            SavesPerGeneration = ps.SavesPerGeneration;

            Generation = ps.Generation;
            SpecimenNum = ps.GenerationNumber;

            PlayerNN.HiddenLayers = HiddenLayers;
            PlayerNN.HiddenLayerSize = HiddenLayerSize;

            
        }
        
        //if project is new make a new set of specimen
        if (ps.Generation == 1 && ps.GenerationNumber == 1 && !CheckTesting())
        {
            if (Directory.Exists(Application.persistentDataPath + "/Project"))
            {
                Directory.Delete(Application.persistentDataPath + "/Project", true);
            }
            Directory.CreateDirectory(Application.persistentDataPath + "/Project");
            
            Directory.CreateDirectory(Application.persistentDataPath + "/Project/CurrGen");
            Directory.CreateDirectory(Application.persistentDataPath + "/Project/PrevGens");
            for (int i = 0; i < GenerationSize; i++)
            {
                Specimen sp = CreateNewSpecimen(1);
                CreateSpecimenFile("/Project/CurrGen" + string.Format("/Specimen{0}", i + 1), sp);
            }
            
        }
        if (SpecimenNumOverride != 0 && !CheckTesting())
        {
            ps.GenerationNumber = SpecimenNumOverride;
            ProjectSettings.CreateProjectSettsFile("/ps", ps);

        }
        SpecimenNum = !CheckTesting() ? ps.GenerationNumber : 0;
        //Debug.Log(SpecimenNum);
        CurrSpecimen = !CheckTesting() ? LoadSpecimen("/Project/CurrGen/Specimen" + SpecimenNum) : ps.TestSpecimen;
       // Debug.Log("/Project/CurrGen/Specimen" + SpecimenNum);
        PlayerNN.specimen = CurrSpecimen;
        PlayerNN.specNum = SpecimenNum;

        GenText.GetComponent<UnityEngine.UI.Text>().text = !CheckTesting() ? "Gen: " + Generation : "Gen Born: " + ps.TestSpecimen.GenBorn;
    }

    void Start()
    {
        
        Scores = new float[GenerationSize];
        if (!CheckTesting()) ph.Death += NextSpecimen;
        else ph.Death += ChangeToMenu;
    }

    void NextSpecimen()
    {
        PlayerMove pm = Player.GetComponent<PlayerMove>();
        ps.Scores[SpecimenNum - 1] = pm.Score;
        if (pm.Score > CurrSpecimen.Score) CurrSpecimen.Score = pm.Score;
        CreateSpecimenFile("/Project/CurrGen" + string.Format("/Specimen{0}", SpecimenNum), CurrSpecimen);
        
        SpecimenNum++;
        if (SpecimenNum > GenerationSize)
        {
            NextGeneration();
            SpecimenNum = 1;
        }
        ps.GenerationNumber = SpecimenNum;
        //updates ps file
        ProjectSettings.CreateProjectSettsFile("/ps", ps);

        SceneManager.LoadScene("Game");
    }

    void NextGeneration()
    {
        //sorts the score array without getting rid of the original index
        Tuple<float, int>[] scoresOrder = SortArrayWithIndex(ps.Scores);

        //saves the best specimen of the generation 
        string newFolder = "/Project/PrevGens"  + string.Format("/Generation{0}", Generation);
        Directory.CreateDirectory(Application.persistentDataPath + newFolder);
        for (int i = 0; i < SavesPerGeneration && i < GenerationSize; i++)
        {
            string newFile = newFolder + string.Format("/Specimen{0}", i + 1);
            string specimenFile = "/Project/CurrGen" + string.Format("/Specimen{0}", scoresOrder[i].Item2 + 1);
            CopyFile(specimenFile, newFile);
        }

        //Replaces the worst half of the specimen with children of the best half
        Generation++;
        ps.Generation = Generation;
        for (int i = 0; i < GenerationSize/2; i++)
        {
            int badSpecimenNum = scoresOrder[GenerationSize / 2 + i].Item2 + 1;
            int goodSpecimenNum = scoresOrder[i].Item2 + 1;
            Specimen goodSpecimen = LoadSpecimen("/Project/CurrGen" + string.Format("/Specimen{0}", goodSpecimenNum));
            CreateSpecimenFile("/Project/CurrGen" + string.Format("/Specimen{0}", badSpecimenNum), ReproduceSpecimen(goodSpecimen, Generation));
        }

        

    }
    //sorts array but stores original index
    public Tuple<float, int>[] SortArrayWithIndex(float[] Array)
    {
        Tuple<float, int>[] output = new Tuple<float, int>[Array.Length];
        for (int i = 0; i < Array.Length; i++)
        {
            output[i] = new Tuple<float, int>(Array[i], i);
        }
        int switches;
        do
        {
            switches = 0;
            for (int i = 0; i < Array.Length - 1; i++)
            {
                if (output[i].Item1 < output[i + 1].Item1)
                {
                    Tuple<float, int> placeHolder = output[i];
                    output[i] = output[i + 1];
                    output[i + 1] = placeHolder;
                    switches++;
                }
            }
        }
        while (switches > 0);
        return output;
    }

    public void CreateSpecimenFile(string filePath, Specimen specimen)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + filePath);
            bf.Serialize(file, specimen);
            file.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public static Specimen LoadSpecimen(string filePath)
    {
        if (File.Exists(Application.persistentDataPath + filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filePath, FileMode.Open);
            Specimen specimen = (Specimen)bf.Deserialize(file);
            file.Close();
            return specimen;
        }
        else
        {
            return null;
        }
    }

    public bool CheckTesting()
    {
        return ps.testMode;
    }

    public Specimen CreateNewSpecimen(int generation, string name)
    {
        //initialize weights and each 2D array within it
        float[][,] weights = new float[HiddenLayers + 1][,];
        weights[0] = HiddenLayers != 0 ? new float[InputLayerSize + 1, HiddenLayerSize] : new float[InputLayerSize + 1, OutputLayerSize];
        for (int i = 1; i < weights.Length - 1; i++)
        {
            weights[i] = new float[HiddenLayerSize + 1, HiddenLayerSize];
        }
        weights[weights.Length - 1] = HiddenLayers != 0 ? new float[HiddenLayerSize + 1, OutputLayerSize] :  weights[0];

        //uses normal distribution to create randomized weight values
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].GetLength(0); j++)
            {
                for (int k = 0; k < weights[i].GetLength(1); k++)
                {
                    float u1 = (float)(1.0 - rand.NextDouble()); //uniform(0,1] random doubles
                    float u2 = (float)(1.0 - rand.NextDouble());
                    float randStdNormal = (float)(Math.Sqrt(-2.0 * Math.Log(u1)) *
                                 Math.Sin(2.0 * Math.PI * u2)); //random normal(0,1)
                    weights[i][j, k] = randStdNormal;
                }
            }
        }
        return new Specimen(weights, generation, name);
    }
    
    public void CopyFile(string startFile, string endFile)
    {
        CreateSpecimenFile(endFile, LoadSpecimen(startFile));
    }

    public Specimen CreateNewSpecimen(int generation)
    {
        return CreateNewSpecimen(generation, RandomName(6));
    }
    public Specimen CreateNewSpecimen()
    {
        return CreateNewSpecimen(1, RandomName(6));
    }

    public Specimen ReproduceSpecimen(Specimen parent, int generation)
    {
        float[][,] ThetaIn = parent.Weights;
        float[][,] ThetaOut = ThetaIn;
        for (int l = 0; l < MutationsPerChild; l++)
        {
            //a given weight in a smaller layers are more likely to be changed
            int i = rand.Next(ThetaIn.Length);
            int j = rand.Next(ThetaIn[i].GetLength(0));
            int k = rand.Next(ThetaIn[i].GetLength(1));
            float u1 = (float)(1.0 - rand.NextDouble()); //uniform(0,1] random doubles
            float u2 = (float)(1.0 - rand.NextDouble());
            float randStdNormal = (float)(Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2)); //random normal(0,1)
            Debug.Log(ThetaOut[i][j, k] + " to " + (ThetaOut[i][j, k] + (float)randStdNormal * MutationSize));
            ThetaOut[i][j, k] += (float)randStdNormal * MutationSize;
        }
        string childName = RandomName(2) + parent.Name[0] + parent.Name[1] + parent.Name[4] + parent.Name[5];
        Specimen child = new Specimen(ThetaOut, generation, childName);
        child.Parents = parent.Parents + 1;
        //child.Parent = parent; probably wont work with saving and loading
        return child;
       
    }

    private string RandomName(int length)
    {
        string consonants = "BCDFGHJKLMNPQRSTVWXYZ";
        string vowels = "AEIOU";
        int nameLength = length;
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < nameLength; i++)
        {
            if(i % 2 == 0)
            {
                sb.Append(consonants[rand.Next(0, 21)]);
            }
            else
            {
                sb.Append(vowels[rand.Next(0, 5)]);
            }
        }
        return sb.ToString();
    }

    private void ChangeToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
