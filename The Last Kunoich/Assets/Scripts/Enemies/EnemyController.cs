using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  //Enemy
  [Header("Enemy")]
  public Vector2 _initialPosition;
  private GameObject enemy;
  private Rigidbody2D rb;

  //Move
  [Header("Move")]
  public bool _mustPatrol = true;
  public bool mustTurn = true;
  private bool alredyTurn = false;
  public float walkSpeed = 100f;
  public float maxXGo = 5f;
  public float minXGo = 1f;

  [Header("Atack")]
  public float _damage = 10;

  //Healt
  [Header("Healt")]
  public float _maxHealth = 100f;
  private float _currentHealth, kockBackStartTime;
  private Image _lifebar;
  private Image _redBar;

  [Header("Ground Config")]
  public LayerMask groundLayer;
  public float radius = 0.3f;
  private Transform groundCheckPos;

  void Start()
  {
    enemy = this.gameObject;

    rb = enemy.GetComponent<Rigidbody2D>();

    var hud = enemy.transform.Find("HUD");
    var barBG = hud.transform.Find("Life Bar BG");

    groundCheckPos = enemy.transform.Find("GroundCheckPosition");
    _lifebar = barBG.transform.Find("LifeBarEnemy").gameObject.GetComponent<Image>();
    _redBar = barBG.transform.Find("RedBarEnemy").gameObject.GetComponent<Image>();

    _initialPosition = enemy.transform.position;
    _currentHealth = _maxHealth;
  }

  void Update()
  {
    if (_mustPatrol)
    {
      Patrol();
    }
  }

  void FixedUpdate()
  {
    if (_mustPatrol)
    {
      mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, radius, groundLayer);

      if (rb.position.x > _initialPosition.x + maxXGo && !alredyTurn)
      {
        alredyTurn = true;
        mustTurn = true;
        Invoke("Teste", 1);
      }


      if (rb.position.x < _initialPosition.x - minXGo && !alredyTurn)
      {
        alredyTurn = true;
        mustTurn = true;
        Invoke("Teste", 1);
      }

      Debug.Log(alredyTurn);

    }
  }

  void Teste()
  {
    alredyTurn = false;
    mustTurn = false;
  }

  private IEnumerator AwaitToFlip(float time)
  {
    yield return new WaitForSeconds(time);
    alredyTurn = false;
  }

  void Patrol()
  {
    if (mustTurn)
    {
      Flip();
    }

    rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
  }

  void Flip()
  {
    _mustPatrol = false;
    enemy.transform.Rotate(0f, 180f, 0f);
    walkSpeed *= -1;
    _mustPatrol = true;
  }

  void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      var player = other.transform.GetComponent<PlayerController>();

      if (player != null)
      {
        player.TakeDamage(_damage);
      }
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

    // SwitchState(State.Knockback);


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

  void OnDrawGizmosSelected()
  {
    if (groundCheckPos)
    {
      Gizmos.DrawWireSphere(groundCheckPos.position, radius);
    }
  }

}
