using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenerationHandler : MonoBehaviour
{
    public string Generation;
    private int page = 0;
    public Specimen specimen;
    public GameObject text, Next, Last, Next5, Last5;
    public PrevGenerationHandler pg;
  
    public void setPrevGen()
    {
        page = 0;
        Generation = "/Project/PrevGens/Generation" + pg.page;
        ChangePage(1);
    }
    public void setGen()
    {
        page = 0;
        Generation = "/Project/CurrGen";
        ChangePage(1);
    }
    public void ChangePage(int change)
    {
        page += change;

        string filePath = Generation + string.Format("/Specimen{0}", page);
        specimen = NNManager.LoadSpecimen(filePath);

        if (specimen == null) Debug.Log("Null" + Generation + string.Format("/Specimen{0}", page));
        text.GetComponent<Text>().text = "Specimen " + page + ":\n" + specimen.ToString();

        if (File.Exists(Application.persistentDataPath + Generation + string.Format("/Specimen{0}", page + 1)))
        {
            Next.SetActive(true);
        }
        else
        {
            Next.SetActive(false);
        }

        if (File.Exists(Application.persistentDataPath + Generation + string.Format("/Specimen{0}", page + 5)))
        {
            Next5.SetActive(true);
        }
        else
        {
            Next5.SetActive(false);
        }

        if (page - 1 < 1)
        {
            Last.SetActive(false);
        }
        else
        {
            Last.SetActive(true);
        }

        if (page - 5 < 1)
        {
            Last5.SetActive(false);
        }
        else
        {
            Last5.SetActive(true);
        }
    }

    public void TestSpecimen(string sceneName)
    {
        ProjectSetts ps;
        if (File.Exists(Application.persistentDataPath + "/ps"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/ps", FileMode.Open);
            ps = (ProjectSetts)bf.Deserialize(file);
            file.Close();
            ps.TestSpecimen = specimen;
            ps.testMode = true;
            ProjectSettings.CreateProjectSettsFile("/ps", ps);
        }
        SceneManager.LoadScene(sceneName);


    }
}
