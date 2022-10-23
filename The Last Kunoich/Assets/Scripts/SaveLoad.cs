using System.Security.AccessControl;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Data.SqlTypes;
using System.Resources;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
       

        // Save load = LoadGame();

        // Debug.Log(load.inventory[0]);
        
    }
    public static void getSlotToSave(int slot)
    {
        if(slot == 1)
        {
            Save s = new Save();
            s.score = 25;
            s.inventory = new List<string>();
            s.inventory.Add("Slot 1");

            SaveGame(s);
        }else if(slot == 2)
        {
            Save s = new Save();
            s.score = 25;
            s.inventory = new List<string>();
            s.inventory.Add("Slot 2");

            SaveGame(s);
        }
        

    }

    public static void SaveGame(Save s) 
    {
        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath;

        if(s.inventory[0] == "Slot 1" )
        {
            FileStream file = File.Create(path + "/savegame1.save");

            bf.Serialize(file, s);
            file.Close();

            Debug.Log("Game Saved");
        } else if(s.inventory[0] == "Slot 2" )
        {
            FileStream file = File.Create(path + "/savegame2.save");

            bf.Serialize(file, s);
            file.Close();

            Debug.Log("Game Saved");
        }
        
    }
    
    // Update is called once per frame
    public static Save LoadGameSlot1()
    {
        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath;

        FileStream file;
        
        
            if (File.Exists(path + "/savegame1.save"))
            {
                file = File.Open(path + "/savegame1.save", FileMode.Open);
                Save load = (Save)bf.Deserialize(file);
                file.Close();

                Debug.Log("Game 1 loaded");

                return load;
            }  
        return null;
    }

    public static Save LoadGameSlot2()
    {
        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath;

        FileStream file;
        
            if (File.Exists(path + "/savegame2.save"))
            {
                file = File.Open(path + "/savegame2.save", FileMode.Open);
                Save load = (Save)bf.Deserialize(file);
                file.Close();

                Debug.Log("Game 2 loaded");

                return load;
            }  
        return null;
    }
}
