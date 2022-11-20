using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  enum State
  {
    idle,
    moving,
    atack,
    dead
  }

  //Enemy
  [Header("Enemy")]
  private GameObject enemy;
  private State currentState;
  private Rigidbody2D rb;
  private Animator anim;
  private float startTime;
  private SpriteRenderer sp;

  //Move
  [Header("Move")]
  public float walkSpeed = 2f;
  public Transform[] points;
  private int destPoint = 0;
  private Vector2 _initialPosition;
  private bool _mustPatrol = true;

  //Idle
  [Header("Idle")]
  public float idleTime = 2f;
  private bool isAdleTimeOver = false;

  //Healt
  [Header("Healt")]
  public float _maxHealth = 100f;
  private float _currentHealth, kockBackStartTime;
  private Image _lifebarImage, _redBarImage;

  [Header("Atack")]
  public float rayToIdentifyPlayer = 5f;
  public float _damage = 10;
  public float maxGoWhenDetectPlayer = 10f;

  [Header("Ground Config")]
  public LayerMask playerLayer;
  public LayerMask groundLayer;
  public float radius = 0.3f;
  private Transform groundCheckPos;

  void Start()
  {
    enemy = this.gameObject;
    currentState = State.moving;

    rb = enemy.GetComponent<Rigidbody2D>();
    sp = enemy.GetComponent<SpriteRenderer>();
    anim = enemy.GetComponent<Animator>();

    var hud = enemy.transform.Find("HUD");
    var barBG = hud.transform.Find("Life Bar BG");

    groundCheckPos = enemy.transform.Find("GroundCheckPosition");
    _lifebarImage = barBG.transform.Find("LifeBarEnemy").gameObject.GetComponent<Image>();
    _redBarImage = barBG.transform.Find("RedBarEnemy").gameObject.GetComponent<Image>();


    _currentHealth = _maxHealth;
    _initialPosition = points.Length >= 0 ? points[0].position : enemy.transform.position;
    startTime = Time.time;

    TurnAround();
  }

  void Update()
  {
    switch (currentState)
    {
      case State.idle:
        Flip();
        break;
      case State.moving:
        Moving();
        break;
      case State.atack:
        Atack();
        break;
      default:
        break;
    }
  }

  void FixedUpdate()
  {
    VerifyHasPlayer();

    if (_mustPatrol)
    {
      var mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, radius, groundLayer);

      if (mustTurn) SwitchState(State.idle);
    }
  }

  void SwitchState(State newState)
  {
    startTime = Time.time;
    currentState = newState;
  }

  void Flip()
  {
    _mustPatrol = false;

    if (Time.time >= startTime + idleTime)
    {
      SwitchState(State.moving);
      TurnAround();
      _mustPatrol = true;
    }
  }

  void TurnAround()
  {
    if (points != null)
    {
      if (points[destPoint].position.x < enemy.transform.position.x)
      {
        sp.flipX = true;
      }
      else
      {
        sp.flipX = false;
      }
    }
    else
    {
      if (sp.flipX)
      {
        sp.flipX = false;
        walkSpeed *= -1;
      }
      else
      {
        sp.flipX = true;
        walkSpeed *= 1;
      }
    }
  }

  void TurnGroundCheck()
  {
    groundCheckPos.transform.Rotate(0f, 180f, 0f);
  }

  void Moving()
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

  void Patrol()
  {
    rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
  }

  void GotoNextPoint()
  {
    enemy.transform.position = Vector2.MoveTowards(new Vector2(enemy.transform.position.x, 1), new Vector2(points[destPoint].position.x, 1), walkSpeed * Time.deltaTime);


    if (Vector2.Distance(enemy.transform.position, points[destPoint].position) < 0.2f)
    {
      destPoint = (destPoint + 1) % points.Length;

      SwitchState(State.idle);
    }
  }

  void VerifyHasPlayer()
  {
    RaycastHit2D[] hits = Physics2D.RaycastAll(enemy.transform.position, !sp.flipX ? Vector2.right : Vector2.left, rayToIdentifyPlayer, playerLayer);

    Debug.DrawRay(enemy.transform.position, !sp.flipX ? Vector2.right : Vector2.left);

    if (hits != null)
    {
      foreach (RaycastHit2D hit in hits)
      {
        var playerTag = hit.collider.tag == "Player";

        if (playerTag && enemy.transform.position.x < _initialPosition.x - maxGoWhenDetectPlayer)
        {
          SwitchState(State.idle);
        }
        else if (playerTag && enemy.transform.position.x > _initialPosition.x)
        {
          SwitchState(State.idle);
        }
        else if (playerTag)
        {
          SwitchState(State.atack);
        }
      }
    }
  }

  void Atack()
  {
    var player = GameObject.Find("Player");

    enemy.transform.position = Vector2.MoveTowards(new Vector2(enemy.transform.position.x, 1), player.transform.position, walkSpeed * Time.deltaTime);

    if (player.transform.position.x < enemy.transform.position.x)
    {
      sp.flipX = true;
    }
    else
    {
      sp.flipX = false;
    }

    if (Vector2.Distance(enemy.transform.position, player.transform.position) < 0.2f)
    {
      //TODO ATACK ANimation
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

    Vector3 _lifebarScale = _lifebarImage.rectTransform.localScale;
    _lifebarScale.x = (float)_currentHealth / _maxHealth;
    _lifebarImage.rectTransform.localScale = _lifebarScale;
    StartCoroutine(DecreasingRedBar(_lifebarScale));
  }

  private IEnumerator DecreasingRedBar(Vector3 newScale)
  {
    yield return new WaitForSeconds(0.5f);
    Vector3 _redBarScale = _redBarImage.transform.localScale;

    while (_redBarImage.transform.localScale.x > newScale.x)
    {
      _redBarScale.x -= Time.deltaTime * 0.25f;
      _redBarImage.transform.localScale = _redBarScale;

      yield return null;
    }

    _redBarImage.transform.localScale = newScale;
  }

  void OnDrawGizmosSelected()
  {
    if (groundCheckPos)
    {
      Gizmos.DrawWireSphere(groundCheckPos.position, radius);
    }
  }

}
