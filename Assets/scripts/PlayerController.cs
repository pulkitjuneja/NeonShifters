using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof( AudioSource))]

public class PlayerController : MonoBehaviour {

	System.Collections.Generic.Dictionary<Shapes,Color> ShapeDictionary;
    Shapes currentShape;
    ParticleSystem Smoke,DestroyPart;
    public float PlayerSpeed;
    public LayerMask CastLayers;
    AudioSource Asource;
    public AudioClip[] sounds;
    bool invincible;
	int moveDirection ;
    bool shouldMove;
    int bullets =5;
    public Sprite[] ShapeSprites;
    SpriteRenderer sp;
    CameraShake cs;

	void Start () {
        shouldMove = true;
        Asource = GetComponent<AudioSource>();
        var ps = GetComponentsInChildren<ParticleSystem>();
        DestroyPart = ps[0];
        Smoke = ps[1];
        manager.Instance.UpdateBulletUi(bullets);
        cs = Constants.camera.GetComponent<CameraShake>();
        sp = GetComponent<SpriteRenderer>();
        Smoke.renderer.sortingLayerName = "particles";
        currentShape = Shapes.circle;
		GameObject.Find ("UIButtons").GetComponent<TriggerSetup> ().setUpTriggers (this);
        AddShapes();    
		Debug.Log ("start complete");
	}

	void Update () 
    {
      if(shouldMove)
        Move();
	}

		void Move()
    {
			var Horpadding = 0.7f;
			float leftBound = Constants.leftBound + Horpadding, rightBound = Constants.RightBound - Horpadding;
		    transform.Translate (Vector2.right *moveDirection* PlayerSpeed*Time.deltaTime);
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x, leftBound, rightBound), transform.position.y);
    }

	public void ToggleMoving(int dir)
	{
		Debug.Log ("moving");
		moveDirection = dir;
	}

	public void stopMoving()
	{
		moveDirection = 0;
	}

	public  void FireBullets()
    {
            if (bullets > 0)
            {
			if (Asource != null) {
				Asource.clip = sounds [2];
				Asource.Play ();
			}
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 19f, CastLayers);

                if (hit.collider != null)
                {
                        var script = hit.collider.gameObject.GetComponent<SpawnedObject>();
                        cs.Shake();
                        manager.Instance.setScore(1);
                        StartCoroutine(script.destroy());
                }
                else
                {
                    
                }
                bullets--;
                manager.Instance.UpdateBulletUi(bullets);
            }
            else
            {
                Asource.clip = sounds[1];
                Asource.Play();
            }
    }

	public void ShapeShift(Shapes sh)
	{
		if (currentShape != sh) {
			DestroyPart.startColor	= ShapeDictionary [sh];		
			StartCoroutine (Ssc (sh));
		}
	}
		
    void AddShapes()
    {
		ShapeDictionary = new System.Collections.Generic.Dictionary<Shapes, Color> ();
		ShapeDictionary.Add(Shapes.circle,new Color(255,0,0));
		ShapeDictionary.Add (Shapes.square, new Color (235, 255, 47));
		ShapeDictionary.Add (Shapes.pentagon, new Color (0, 255, 65));
		ShapeDictionary.Add (Shapes.triangle, new Color (0, 181, 255));
    }

    IEnumerator Ssc(Shapes shape)
    {
        Smoke.Play();
        Asource.clip = sounds[0];
        Asource.Play();
        currentShape = shape;
		Debug.Log (currentShape);
        yield return new  WaitForSeconds(0.2f);
		sp.sprite = ShapeSprites[(int)shape];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var script = other.gameObject.GetComponent<SpawnedObject>();
        if (script == null)
            return;
        if(script.shape == Shapes.Bullets)
        {
            bullets += 5;
            manager.Instance.UpdateBulletUi(bullets);
            StartCoroutine(script.Absorb());
        }
        else if(script.shape == Shapes.Invincible)
        {

        }
        else if (script.shape == currentShape)
        {
            manager.Instance.setScore(1);
            StartCoroutine(script.destroy());
        }
        else
        {
            StartCoroutine(destroyAnim());
        }
    }

    IEnumerator destroyAnim()
    {
        sp.enabled = false;
        DestroyPart.Play();
        shouldMove = false;
        Asource.clip = sounds[3];
        Asource.Play();
        GetComponent<BoxCollider2D>().enabled= false;
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
		GameObject.Find ("UIButtons").GetComponent<TriggerSetup> ().removetriggers();
        manager.Instance.SetState(states.GameOver);
    }
}
