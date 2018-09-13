using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class DataCollector : MonoBehaviour
{
    private static DataCollector instance;
    [SerializeField]

    public string filePath;

    [SerializeField]
    private int entriesInFile = 0;

    private static StreamWriter streamWriter;

    // private StringBuilder stringBuilder = new StringBuilder();
    private const string EXT = ".csv";

    private static Data data;

    void Start()
    {
        instance = this;

        string completeFileName = filePath + EXT;
        if (System.IO.File.Exists(completeFileName))
        {
            streamWriter = new StreamWriter(completeFileName, true);
        } else {
            streamWriter = new StreamWriter(completeFileName, false);
            streamWriter.WriteLine("Translation id,Reaction time,Group");
        }
    }

    public static void StoreNewTranslation()
    {
        data = new Data();
        data.SetTranslationId(DateTime.Now);
        data.SetGroup(ModeController.SoundCues, ModeController.Minimap);
        Debug.Log("Translation id: " + data.TranslationId);
        Debug.Log("Group: " + data.Group);
    }

    public static void StoreTranlationStartTime(float time)
    {
        Debug.Log("Translation start time: " + time);
        data.SetTranlsationStartTime(time);
    }

    public static void StoreGuessTime(float time)
    {
        Debug.Log("Guess time: " + time);
        data.SetGuessTime(time);
    }

    /// <summary>Saves Data to file.</summary>
    public static void LogDataEntry()
    {
        string dataString = data.ToString();
        Debug.Log("Logging data entry: " + dataString);

        streamWriter.WriteLine(dataString);
        streamWriter.Flush();

        instance.entriesInFile++;
    }



    private void CloseStreamWriter()
    {
        if (streamWriter != null)
        {
            streamWriter.Close();
        }
    }

    // Close streams
    void OnApplicationQuit()
    {
        CloseStreamWriter();
    }

    public class Data
    {
        private string tranlsationId;
        private float translationStartTime;
        private float guessTime;
        private int group;

        private float ReactionTime
        {
            get
            {
                return guessTime - translationStartTime;
            }
        }

        public string TranslationId
        {
            get { return tranlsationId; }
        }
        public int Group {
            get { return group; }
        }

        public void SetTranslationId(DateTime now)
        {
            tranlsationId = now.ToString("yyyyMMddHHmmssffff");
        }

        public void SetTranlsationStartTime(float time)
        {
            translationStartTime = time;
            guessTime = time; // if the difference is 0, we interpret it as no correct guess
        }

        public void SetGuessTime(float time)
        {
            guessTime = time;
        }

        public void SetGroup(bool soundCues, bool minimap)
        {
            if (soundCues && minimap)
            {
                group = 1;
            }

            if (soundCues && !minimap)
            {
                group = 2;
            }

            if (!soundCues && minimap)
            {
                group = 3;
            }
        }


        public override string ToString()
        {
            return tranlsationId + "," + ReactionTime + "," + group;
        }
    }
}
