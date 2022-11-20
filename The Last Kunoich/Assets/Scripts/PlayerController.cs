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
  public Image _lifebar;
  public Image _redBar;

  //Jump
  float speed = 6f;
  public bool onJump = false;
  private int nJump;
  float jumpSpeed = 12f;

  //Weapon
  public bool basicAttack = false;
  public float _damage = 10;
  public static int targetSide;
  bool hasShuriken;


  [SerializeField] private LayerMask layerGround;
  [SerializeField] private LayerMask layerClimb;

  SpriteRenderer sprite;
  Rigidbody2D body;
  BoxCollider2D bc2d;
  Animator anim;
  Image icon;

  void Start()
  {
    sprite = GetComponent<SpriteRenderer>();
    body = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    bc2d = GetComponent<BoxCollider2D>();
    icon = GetComponent<Image>();
    
    _lifebar = GameObject.Find("LifeBar").GetComponent<Image>();
    _redBar = GameObject.Find("RedBar").GetComponent<Image>();

    _currentHealth = _maxHealth;
  }

  // Update is called once per frame
  void Update()
  {
    Move();
    // MoveJoystick();
    if (Input.GetKeyDown(KeyCode.J))
        {
           basicAttack = true;
           speed = speed * 0;
        }
    if (Input.GetKeyDown(KeyCode.K))
        {
          if(targetSide == 1){
            Instantiate(shuriken, targetR.position, targetR.rotation);
          } else {
            Instantiate(shuriken, targetL.position, targetL.rotation);
          } 
        }
    if (Input.GetKeyDown(KeyCode.L))
        {
          if(targetSide == 1){
            Instantiate(bomb, targetR.position, targetR.rotation);
          } else {
            Instantiate(bomb, targetL.position, targetL.rotation);
          }
          
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
  void MoveJoystick()
  {
    float horizontalJoystick = Input.GetAxis("HorizontalJoystick");

    body.velocity = new UnityEngine.Vector2(horizontalJoystick * speed, body.velocity.y);

    if (Input.GetButtonDown("JumpJoystick") && isGrounded())
    {
      // anim.Play("Jump");
      Jump();
    }

    if (isClimb())
    {
      anim.Play("climb");
      float vertical = Input.GetAxis("VerticalJoystick");
      body.velocity = new UnityEngine.Vector2(body.velocity.x, vertical * speed / 2);
    }


    if (horizontalJoystick < 0 && sprite.flipX == false)
    {
      Flip();
    }
    else if (horizontalJoystick > 0 && sprite.flipX == true)
    {
      Flip();
    }
  }
  void PlayerAnimator()
    {
        // Iniciando animações do player
        if(body.velocity.x == 0 && body.velocity.y == 0 && isGrounded() && !isClimb() && !basicAttack)
        {
            anim.Play("idle");
            //Inicia a animação dela parada
        }
        else if(body.velocity.x != 0 && isGrounded() && !isClimb() && !basicAttack)
        {
            anim.Play("Running");
            //Inicia a animação dela correndo 
        } else if (body.velocity.x != 0 && isGrounded() && !isClimb() && basicAttack)
        {
           anim.Play("BasicAttack");
        }
        else if(body.velocity.y != 0 && body.velocity.x != 0) 
        {
            // anim.Play("Jump");
        } else if (isClimb() && !basicAttack)
        {
          anim.Play("climb");
        } else if (basicAttack)
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
}
