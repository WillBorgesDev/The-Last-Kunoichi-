using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  //Enemy
  [Header("Enemy")]
  private GameObject enemy;
  private Rigidbody2D rb;

  //Move
  [Header("Move")]
  public bool _mustPatrol = true;
  public bool mustTurn = true;
  public float walkSpeed = 7f;
  public Transform[] points;
  private int destPoint = 0;

  //Healt
  [Header("Healt")]
  public float _maxHealth = 100f;
  private float _currentHealth, kockBackStartTime;
  private Image _lifebar;
  private Image _redBar;

  [Header("Atack")]
  public float _damage = 10;

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

    _currentHealth = _maxHealth;
  }

  void Update()
  {
    if (_mustPatrol)
    {
      if (points.Length > 0)
      {
        GotoNextPoint();
      }
      else
      {
        Patrol();
      }
    }
  }

  void FixedUpdate()
  {
    if (_mustPatrol)
    {
      mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, radius, groundLayer);
    }
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
    if (points.Length == 0) walkSpeed *= -1;
    _mustPatrol = true;
  }

  void GotoNextPoint()
  {
    // Set the agent to go to the currently selected destination.
    enemy.transform.position = Vector2.MoveTowards(new Vector2(enemy.transform.position.x, 1), new Vector2(points[destPoint].position.x, 1), walkSpeed * Time.deltaTime);

    if (Vector2.Distance(enemy.transform.position, points[destPoint].position) < 0.2f)
    {
      destPoint = (destPoint + 1) % points.Length;
      Flip();
    }
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
