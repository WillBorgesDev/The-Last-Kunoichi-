using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

  public static int flipState;
  enum State
  {
    idle,
    patrol,
    followPlayer,
    atack,
    dead
  }

  struct AnimTypes
  {
    public const string attack = "attack";
    public const string walking = "walking";
    public const string idle = "idle";
  }
  public float positionY;
  public bool isBoss;
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
  private bool _mustPatrol = true;

  //Idle
  [Header("Idle")]
  public float idleTime = 2f;
  private bool isAdleTimeOver = false;

  //Healt
  [Header("Healt")]
  public float _maxHealth = 100f;
  private float _currentHealth;
  private Image _lifebarImage, _redBarImage;


  [Header("Atack")]
  // public float _damage = 10;
  public float distanceToNotAtack = 5f;

  [Header("Ground Config")]
  public LayerMask playerLayer;
  public LayerMask groundLayer;
  public float radius = 0.3f;
  private Transform groundCheckPos;

  void Start()
  {
    enemy = this.gameObject;
    currentState = State.patrol;

    rb = enemy.GetComponent<Rigidbody2D>();
    sp = enemy.GetComponent<SpriteRenderer>();
    anim = enemy.GetComponent<Animator>();

    var hud = enemy.transform.Find("HUD");
    var barBG = hud.transform.Find("Life Bar BG");

    groundCheckPos = enemy.transform.Find("GroundCheckPosition");
    _lifebarImage = barBG.transform.Find("LifeBarEnemy").gameObject.GetComponent<Image>();
    _redBarImage = barBG.transform.Find("RedBarEnemy").gameObject.GetComponent<Image>();

    _currentHealth = _maxHealth;
    startTime = Time.time;

    TurnAround();
  }

  void Update()
  {
    if (currentState != State.atack)
    {
      VerifyHasPlayer();
    }

    switch (currentState)
    {
      case State.idle:
        Flip();
        break;
      case State.patrol:
        Moving();
        break;
      case State.followPlayer:
        FollowPLayer();
        break;
      case State.atack:
        Attack();
        break;
      default:
        break;
    }
  }

  void FixedUpdate()
  {
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

    anim.Play(AnimTypes.idle);

    if (Time.time >= startTime + idleTime)
    {
      if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
      {
        SwitchState(State.patrol);
        TurnAround();
        _mustPatrol = true;
      }
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
      anim.Play(AnimTypes.walking);

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
    enemy.transform.position = Vector2.MoveTowards(new Vector2(enemy.transform.position.x, positionY), new Vector2(points[destPoint].position.x, positionY), walkSpeed * Time.deltaTime);


    if (Vector2.Distance(enemy.transform.position, points[destPoint].position) < 0.2f)
    {
      destPoint = (destPoint + 1) % points.Length;

      SwitchState(State.idle);
    }
  }

  void VerifyHasPlayer()
  {
    RaycastHit2D[] hits = Physics2D.RaycastAll(enemy.transform.position, !sp.flipX ? Vector2.right : Vector2.left, distanceToNotAtack, playerLayer);

    Debug.DrawRay(enemy.transform.position, !sp.flipX ? Vector2.right : Vector2.left);

    if (hits != null)
    {
      foreach (RaycastHit2D hit in hits)
      {
        var playerTag = hit.collider.tag == "Player";

        if (playerTag)
        {
          SwitchState(State.followPlayer);
        }
      }
    }
  }

  void FollowPLayer()
  {
    var player = GameObject.Find("Player");
    var distance = Vector2.Distance(player.transform.position, enemy.transform.position);

    enemy.transform.position = Vector2.MoveTowards(new Vector2(enemy.transform.position.x, positionY), player.transform.position, walkSpeed * Time.deltaTime);

    if (distance > distanceToNotAtack)
    {
      SwitchState(State.idle);
      return;
    }

    anim.Play(AnimTypes.walking);

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

    if (Vector2.Distance(enemy.transform.position, player.transform.position) < 2f)
    {
      SwitchState(State.atack);
    }
  }

  void Attack()
  {
    anim.Play(AnimTypes.attack);

    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 3 && !anim.IsInTransition(0))
    {
      SwitchState(State.followPlayer);
    }
  }

  // void OnCollisionEnter2D(Collision2D other)
  // {
  //   if (other.gameObject.tag == "Player")
  //   {
  //     var player = other.transform.GetComponent<PlayerController>();

  //     if (player != null)
  //     {
  //       player.TakeDamage(_damage);
  //     }
  //   }
  // }

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
