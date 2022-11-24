using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject painel;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void deathPainel()
    {
        painel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void restart()
    {
        Debug.Log("clico");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void sair()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void credit()
    {
        SceneManager.LoadScene("Credits");
    }
}
