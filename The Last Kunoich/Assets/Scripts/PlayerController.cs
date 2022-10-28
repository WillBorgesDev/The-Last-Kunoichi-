using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
  //Healt
  public float maxHealth = 100f;
  private float currentHealth;
  private Image lifebar;
  private Image redBar;

  //Jump
  public float speed = 4f;
  public bool onJump = false;
  private int nJump;
  public float jumpSpeed = 4f;

  [SerializeField] private LayerMask layerGround;
  [SerializeField] private LayerMask layerClimb;

  SpriteRenderer sprite;
  Rigidbody2D body;
  BoxCollider2D bc2d;

  void Start()
  {
    sprite = GetComponent<SpriteRenderer>();
    body = GetComponent<Rigidbody2D>();
    bc2d = GetComponent<BoxCollider2D>();

    lifebar = GameObject.Find("LifeBar").GetComponent<Image>();
    redBar = GameObject.Find("RedBar").GetComponent<Image>();

    Debug.Log(lifebar);

    currentHealth = maxHealth;
  }

  // Update is called once per frame
  void Update()
  {
    Move();
    // MoveJoystick();

  }

  void Move()
  {
    if (Input.GetButtonDown("Jump") && isGrounded())
    {
      // anim.Play("Jump");
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
      Flip();
    }
    else if (horizontal > 0 && sprite.flipX == true)
    {
      Flip();
    }
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

  public void SetHealth(int amount)
  {
    if (amount < 0)
    {
      // Morreu
    }

    currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

    Vector3 lifebarScale = lifebar.rectTransform.localScale;
    lifebarScale.x = (float)currentHealth / maxHealth;
    lifebar.rectTransform.localScale = lifebarScale;
    StartCoroutine(DecreasingRedBar(lifebarScale));
  }

  private IEnumerator DecreasingRedBar(Vector3 newScale)
  {
    yield return new WaitForSeconds(0.5f);
    Vector3 redBarScale = redBar.transform.localScale;

    while (redBar.transform.localScale.x > newScale.x)
    {
      redBarScale.x -= Time.deltaTime * 0.25f;
      redBar.transform.localScale = redBarScale;

      yield return null;
    }

    redBar.transform.localScale = newScale;
  }
}
