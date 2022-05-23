# BetterPrefs

BetterPrefs is a replacement for Unity's PlayerPrefs that aims to add features that PlayerPrefs is lacking, such as support for multiple saves, save import/export and even more data types, such as booleans, Vector2s and Vector3s.

BetterPrefs is designed to be similar to PlayerPrefs, so switching is super easy.

License: [MIT](https://opensource.org/licenses/MIT)

## Features

- **Multiple saves**: Save data can be saved to multiple files, and the user can switch between them.
- **Serialization**: Saves are serialized, which makes them harder to edit (PlayerPrefs usually stores them in plain-text or in registry, making them super easy to edit and to cheat) ([more info](#serialization)
- **Save import/export**: Save data can be imported and exported, which is very useful for sharing save data between different devices.
- **Cross-platform**: Save data can be saved to a file on the desktop, and loaded from a file on Android, etc. All platforms Unity supports can read the files in the same way.
- **Data types**: BetterPrefs supports more data types than Unity's PlayerPrefs, such as booleans, Vector2s and Vector3s.
- **Open source**: The source code is public, unlike Unity's PlayerPrefs.
- **More to come**: More features are coming soon, like importing old PlayerPrefs saves and converting them to BetterPrefs

## Getting Started

To start using BetterPrefs, you need to add the [BetterPrefs.cs](https://github.com/Carroted/BetterPrefs/blob/master/BetterPrefs.cs) script to your Unity project.

Once you've added the script, you can use the BetterPrefs class to access your save data.

```csharp
BetterPrefs.Load("/saves/example.save"); // Loads the save data from "/saves/example.save", or, if it doesn't exist, loads a blank save and remembers to save at that location

BetterPrefs.SetBool("My Bool", true);
BetterPrefs.SetInt("My Int", 5);
BetterPrefs.SetFloat("My Float", 5.5f);
BetterPrefs.SetString("My String", "Hello World!\nNewline supported!");
BetterPrefs.SetVector2("My Vector2", new Vector2(1.25f, 5.7f));
BetterPrefs.SetVector3("My Vector3", new Vector3(1.25f, 5.7f, 3.5f));

BetterPrefs.Save(); // Saves the data to the current save file, in this case "/saves/example.save"
BetterPrefs.Save("/saves/backups/game.save"); // Saves a backup of the data to the file "/saves/backups/game.save"
```

Unlike PlayerPrefs, BetterPrefs lets you easily choose which save file to load and save to.

In the above example, we even create a backup of the data, so that if something goes wrong, we can still load the data we saved before.

BetterPrefs doesn't automatically save the data on quit due to Unity limitations (scripts that don't derive from MonoBehaviour can't access Unity's [OnApplicationQuit](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationQuit.html) event), but this can actually be helpful, as you can create your own dialog that asks if you want to save or not.

You are recommended to create a "save manager", which can take many forms. You could have a UI on game start which asks which save you want to load from a few slots (if people share devices), you could automatically load a save from a certain location, etc. This save manager could run `BetterPrefs.Save` in [OnApplicationQuit](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationQuit.html).

If you want to automatically load a specific save when you enter the game, calling `BetterPrefs.Load` on [Awake](https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html) is a great way to start.

## Migration from PlayerPrefs

If your Unity project currently uses PlayerPrefs, switching should be easy.

The instructions in [Getting Started](#getting-started) should be helpful.

```csharp
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExampleSaveLoader : MonoBehaviour
{
    void Awake()
    {
        BetterPrefs.Load(); // You can specify a path if you want, or you could make a UI where you can choose your save
    }
}
```

Then, you can use the BetterPrefs class to access your save data.

If you do want to have a UI to manage saves in your game, you should make sure your code doesn't try to access data before the BetterPrefs.Load method has been called. A great way to do this is to make that UI be in its own scene.

You can get a list of saves by listing files in the saves directory, which, by default, is `Application.persistentDataPath + "/saves"`, and then add a list of save options the user can choose from, and call BetterPrefs.Load on the chosen save.

Once your save manager is done and ready, you might be able to simply do a find and replace on your scripts from `PlayerPrefs` to `BetterPrefs`. (can be done easily in most IDEs). **MAKE SURE TO HAVE A BACKUP BEFORE DOING THIS!**

It's also worth noting that old PlayerPrefs saves currently aren't supported, and if many people already have your game and have saves in it, those saves will be lost. This should only be an issue before release.

## Methods

- **BetterPrefs.Load()**: Loads the save data from the default save file.
- **BetterPrefs.Load(string path)**: Loads the save data from the specified file.
- **BetterPrefs.Save()**: Saves the save data into the save that was Loaded.
- **BetterPrefs.Save(string path)**: Saves the data to the specified file.
- **BetterPrefs.SetBool(string key, bool value)**: Sets the value of the specified key to the specified boolean value.
- **BetterPrefs.SetInt(string key, int value)**: Sets the value of the specified key to the specified integer value.
- **BetterPrefs.SetFloat(string key, float value)**: Sets the value of the specified key to the specified float value.
- **BetterPrefs.SetString(string key, string value)**: Sets the value of the specified key to the specified string value.
- **BetterPrefs.SetVector2(string key, Vector2 value)**: Sets the value of the specified key to the specified Vector2 value.
- **BetterPrefs.SetVector3(string key, Vector3 value)**: Sets the value of the specified key to the specified Vector3 value.
- **BetterPrefs.GetBool(string key)**: Gets the value of the specified key as a boolean.
- **BetterPrefs.GetInt(string key)**: Gets the value of the specified key as an integer.
- **BetterPrefs.GetFloat(string key)**: Gets the value of the specified key as a float.
- **BetterPrefs.GetString(string key)**: Gets the value of the specified key as a string.
- **BetterPrefs.GetVector2(string key)**: Gets the value of the specified key as a Vector2.
- **BetterPrefs.GetVector3(string key)**: Gets the value of the specified key as a Vector3.
- **BetterPrefs.DeleteKey(string key)**: Deletes the specified key.
- **BetterPrefs.DeleteAll()**: Deletes all keys.
- **BetterPrefs.GetDate()**: Gets the `DateTime` of the currently loaded save file, or the current date if the save hasn't been Saved yet.
- **BetterPrefs.GetDate(string path)**: Gets the `DateTime` of when the specified save file was saved.
- **BetterPrefs.HasKey(string key)**: Checks if the specified key exists.

## Serialization

Saves are serialized using `System.Runtime.Serialization.Formatters.Binary`'s `BinaryFormatter`.

## Known Limitations

- PlayerPrefs saves can't be imported

## About

BetterPrefs was released on May 21, 2022 by [Alex_Sour](https://github.com/Alex-Sour) under the [MIT license](https://opensource.org/licenses/MIT).
