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

        // Open streams
        int count = 1;
        while (true) {
            string completeFileName = filePath + count + EXT;

            if (System.IO.File.Exists(completeFileName)) {
                count++;
                continue;
            }

            streamWriter = new StreamWriter(completeFileName, false);
            break;
        }

        streamWriter.WriteLine("Translation id,Reaction time,Group");
    }

    public static void StoreNewTranslation() {
        data = new Data();
        data.SetTranslationId(DateTime.Now);
    }

    public static void StoreTranlationStartTime(float time) {
        data.SetTranlsationStartTime(time);
    }

    public static void StoreGuessTime(float time) {
        data.SetGuessTime(time);
    }

    public static void LogDataEntry()
    {
        string dataString = data.ToString();

        streamWriter.WriteLine(dataString);
        streamWriter.Flush();

        instance.entriesInFile++;
    }

    

    private  void CloseStreamWriter()
    {
        if (streamWriter != null) {
            streamWriter.Close();
        }
    }

	// Close streams
    void OnApplicationQuit()
    {
        CloseStreamWriter();
    }

    public class Data {
        private string tranlsationId;
        private float translationStartTime;
        private float guessTime;
        private int group;

        private float ReactionTime {
            get {
                return guessTime - translationStartTime;
            }
        }

        public void SetTranslationId(DateTime now) {
            tranlsationId = now.ToString("yyyyMMddHHmmssffff");
        }

        public void SetTranlsationStartTime(float time) {
            translationStartTime = time;
            guessTime = time; // if the difference is 0, we interpret it as no correct guess
        }

        public void SetGuessTime(float time) {
            guessTime = time;
        }

        public void SetGroup(bool soundCues, bool minimap) {
            if (soundCues && minimap) {
                group = 1;
            }

            if (soundCues && !minimap) {
                group = 2;
            }

            if (!soundCues && minimap) {
                group = 3;
            }
        }


        public string ToString() {
            return tranlsationId + "," + ReactionTime + "," + group;
        }
    }
}
