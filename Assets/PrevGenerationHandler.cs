using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class PrevGenerationHandler : MonoBehaviour
{
    ProjectSetts ps;
    private int prevGens;
    public int page = 0;
    public GameObject Text, Next, Last, Next5, Last5;

    private void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/ps"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/ps", FileMode.Open);
            ps = (ProjectSetts)bf.Deserialize(file);
            file.Close();
            prevGens = ps.Generation - 1;
        }
        if(prevGens > 0)
        {
            ChangeGen(1);
        }
        

    }
    public void ChangeGen(int change)
    {
        page += change;
        if (page + 1 > prevGens) Next.SetActive(false);
        else Next.SetActive(true);

        if (page + 5 > prevGens) Next5.SetActive(false);
        else Next5.SetActive(true);

        if (page - 1 < 1) Last.SetActive(false);
        else Last.SetActive(true);

        if (page - 5  < 1) Last5.SetActive(false);
        else Last5.SetActive(true);

        Text.GetComponent<Text>().text = "Generation " + page;
    }
}
