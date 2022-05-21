/*
BetterPrefs is a replacement for Unity's PlayerPrefs that aims to add features that PlayerPrefs is lacking, such as support for multiple saves, save import/export and even more data types, such as booleans, Vector2s and Vector3s.

https://github.com/Alex-Sour/BetterPrefs

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

    public static char splitChar = '┄'; // The character that will be used to split the data in the save files. Preferably something you won't use in strings. Instances of this character attempted to be used in SetString will be replaced with a space.

    static Dictionary<string, object> data; // The data that will be saved. This should only ever be accessed through the Get and Set functions.
    public static string currentSave = null; // The full path and name of the save file that is currently loaded, null if no save is loaded.

    public static string startComment = ""; // The comment that will be added to the start of the save file. If this is null or empty, no comment will be added. This is useful for, for example, adding the name of the game at the start of the file.
    public static string endComment = ""; // The comment that will be added to the end of the save file. If this is null or empty, no comment will be added. This is useful for, for example, adding the name of the game at the end of the file.

    public static void Load(string savePath = "default") // Try to load default save if none is specified
    {
        if (savePath == "default")
        {
            savePath = Path.Combine(saveLocation, defaultSaveName, saveExtension);
        }

        data = new Dictionary<string, object>();

        if (File.Exists(savePath))
        {
            // If there is a save, read it

            string saveData = File.ReadAllText(savePath);

            // Read save data into a dictionary

            // Format is as follows: <type>|<key>|<value>

            // Lines that start with # are comments and are ignored

            // Types are: bool,string,int,float,vector2,vector3

            // One per line of the above.

            string[] lines = saveData.Split('\n'); // While we could use Environment.NewLine, it's less cross-platform in this case since it will store different data on different platforms, and our goal is to make a cross-platform portable save format.

            foreach (string line in lines)
            {
                if (line.StartsWith("#"))
                {
                    // This is a comment
                    continue;
                }

                // Make sure there are 2 split characters
                string[] parts = line.Split(splitChar);

                if (parts.Length != 3)
                {
                    // This is not a valid line, it could be an empty line or caused by bit flips/data rot

                    if (line.Trim() != "")
                    {
                        Debug.LogWarning("Invalid line in save file: " + line);
                    }

                    continue;
                }
                else
                {
                    // This is a valid line

                    // Split the line into type, key and value



                    // Check if the type is valid

                    if (parts[0] == "bool")
                    {
                        // This is a bool

                        bool value;

                        if (bool.TryParse(parts[2], out value))
                        {
                            // The value is a valid bool
                        }
                        else
                        {
                            // The value is not a valid bool

                            Debug.LogWarning("Invalid bool value in save file \"" + savePath + "\": " + line);

                            continue;
                        }

                        // Add the value to the dictionary

                        if (!data.ContainsKey(parts[1]))
                        {
                            data.Add(parts[1], value);
                        }
                        else
                        {
                            Debug.LogWarning("Duplicate key in save file \"" + savePath + "\" " + parts[1] + ". Using the last one.");
                            data[parts[1]] = value;
                        }
                    }
                    else if (parts[0] == "string")
                    {
                        // This is a string

                        string value = parts[2]; // This is a text file, so no need to check if the value is a string or not :)

                        // Add the value to the dictionary

                        if (!data.ContainsKey(parts[1]))
                        {
                            data.Add(parts[1], value);
                        }
                        else
                        {
                            Debug.LogWarning("Duplicate key in save file \"" + savePath + "\" " + parts[1] + ". Using the last one.");
                            data[parts[1]] = value;
                        }
                    }
                    else if (parts[0] == "int")
                    {
                        // This is an int

                        int value;

                        if (int.TryParse(parts[2], out value))
                        {
                            // The value is a valid int
                        }
                        else
                        {
                            // The value is not a valid int

                            Debug.LogWarning("Invalid int value in save file \"" + savePath + "\": " + line);

                            continue;
                        }

                        // Add the value to the dictionary

                        if (!data.ContainsKey(parts[1]))
                        {
                            data.Add(parts[1], value);
                        }
                        else
                        {
                            Debug.LogWarning("Duplicate key in save file \"" + savePath + "\" " + parts[1] + ". Using the last one.");
                            data[parts[1]] = value;
                        }
                    }
                    else if (parts[0] == "float")
                    {
                        // This is a float

                        float value;

                        if (float.TryParse(parts[2], out value))
                        {
                            // The value is a valid float
                        }
                        else
                        {
                            // The value is not a valid float

                            Debug.LogWarning("Invalid float value in save file \"" + savePath + "\": " + line);

                            continue;
                        }

                        // Add the value to the dictionary

                        if (!data.ContainsKey(parts[1]))
                        {
                            data.Add(parts[1], value);
                        }
                        else
                        {
                            Debug.LogWarning("Duplicate key in save file \"" + savePath + "\" " + parts[1] + ". Using the last one.");
                            data[parts[1]] = value;
                        }
                    }
                    else if (parts[0] == "vector2")
                    {
                        // This is a vector2

                        Vector2 value;

                        // There is no TryParse for Vector2, so we have to do it manually

                        float x;
                        float y;

                        string[] values = parts[2].Split(',');

                        if (values.Length != 2)
                        {
                            // There is more than one comma in the value, so it is not a valid vector2

                            Debug.LogWarning("Invalid vector2 value in save file \"" + savePath + "\": " + line);

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

                                Debug.LogWarning("Invalid vector2 X value in save file \"" + savePath + "\": " + line);

                                continue;
                            }

                            if (float.TryParse(values[1].Trim(), out y))
                            {
                                // The y value is a valid float
                            }
                            else
                            {
                                // The y value is not a valid float

                                Debug.LogWarning("Invalid vector2 Y value in save file \"" + savePath + "\": " + line);

                                continue;
                            }

                            // Add the value to the dictionary

                            value = new Vector2(x, y);

                            if (!data.ContainsKey(parts[1]))
                            {
                                data.Add(parts[1], value);
                            }
                            else
                            {
                                Debug.LogWarning("Duplicate key in save file \"" + savePath + "\" " + parts[1] + ". Using the last one.");
                                data[parts[1]] = value;
                            }
                        }
                    }
                    else if (parts[0] == "vector3")
                    {
                        // This is a vector3

                        Vector3 value;

                        // There is no TryParse for Vector3, so we have to do it manually

                        float x;
                        float y;
                        float z;

                        string[] values = parts[2].Split(',');

                        if (values.Length != 3)
                        {
                            // There is more than one comma in the value, so it is not a valid vector3

                            Debug.LogWarning("Invalid vector3 value in save file \"" + savePath + "\": " + line);

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

                                Debug.LogWarning("Invalid vector3 X value in save file \"" + savePath + "\": " + line);

                                continue;
                            }

                            if (float.TryParse(values[1].Trim(), out y))
                            {
                                // The y value is a valid float
                            }
                            else
                            {
                                // The y value is not a valid float

                                Debug.LogWarning("Invalid vector3 Y value in save file \"" + savePath + "\": " + line);

                                continue;
                            }

                            if (float.TryParse(values[2].Trim(), out z))
                            {
                                // The z value is a valid float
                            }
                            else
                            {
                                // The z value is not a valid float

                                Debug.LogWarning("Invalid vector3 Z value in save file \"" + savePath + "\": " + line);

                                continue;
                            }

                            // Add the value to the dictionary

                            value = new Vector3(x, y, z);

                            if (!data.ContainsKey(parts[1]))
                            {
                                data.Add(parts[1], value);
                            }
                            else
                            {
                                Debug.LogWarning("Duplicate key in save file \"" + savePath + "\" " + parts[1] + ". Using the last one.");
                                data[parts[1]] = value;
                            }
                        }
                    }
                    else if (parts[0] == "unknown")
                    {
                        Debug.LogWarning("A type of unknown was found in save file \"" + savePath + "\". This is caused by BetterPrefs.Save encountering an unsupported type in its data. Please do not manually edit BetterPrefs data from memory without using BetterPrefs methods.");
                    }
                    else
                    {
                        // This is not a valid type

                        Debug.LogWarning("Invalid type in save file \"" + savePath + "\": " + line);

                        continue;
                    }
                }
            }
        }

        currentSave = savePath;
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

        if (key != "date")
        {
            data[key] = value;
        }
        else
        {
            Debug.LogError("BetterPrefs: Key \"date\" is reserved for the date of the save file. Please use a different key.");
        }
    }

    public static void SetFloat(string key, float value)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return;
        }
        data[key] = value;
    }

    public static void SetString(string key, string value)
    {
        if (data == null)
        {
            Debug.LogError("BetterPrefs: No save is loaded, but you are trying to access it");
            return;
        }
        data[key] = value.Replace("\n", "\\n");
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
            Debug.LogError("BetterPrefs: Key \"" + key + "\" does not exist. You should always use BetterPrefs.HasKey to check if a key exists before trying to access it.");
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
            Debug.LogError("BetterPrefs: Key \"" + key + "\" does not exist. You should always use BetterPrefs.HasKey to check if a key exists before trying to access it.");
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
            return ((string)data[key]).Replace("\\n", "\n");
        }
        else
        {
            Debug.LogError("BetterPrefs: Key \"" + key + "\" does not exist. You should always use BetterPrefs.HasKey to check if a key exists before trying to access it.");
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
            Debug.LogError("BetterPrefs: Key \"" + key + "\" does not exist. You should always use BetterPrefs.HasKey to check if a key exists before trying to access it.");
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
            Debug.LogError("BetterPrefs: Key \"" + key + "\" does not exist. You should always use BetterPrefs.HasKey to check if a key exists before trying to access it.");
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
            savePath = currentSave;
        }

        // Create a string to store the data in

        string dataString = "";

        if (data.Count > 0)
        {
            // Loop through the data

            foreach (KeyValuePair<string, object> pair in data)
            {
                // Add the key and value to the string

                string objectType = "unknown";
                string valueFormatted = "unparsable";

                if (pair.Value is bool)
                {
                    objectType = "bool";
                    valueFormatted = pair.Value.ToString().ToLower();
                }
                else if (pair.Value is int)
                {
                    if (pair.Key != "date")
                    {
                        objectType = "int";
                        valueFormatted = pair.Value.ToString();
                    }
                    else
                    {
                        continue; // Ignore the date key since we add it automatically after this foreach loop
                    }
                }
                else if (pair.Value is float)
                {
                    objectType = "float";
                    valueFormatted = pair.Value.ToString();
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

                dataString += objectType + splitChar + pair.Key + splitChar + valueFormatted + "\n";
            }

            // Add the current date and time to the string (UNIX time)

            dataString += "int" + splitChar + "date" + splitChar + (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + "\n";

            // Save the data to the file

            // If any of the directories in savePath don't exist, create them

            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            if (startComment != null && startComment != "")
            {
                dataString = "# " + startComment + "\n\n" + dataString;
            }

            if (endComment != null && endComment != "")
            {
                dataString += "\n\n# " + endComment;
            }

            File.WriteAllText(savePath, dataString);

            Debug.Log("BetterPrefs: Saved data to \"" + savePath + "\"");

            return savePath; // Return the path to the file, could be useful
        }
        else
        {
            Debug.Log("BetterPrefs: No data to save, deleting save file...");

            // Delete the save file

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            return null;
        }
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

    public static DateTime GetDate(string savePath = "current")
    {
        if (savePath == "current")
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(GetInt("date"));

            return dateTimeOffset.LocalDateTime;
        }
        else
        {
            // Get the date from the save file

            string[] lines = File.ReadAllLines(savePath);

            foreach (string line in lines)
            {
                if (line.StartsWith("int"))
                {
                    string[] parts = line.Split(splitChar);

                    if (parts.Length == 3 && parts[1] == "date")
                    {
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(int.Parse(parts[2]));

                        return dateTimeOffset.LocalDateTime;
                    }
                    else
                    {
                        Debug.LogError("BetterPrefs: Could not find the date in the save file");
                        return DateTime.MinValue;
                    }
                }
            }

            Debug.LogError("BetterPrefs: Could not find the date in the save file");

            return DateTime.MinValue;
        }
    }
}