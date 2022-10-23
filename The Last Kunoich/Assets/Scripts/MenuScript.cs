using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Camera main;
    public Camera startGame;
    public Camera NewGame;
    public Camera NewExists;
    public Camera LoadGame;
    public Camera config;
    public Camera credits;

    public Text txtExist;
    public int existSlot;
    

    // Start is called before the first frame update
    void Start()
    {
        main.depth = 1;
    }

    public void OnClickStartGame()
    {
        main.depth = 0;
        startGame.depth = 1;    
    }

    public void OnClickNewGame()
    {
        startGame.depth = 0;
        NewGame.depth = 1;
    }

    public void NewGameSlot1()
    {
        Save s = new Save();
        Save load = SaveLoad.LoadGameSlot1();
        
        if(load != null)
        {
            slotExistPainel(1);
        } else {
            CreateNewSave(1);
        }
    }

    public void NewGameSlot2()
    {
        Save s = new Save();
        Save load = SaveLoad.LoadGameSlot2();

        if(load != null)
        {
            slotExistPainel(2);
        } else {
            CreateNewSave(2);
        }
    }

    public void CreateNewSave(int slot)
    {
        SaveLoad.getSlotToSave(slot);
        SelectLevel.slot = slot;

        SceneManager.LoadScene("SelectLevel");
    }

    public void slotExistPainel(int slot)
    {
        txtExist.text = "O Slot " + slot + " ja esta sendo usado."; 
        NewGame.depth = 0;
        NewExists.depth = 1;
        existSlot = slot;
    }
    public void OverwriteSlot()
    {
        if (existSlot == 1)
        {
            SaveLoad.getSlotToSave(1);
            SelectLevel.slot = 1;
            existSlot = 0;
            SceneManager.LoadScene("SelectLevel");
        } else if(existSlot == 2) {

            SaveLoad.getSlotToSave(2);
            SelectLevel.slot = 2;
            existSlot = 0;
            SceneManager.LoadScene("SelectLevel");
        }
    }

    public void NotOverwriteSlot()
    {
        NewGame.depth = 1;
        NewExists.depth = 0;
    }

    public void OnClickLoadGame()
    { 
        startGame.depth = 0;
        LoadGame.depth = 1;
    }
    public void LoadGameSlot1()
    {
        SelectLevel.slot = 1;

        SceneManager.LoadScene("SelectLevel");
    }

    public void LoadGameSlot2()
    {
        SelectLevel.slot = 2;

        SceneManager.LoadScene("SelectLevel");
    }
    public void OnClickConfig()
    {
        main.depth = 0;
        config.depth = 1;
    }
    public void OnClickCredits()
    {
        main.depth = 0;
        credits.depth = 1;  
    }
    public void OnClickVoltar()
    {
        startGame.depth = 0;
        config.depth = 0;
        credits.depth = 0;  
        main.depth = 1;
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

}
