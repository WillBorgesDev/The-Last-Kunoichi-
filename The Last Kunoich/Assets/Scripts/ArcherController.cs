using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherController : MonoBehaviour
{


    //Healt
    public float _maxHealth = 100f;
    private float _currentHealth;

    //HUD
    public Image _lifebar;
    public Image _redBar;

    public GameObject arrowR;
    public GameObject arrowL;
    public Transform targetR;
    public Transform targetL;
    Animator anim; 
    public LayerMask playerLayer;
    public static bool isDetect;
    private GameObject enemy;
    private SpriteRenderer sp;
    private int flipState;
    public float positionY;

    void Start()
    {
        enemy = this.gameObject;
        sp = enemy.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        isDetect = false;
        var hud = enemy.transform.Find("HUD");
        var barBG = hud.transform.Find("Life Bar BG");

        _lifebar = barBG.transform.Find("LifeBarEnemy").gameObject.GetComponent<Image>();
        _redBar = barBG.transform.Find("RedBarEnemy").gameObject.GetComponent<Image>();

        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        DetectPLayer();
        if (isDetect == true)
        {
            attack();
        }
    }

    void DetectPLayer()
    {
        var player = GameObject.Find("Player");
        var distance = Vector2.Distance(player.transform.position, enemy.transform.position);

        if (player.transform.position.x < enemy.transform.position.x)
        {
        sp.flipX = true;
        flipState = -1;
        }
        else
        {
        sp.flipX = false;
        flipState = 1;
        }

        if (Vector2.Distance(enemy.transform.position, player.transform.position) < 18f)
        {
            attack();    
        } else {
            anim.Play("Idle");
        }
    }
    
    public void attack()
    {
        anim.Play("Attack");
    }

    public void instanceArrow()
    {
        if(flipState == 1)
        {
            Instantiate(arrowR, targetR.position, targetR.rotation);
        } else if(flipState == -1)
        {
            Instantiate(arrowL, targetL.position, targetL.rotation);
        }
        
    }

    public void TakeDamage(float damage)
    {
        SetHealth(damage, "damage");
    }

    public void Heal(float damage)
    {
        SetHealth(damage, "healing");
    }

    private void SetHealth(float amount, string type)
    {
        if (type == "healing")
        {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        }
        else if (type == "damage")
        {
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _maxHealth);
        }
        //Die
        if (_currentHealth <= 0)
        {
            Destroy(this.gameObject);
        // SwitchState(State.Dead);
        }
        Vector3 _lifebarScale = _lifebar.rectTransform.localScale;
        _lifebarScale.x = (float)_currentHealth / _maxHealth;
        _lifebar.rectTransform.localScale = _lifebarScale;
        StartCoroutine(DecreasingRedBar(_lifebarScale));
    }
    private IEnumerator DecreasingRedBar(Vector3 newScale)
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 _redBarScale = _redBar.transform.localScale;

        while (_redBar.transform.localScale.x > newScale.x)
        {
        _redBarScale.x -= Time.deltaTime * 0.25f;
        _redBar.transform.localScale = _redBarScale;

        yield return null;
        }

        _redBar.transform.localScale = newScale;
    }

    
    
}
