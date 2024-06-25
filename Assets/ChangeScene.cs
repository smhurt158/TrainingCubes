using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void StartTrain(string sceneName)
    {
        ProjectSetts ps;
        if (File.Exists(Application.persistentDataPath + "/ps"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/ps", FileMode.Open);
            ps = (ProjectSetts)bf.Deserialize(file);
            file.Close();
            ps.testMode = false;
            ProjectSettings.CreateProjectSettsFile("/ps", ps);
        }
        SceneManager.LoadScene(sceneName);
    }

}
