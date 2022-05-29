/*
BetterPrefs is a replacement for Unity's PlayerPrefs that aims to add features that PlayerPrefs is lacking, such as support for multiple saves, save import/export and even more data types, such as booleans, Vector2s and Vector3s.

Version: 3.0.0

https://github.com/Carroted/BetterPrefs

Author: Alex_Sour
License: MIT
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

public static class BetterPrefs
{
    public static string saveLocation = Application.persistentDataPath + "/saves/"; // Set this to wherever you want your saves to be stored. In portable builds, you could use Application.dataPath + "/saves/". (makes saves in the same folder as the executable, like for storing games on a flash drive)
    public static string saveExtension = ".sav"; // Set this to the extension you want your saves for your game to have.
    public static string defaultSaveName = "game"; // The saves created by Set functions (like SetBool, SetFloat etc) will be, by default, saved to a file with this name.

    static Dictionary<string, object> data; // The data that will be saved. This should only ever be accessed through the Get and Set functions.
    public static string currentSave = null; // The full path and name of the save file that is currently loaded, null if no save is loaded.

    static void Write2DArray(string[,] array, BinaryWriter writer) // Used by Save
    {
        writer.Write(array.GetLength(0));
        writer.Write(array.GetLength(1));
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                writer.Write(array[i, j]);
            }
        }

        // Close the writer
        writer.Close();
    }

    static string[,] Read2DArray(BinaryReader reader) // Used by Load
    {
        int x = reader.ReadInt32();
        int y = reader.ReadInt32();
        string[,] array = new string[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                array[i, j] = reader.ReadString();
            }
        }
        // Close the reader
        reader.Close();
        return array;
    }

    public static void SetBool(string key, bool value)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return;
        }
        data[key] = value;
    }

    public static void SetInt(string key, int value)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return;
        }

        data[key] = value;
    }

    public static void SetFloat(string key, float value)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return;
        }

        if (key != "date")
        {
            data[key] = value;
        }
        else
        {
            Debug.LogError("BetterPrefs: Key \"date\" is reserved for the date of the save file. Please use a different key.");
        }
    }

    public static void SetString(string key, string value)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return;
        }
        data[key] = value;
    }

    public static void SetVector2(string key, Vector2 value)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return;
        }
        data[key] = value;
    }

    public static void SetVector3(string key, Vector3 value)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return;
        }
        data[key] = value;
    }

    public static bool GetBool(string key, bool fallback)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return false;
        }

        if (data.ContainsKey(key))
        {
            return (bool)data[key];
        }
        else
        {
            return fallback;
        }
    }

    public static int GetInt(string key, int fallback)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return -1;
        }

        if (data.ContainsKey(key))
        {
            return (int)data[key];
        }
        else
        {
            return fallback;
        }
    }

    public static float GetFloat(string key, float fallback)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return -1;
        }

        if (data.ContainsKey(key))
        {
            return (float)data[key];
        }
        else
        {
            return fallback;
        }
    }

    public static string GetString(string key, string fallback)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return "";
        }

        if (data.ContainsKey(key))
        {
            return ((string)data[key]);
        }
        else
        {
            return fallback;
        }
    }

    public static Vector2 GetVector2(string key, Vector2 fallback)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return Vector2.zero;
        }

        if (data.ContainsKey(key))
        {
            return (Vector2)data[key];
        }
        else
        {
            return fallback;
        }
    }

    public static Vector3 GetVector3(string key, Vector3 fallback)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return Vector3.zero;
        }

        if (data.ContainsKey(key))
        {
            return (Vector3)data[key];
        }
        else
        {
            return fallback;
        }
    }

    // Get methods without fallback

    public static bool GetBool(string key)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return false;
        }

        if (data.ContainsKey(key))
        {
            return (bool)data[key];
        }
        else
        {
            Debug.LogError("BetterPrefs: Key " + key + " does not exist. You should always use BetterPrefs.HasKey to check if a key exists before trying to access it.");
            return false;
        }
    }

    public static int GetInt(string key)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return -1;
        }

        if (data.ContainsKey(key))
        {
            return (int)data[key];
        }
        else
        {
            Debug.LogError("BetterPrefs: Key " + key + " does not exist. You should always use BetterPrefs.HasKey to check if a key exists before trying to access it.");
            return -1;
        }
    }

    public static float GetFloat(string key)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return -1;
        }

        if (data.ContainsKey(key))
        {
            return (float)data[key];
        }
        else
        {
            Debug.LogError("BetterPrefs: Key " + key + " does not exist. You should always use BetterPrefs.HasKey to check if a key exists before trying to access it.");
            return -1;
        }
    }

    public static string GetString(string key)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return "";
        }

        if (data.ContainsKey(key))
        {
            return ((string)data[key]);
        }
        else
        {
            Debug.LogError("BetterPrefs: Key " + key + " does not exist. You should always use BetterPrefs.HasKey to check if a key exists before trying to access it.");
            return "";
        }
    }

    public static Vector2 GetVector2(string key)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return Vector2.zero;
        }

        if (data.ContainsKey(key))
        {
            return (Vector2)data[key];
        }
        else
        {
            Debug.LogError("BetterPrefs: Key " + key + " does not exist. You should always use BetterPrefs.HasKey to check if a key exists before trying to access it.");
            return Vector2.zero;
        }
    }

    public static Vector3 GetVector3(string key)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return Vector3.zero;
        }

        if (data.ContainsKey(key))
        {
            return (Vector3)data[key];
        }
        else
        {
            Debug.LogError("BetterPrefs: Key " + key + " does not exist. You should always use BetterPrefs.HasKey to check if a key exists before trying to access it.");
            return Vector3.zero;
        }
    }

    public static string Save(string savePath = "default")
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return null;
        }

        if (savePath == "default")
        {
            savePath = currentSave; // If no save path is specified, we save to where our loaded save is located. This is useful for saving over the same save file.
        }

        // Data is stored in a two-dimensional array of strings. They contain the type, key and value of each entry.
        /* Example:
        {
            { "int", "exampleInt", "2" },
            { "string", "exampleString", "Hello World!"},
            { "vector2", "exampleVector2", "1.0,2.0" },
            { "bool", "exampleBool", "true" }
        }
        */

        // Add the current date and time to the array (UNIX time)

        double totalSecs = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        data["date"] = (float)totalSecs; // Set the date key to the current time now that we know at what time the save was made

        string[,] dataArray = new string[data.Count, 3];







        if (data.Count == 0)
        {
            // Delete save file if it exists
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            Debug.Log("BetterPrefs: Saved file with no data; deleted file");

            return null;
        }

        int i = 0; // Using a foreach loop with an index is actually simpler than using a for loop for this very specific case.

        foreach (KeyValuePair<string, object> pair in data)
        {
            // Add the stuff to the array

            string objectType = "unknown";
            string valueFormatted = "unparsable";

            if (pair.Value is bool)
            {
                objectType = "bool";
                valueFormatted = pair.Value.ToString().ToLower();
            }
            else if (pair.Value is int)
            {
                objectType = "int";
                valueFormatted = pair.Value.ToString();
            }
            else if (pair.Value is float)
            {
                if (pair.Key != "date")
                {
                    objectType = "float";
                    valueFormatted = pair.Value.ToString();
                }
                else
                {
                    objectType = "float";
                    valueFormatted = totalSecs.ToString(); // override the date value with the current time
                }
            }
            else if (pair.Value is string)
            {
                objectType = "string";
                valueFormatted = pair.Value.ToString();
            }
            else if (pair.Value is Vector2)
            {
                objectType = "vector2";

                Vector2 value = (Vector2)pair.Value;

                valueFormatted = value.x.ToString() + "," + value.y.ToString();
            }
            else if (pair.Value is Vector3)
            {
                objectType = "vector3";

                Vector3 value = (Vector3)pair.Value;

                valueFormatted = value.x.ToString() + "," + value.y.ToString() + "," + value.z.ToString();
            }
            else
            {
                Debug.LogError("BetterPrefs: Unsupported type " + pair.Value.GetType() + " for key " + pair.Key);
                continue;
            }

            dataArray[i, 0] = objectType;
            dataArray[i, 1] = pair.Key;
            dataArray[i, 2] = valueFormatted;

            i++;
        }

        // Save the data to the file

        // If any of the directories in savePath don't exist, create them

        Directory.CreateDirectory(Path.GetDirectoryName(savePath));

        // Use Write2DArray we defined earlier to write the data to the file

        FileStream file = File.Create(savePath);

        // Write2DArray(string[,], BinaryWriter)

        Write2DArray(dataArray, new BinaryWriter(file));




        file.Close();

        Debug.Log("BetterPrefs: Saved data to \"" + savePath + "\"");

        return savePath; // Return the path to the file, could be useful

    }

    public static void Load(string savePath = "default")
    {
        if (savePath == "default")
        {
            savePath = Path.Combine(saveLocation, defaultSaveName, saveExtension); // If no save path is specified, we load from the default save file.
        }

        data = new Dictionary<string, object>();

        if (File.Exists(savePath)) // If the file doesn't exist, we know that file path is where the current save should be, so we store that and when Save is called, saves will be put there by default.
        {
            // Use Read2DArray we defined earlier to read the data from the file

            FileStream file = File.Open(savePath, FileMode.Open);

            // Read2DArray(BinaryReader)

            string[,] dataArray = Read2DArray(new BinaryReader(file));

            file.Close();

            // Add the data from the array to the dictionary

            for (int i = 0; i < dataArray.GetLength(0); i++)
            {
                string objectType = dataArray[i, 0];
                string key = dataArray[i, 1];
                string valueFormatted = dataArray[i, 2];

                if (objectType == "bool")
                {
                    // This is a bool

                    bool value;

                    if (bool.TryParse(valueFormatted, out value))
                    {
                        // The value is a valid bool
                    }
                    else
                    {
                        // The value is not a valid bool

                        Debug.LogWarning("BetterPrefs: Invalid bool value in save file \"" + savePath + "\" for key \"" + key + "\"");

                        continue;
                    }

                    // Add the value to the dictionary

                    if (!data.ContainsKey(key))
                    {
                        data.Add(key, value);
                    }
                    else
                    {
                        Debug.LogWarning("BetterPrefs: Duplicate key in save file \"" + savePath + "\" " + key + ". Using the last one.");
                        data[key] = value;
                    }
                }
                else if (objectType == "string")
                {
                    // This is a string

                    string value = valueFormatted; // :D :) :D :D

                    // Add the value to the dictionary

                    if (!data.ContainsKey(key))
                    {
                        data.Add(key, value);
                    }
                    else
                    {
                        Debug.LogWarning("BetterPrefs: Duplicate key in save file \"" + savePath + "\" " + key + ". Using the last one.");
                        data[key] = value;
                    }
                }
                else if (objectType == "int")
                {
                    // This is an int

                    int value;

                    if (int.TryParse(valueFormatted, out value))
                    {
                        // The value is a valid int
                    }
                    else
                    {
                        // The value is not a valid int

                        Debug.LogWarning("BetterPrefs: Invalid int value in save file \"" + savePath + "\" for key \"" + key + "\"");

                        continue;
                    }

                    // Add the value to the dictionary

                    if (!data.ContainsKey(key))
                    {
                        data.Add(key, value);
                    }
                    else
                    {
                        Debug.LogWarning("BetterPrefs: Duplicate key in save file \"" + savePath + "\" " + key + ". Using the last one.");
                        data[key] = value;
                    }
                }
                else if (objectType == "float")
                {
                    // This is a float

                    float value;

                    if (float.TryParse(valueFormatted, out value))
                    {
                        // The value is a valid float
                    }
                    else
                    {
                        // The value is not a valid float

                        Debug.LogWarning("BetterPrefs: Invalid float value in save file \"" + savePath + "\" for key \"" + key + "\"");

                        continue;
                    }

                    // Add the value to the dictionary

                    if (!data.ContainsKey(key))
                    {
                        data.Add(key, value);
                    }
                    else
                    {
                        Debug.LogWarning("BetterPrefs: Duplicate key in save file \"" + savePath + "\" " + key + ". Using the last one.");
                        data[key] = value;
                    }
                }
                else if (objectType == "vector2")
                {
                    // This is a vector2

                    Vector2 value;

                    // There is no TryParse for Vector2, so we have to do it manually

                    float x;
                    float y;

                    string[] values = valueFormatted.Split(',');

                    if (values.Length != 2)
                    {
                        // There is more than one comma in the value, so it is not a valid vector2

                        Debug.LogWarning("BetterPrefs: Invalid vector2 value in save file \"" + savePath + "\" for key \"" + key + "\"");

                        continue;
                    }
                    else
                    {
                        if (float.TryParse(values[0].Trim(), out x))
                        {
                            // The x value is a valid float
                        }
                        else
                        {
                            // The x value is not a valid float

                            Debug.LogWarning("BetterPrefs: Invalid vector2 X value in save file \"" + savePath + "\" for key \"" + key + "\"");

                            continue;
                        }

                        if (float.TryParse(values[1].Trim(), out y))
                        {
                            // The y value is a valid float
                        }
                        else
                        {
                            // The y value is not a valid float

                            Debug.LogWarning("BetterPrefs: Invalid vector2 Y value in save file \"" + savePath + "\" for key \"" + key + "\"");

                            continue;
                        }

                        // Add the value to the dictionary

                        value = new Vector2(x, y);

                        if (!data.ContainsKey(key))
                        {
                            data.Add(key, value);
                        }
                        else
                        {
                            Debug.LogWarning("BetterPrefs: Duplicate key in save file \"" + savePath + "\" " + key + ". Using the last one.");
                            data[key] = value;
                        }
                    }
                }
                else if (objectType == "vector3")
                {
                    // This is a vector3

                    Vector3 value;

                    // There is no TryParse for Vector3, so we have to do it manually

                    float x;
                    float y;
                    float z;

                    string[] values = valueFormatted.Split(',');

                    if (values.Length != 3)
                    {
                        // There is more than one comma in the value, so it is not a valid vector3

                        Debug.LogWarning("BetterPrefs: Invalid vector3 value in save file \"" + savePath + "\" for key \"" + key + "\"");

                        continue;
                    }
                    else
                    {
                        if (float.TryParse(values[0].Trim(), out x))
                        {
                            // The x value is a valid float
                        }
                        else
                        {
                            // The x value is not a valid float

                            Debug.LogWarning("BetterPrefs: Invalid vector3 X value in save file \"" + savePath + "\" for key \"" + key + "\"");

                            continue;
                        }

                        if (float.TryParse(values[1].Trim(), out y))
                        {
                            // The y value is a valid float
                        }
                        else
                        {
                            // The y value is not a valid float

                            Debug.LogWarning("BetterPrefs: Invalid vector3 Y value in save file \"" + savePath + "\". for key \"" + key + "\"");

                            continue;
                        }

                        if (float.TryParse(values[2].Trim(), out z))
                        {
                            // The z value is a valid float
                        }
                        else
                        {
                            // The z value is not a valid float

                            Debug.LogWarning("BetterPrefs: Invalid vector3 Z value in save file \"" + savePath + "\" for key \"" + key + "\"");

                            continue;
                        }

                        // Add the value to the dictionary

                        value = new Vector3(x, y, z);

                        if (!data.ContainsKey(key))
                        {
                            data.Add(key, value);
                        }
                        else
                        {
                            Debug.LogWarning("BetterPrefs: Duplicate key in save file \"" + savePath + "\" " + key + ". Using the last one.");
                            data[key] = value;
                        }
                    }
                }
                else if (objectType == "unknown")
                {
                    Debug.LogWarning("BetterPrefs: A type of unknown was found in save file \"" + savePath + "\". This is caused by BetterPrefs.Save encountering an unsupported type in its data. Please do not manually edit BetterPrefs data from memory without using BetterPrefs methods.");
                }
                else
                {
                    // This is not a valid type

                    Debug.LogWarning("BetterPrefs: Invalid type in save file \"" + savePath + "\" for key \"" + key + "\"");

                    continue;
                }

            }
        }

        currentSave = savePath;
    }

    public static void DeleteAll()
    {
        // Delete all the data

        if (data == null)
        {
            data = new Dictionary<string, object>();
        }

        data.Clear();

        Debug.Log("BetterPrefs: Deleted all data (this can be undone by quitting without saving)");
    }

    public static void DeleteKey(string key)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return;
        }

        // Delete the key

        data.Remove(key);
    }

    public static bool HasKey(string key)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return false;
        }

        // Check if the key exists

        return data.ContainsKey(key);
    }

    public static DateTime GetDate(string savePath = "current") // Get the date of a save, or, if the save wasn't saved yet, the current date
    {
        if (savePath == "current")
        {
            if (HasKey("date"))
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((int)GetFloat("date"));

                return dateTimeOffset.LocalDateTime;
            }
            else
            {
                return DateTime.Now;
            }
        }
        else
        {
            if (!File.Exists(savePath))
            {
                Debug.LogWarning("BetterPrefs: The save file \"" + savePath + "\" does not exist");
                return DateTime.Now;
            }

            // Get the date from the save file using Read2DArray

            FileStream file = File.Open(savePath, FileMode.Open);

            BinaryReader reader = new BinaryReader(file);

            string[,] dataArray = Read2DArray(reader);

            file.Close();

            for (int i = 0; i < dataArray.GetLength(0); i++)
            {
                string objectType = dataArray[i, 0];
                string key = dataArray[i, 1];
                string valueFormatted = dataArray[i, 2];

                if (key == "date")
                {
                    if (objectType == "float")
                    {
                        // This is a float

                        float value;

                        if (float.TryParse(valueFormatted, out value))
                        {
                            // The value is a valid float
                        }
                        else
                        {
                            // The value is not a valid float

                            Debug.LogWarning("BetterPrefs: Invalid date in save file \"" + savePath + "\"");

                            continue;
                        }

                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((int)value);

                        return dateTimeOffset.LocalDateTime;
                    }
                }
            }

            Debug.LogError("BetterPrefs: Could not find the date in the save file");

            return DateTime.MinValue;
        }
    }

    public static int GetCount() // Get how many keys are in the data
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return -1;
        }

        return data.Count;
    }

    public static Dictionary<string, object> GetData() // Get the data
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return new Dictionary<string, object>();
        }

        return data;
    }
}
