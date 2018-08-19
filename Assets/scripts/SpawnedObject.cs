using UnityEngine;
using System.Collections;

public enum Shapes:int { circle=0 , square=1 , pentagon , triangle, Bullets , Invincible};
public class SpawnedObject : MonoBehaviour {

    public Shapes shape;
    float speed;
    bool shouldmove= true;
    Collider2D bx;
    public AudioClip[] Sounds;
    SpriteRenderer sp;
    ParticleSystem[] effects;

    void Start()
    {
        speed = Random.Range(Constants.GameSpeed - 3.0f, Constants.GameSpeed + 2.0f);
        bx = GetComponent<Collider2D>();
        effects = GetComponentsInChildren<ParticleSystem>();
        sp = GetComponent<SpriteRenderer>();
    }
	// Update is called once per frame
	void Update () {
        if(shouldmove)
        transform.Translate(-Vector2.up *speed * Time.deltaTime,Space.World);      
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Destroyer")
        {
            Destroy(this.gameObject);
            manager.Instance.setScore(-1);
        }
    }

    public IEnumerator destroy()
    {
        effects[0].Play();
        Constants.AudioCenter.PlayOneShot(Sounds[0]);
        bx.enabled = false;
        shouldmove = false;
        sp.enabled = false;
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

   public IEnumerator Absorb()
    {
        if (effects.Length <= 1)
            yield break;
        effects[1].Play();
        Constants.AudioCenter.PlayOneShot(Sounds[1]);
        bx.enabled = false;
        shouldmove = false;
        sp.enabled = false;
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
