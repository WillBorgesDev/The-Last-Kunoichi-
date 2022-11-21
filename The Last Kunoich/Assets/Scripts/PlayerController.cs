using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
 
  //instance gameobject
  public GameObject shuriken;
  public GameObject bomb;
	public Transform targetR;
  public Transform targetL;
  //Healt
  public float _maxHealth = 100f;
  private float _currentHealth;

  //HUD
  public Image _lifebar;
  public Image _redBar;
  public Image shurikenCount;
  public Image bombCount;
  public Image dashCount;
  private float timerSuriken = 5;
  private float timerBomb = 5;

  //Jump
  float speed = 6f;
  public bool onJump = false;
  private int nJump;
  float jumpSpeed = 12f;
  

  //Weapon
  public bool basicAttack = false;
  public float _damage = 10;
  public static int targetSide;
  bool startCountShuriken;
  bool startCountBomb;
  bool startCountDash;
  bool hasShuriken = true;
  bool hasBomb = true;
  
  //Dash
  bool hasDash = true;
  bool isDashing;
  float dashSpeed = 12f;
  float dashingTime = 0.2f;
  float dashingCoolDown = 5f;

  [SerializeField] private LayerMask layerGround;
  [SerializeField] private LayerMask layerClimb;

  SpriteRenderer sprite;
  Rigidbody2D body;
  BoxCollider2D bc2d;
  Animator anim;


  void Start()
  {
    sprite = GetComponent<SpriteRenderer>();
    body = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    bc2d = GetComponent<BoxCollider2D>();
    
    startCountShuriken = false;
    _lifebar = GameObject.Find("LifeBar").GetComponent<Image>();
    _redBar = GameObject.Find("RedBar").GetComponent<Image>();

    _currentHealth = _maxHealth;
  }

  // Update is called once per frame
  void Update()
  {
    if(isDashing)
    {
      return;
    }
    Move();
    shurikenCoolDownTimer();
    bombCoolDownTimer();
    // MoveJoystick();
    if (Input.GetKeyDown(KeyCode.J))
      {
         basicAttack = true;
         speed = speed * 0;
      }
    if (Input.GetKeyDown(KeyCode.K) && hasShuriken == true)
      {
        startCountShuriken = true;
        if(targetSide == 1){
          Instantiate(shuriken, targetR.position, targetR.rotation);
        } else {
          Instantiate(shuriken, targetL.position, targetL.rotation);
        } 
        hasShuriken = false;
      }
    if (Input.GetKeyDown(KeyCode.L) && hasBomb == true)
      {
        startCountBomb = true;
        if(targetSide == 1){
          Instantiate(bomb, targetR.position, targetR.rotation);
        } else {
          Instantiate(bomb, targetL.position, targetL.rotation);
        }
        hasBomb = false;      
      }
  }
  void Move()
  {
    if (Input.GetButtonDown("Jump") && isGrounded())
    {
      Jump();
    }

    if (isClimb())
    {
      float vertical = Input.GetAxis("Vertical");
      body.velocity = new UnityEngine.Vector2(body.velocity.x, vertical * speed / 2);
    }

    float horizontal = Input.GetAxis("Horizontal");
    body.velocity = new UnityEngine.Vector2(horizontal * speed, body.velocity.y);

    if (Input.GetKeyDown(KeyCode.H) && hasDash == true)
    {
      if(sprite.flipX == true)
      {
        dashSpeed = dashSpeed * -1;
      }
      StartCoroutine(Dash());
    }

    if (horizontal < 0 && sprite.flipX == false)
    {
      targetSide = -1;
      shuriken.GetComponent<ShurikenController>().side = targetSide;
      bomb.GetComponent<BombController>().side = targetSide;
      Flip();
    }
    else if (horizontal > 0 && sprite.flipX == true)
    {
      targetSide = 1;
      shuriken.GetComponent<ShurikenController>().side = targetSide;
      bomb.GetComponent<BombController>().side = targetSide;
      Flip();
    }

    PlayerAnimator();
  }
  public void shurikenCoolDownTimer()
  {
    if(startCountShuriken == true)
      {
        if(timerSuriken > 0)
        {
          shurikenCount.GetComponent<Image>().color = new Color32(125,125,125,255);
          timerSuriken -= Time.deltaTime;
        } else if(timerSuriken <= 0)
        {
          startCountShuriken = false;
          shurikenCount.GetComponent<Image>().color = new Color32(255,255,255,255);
          hasShuriken = true;
        } 
      } else {
        timerSuriken = 5;
      }
  }

  public void bombCoolDownTimer()
  {
    if(startCountBomb == true)
      {
        if(timerBomb > 0)
        {
          bombCount.GetComponent<Image>().color = new Color32(125,125,125,255);
          timerBomb -= Time.deltaTime;
        } else if(timerBomb <= 0)
        {
          startCountBomb = false;
          bombCount.GetComponent<Image>().color = new Color32(255,255,255,255);
          hasBomb = true;
        } 
      } else {
        timerBomb = 5;
      }
  }

  IEnumerator Dash()
  {
    anim.Play("StartDash");
    hasDash = false;
    isDashing = true;
    float originalGravity = body.gravityScale;
    body.gravityScale = 0f;
    body.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
    yield return new WaitForSeconds(dashingTime);
    anim.Play("FinishDash");
    dashCount.GetComponent<Image>().color = new Color32(125,125,125,255);
    body.gravityScale = originalGravity;
    yield return new WaitForSeconds(dashingCoolDown);
    dashCount.GetComponent<Image>().color = new Color32(255,255,255,255);
    hasDash = true;
    dashSpeed = 12f;  
  }
  void PlayerAnimator()
    {
        // Iniciando animações do player
        if(body.velocity.x == 0 && body.velocity.y == 0 && isGrounded() && !isClimb() && !basicAttack && !isDashing)
        {
            anim.Play("idle");
            //Inicia a animação dela parada
        }
        else if(body.velocity.x != 0 && isGrounded() && !isClimb() && !basicAttack && !isDashing)
        {
            anim.Play("Running");
            //Inicia a animação dela correndo 
        } else if (body.velocity.x != 0 && isGrounded() && !isClimb() && basicAttack && !isDashing)
        {
           anim.Play("BasicAttack");
        }
        else if(body.velocity.y > 0 && !isGrounded() && !isClimb() && !isDashing) 
        {
            anim.Play("Jump");
        } else if (isClimb() && !basicAttack && !isDashing)
        {
          anim.Play("climb");
        } else if (basicAttack && !isDashing)
        {
           anim.Play("BasicAttack");
        }
        
    }

  private bool isGrounded()
  {
    RaycastHit2D ground = Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0, UnityEngine.Vector2.down, 0.1f, layerGround);

    return ground.collider != null;
  }
  private bool isClimb()
  {
    RaycastHit2D climbR = Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0, UnityEngine.Vector2.right, 0.1f, layerClimb);
    RaycastHit2D climbL = Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0, UnityEngine.Vector2.left, 0.1f, layerClimb);
    return (climbR.collider != null) || (climbL.collider != null);
  } 
  void Jump()
  {
    body.AddForce(new UnityEngine.Vector2(0, jumpSpeed), ForceMode2D.Impulse);
  }

  void Flip()
  {
    sprite.flipX = !sprite.flipX;
  }
  // void OnTriggerEnter2D(Collision2D other)
  // {
  //   if (other.gameObject.tag == "Arrow")
  //   {
  //     var arrow = other.transform.GetComponent<ArrowAction>();

  //     if (arrow != null)
  //     {
  //       arrow.TakeDamage(_damage);
  //     }
  //   }
  // }

  void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Enemy")
    {
      var enemy = other.transform.GetComponent<EnemyController>();

      if (enemy != null)
      {
        enemy.TakeDamage(_damage);
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

  void finishAttack()
  {
    basicAttack = false;
    speed = 6f;
  }
  void finishDash()
  {
    isDashing = false;
  }
  void continuousAir()
  {
    anim.Play("Air Continuous");
  }
}
