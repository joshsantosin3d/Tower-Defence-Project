using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(HighScoreData newData)
    {
        //Formatter instance
        BinaryFormatter formatter = new BinaryFormatter();

        //Determine the location of the file
        string path = Application.persistentDataPath + "/SaveData" + newData.levelName + ".txt";
        Debug.Log(path);

        //Open a file stream to put the date into the file.
        FileStream stream = new FileStream(path, FileMode.Create);

        //Put the infomation into the file.
        formatter.Serialize(stream, newData);

        //Stop the file stream
        stream.Close();
    }

    public static HighScoreData LoadPlayer()
    {
        string levelName = SceneManager.GetActiveScene().name;

        //Determine the location of the file
        string path = Application.persistentDataPath + "/SaveData" + levelName + ".txt";

        if (File.Exists(path))      //Check if we have a scores file to load
        {
            //Formatter instance
            BinaryFormatter formatter = new BinaryFormatter();

            //Open a file stream to put the data into the file.
            FileStream stream = new FileStream(path, FileMode.Open);

            //De-incrypt data
            HighScoreData data = formatter.Deserialize(stream) as HighScoreData;
            stream.Close();

            return data;
        }
        else        //No scores to load
        {
            Debug.LogWarning("No file data found");
            return new HighScoreData(levelName);        //make an empty file to "load" if we don't have one already
        }
    }
}
