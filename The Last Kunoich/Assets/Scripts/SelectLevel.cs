using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public Text slotName;
    public static int slot;
    // Start is called before the first frame update
    void Start()
    {
        if (slot == 1)
        {
            Save s = new Save();
            Save load = SaveLoad.LoadGameSlot1();
            slotName.text = load.inventory[0];
        }else if (slot == 2)
        {
            Save s = new Save();
            Save load = SaveLoad.LoadGameSlot2();
            slotName.text = load.inventory[0];
        }
    }
    public void OnClickRiceLevel()
    {
        
    }
    public void OnClickRiceLevelConfirm()
    {
        SceneManager.LoadScene("RiceLevel");
    }
    public void OnClickGunpowderLevel()
    {

    }
    public void OnClickGunpowderLevelConfirm()
    {
        
    }
    public void OnClickBossLevel()
    {

    }
    public void OnClickBossLevelConfirm()
    {
        
    }

    public void OnClickExit() 
    {
        SceneManager.LoadScene("Main Menu");
    }
}
