using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TriggerSetup : MonoBehaviour {

	EventTrigger left , right,shoot,circle,pentagon,triangle,square;

	void Start () {
		left = transform.FindChild ("left").gameObject.GetComponent<EventTrigger> ();
		right = transform.FindChild ("right").gameObject.GetComponent<EventTrigger> ();
		shoot = transform.FindChild ("shoot").gameObject.GetComponent<EventTrigger> ();
		circle = transform.FindChild ("circle").gameObject.GetComponent<EventTrigger> ();
		pentagon = transform.FindChild ("pentagon").gameObject.GetComponent<EventTrigger> ();
		triangle = transform.FindChild ("triangle").gameObject.GetComponent<EventTrigger> ();
		square =transform.FindChild ("square").gameObject.GetComponent<EventTrigger> ();
	}

	public void setUpTriggers(PlayerController player)
	{
		leftTrigger (player);
		rightTrigger (player);
		shootTrigger (player);
		shpaeTriggers (player);
    }

	void leftTrigger(PlayerController player)
	{
		EventTrigger.Entry e = new EventTrigger.Entry ();
		e.eventID = EventTriggerType.PointerDown;
		e.callback.AddListener(delegate {
			player.ToggleMoving(-1);
		});
		left.delegates.Add (e);
		e = new EventTrigger.Entry ();
		e.eventID = EventTriggerType.PointerUp;
		e.callback.AddListener (delegate {
			player.stopMoving();	
		});
		left.delegates.Add (e);
	}

	void rightTrigger(PlayerController player)
	{
		EventTrigger.Entry e = new EventTrigger.Entry ();
		e.eventID = EventTriggerType.PointerDown;
		e.callback.AddListener(delegate {
			player.ToggleMoving(1);
		});
		right.delegates.Add (e);
		e = new EventTrigger.Entry ();
		e.eventID = EventTriggerType.PointerUp;
		e.callback.AddListener (delegate {
			player.stopMoving();	
		});
		right.delegates.Add (e);
	}

	void shootTrigger(PlayerController pl)
	{
		EventTrigger.Entry e = new EventTrigger.Entry();
		e.eventID = EventTriggerType.PointerClick;
		e.callback.AddListener (delegate {
			pl.FireBullets();	
		});
		shoot.delegates.Add (e);
	}

	void shpaeTriggers(PlayerController pl)
	{
		EventTrigger.Entry e = new EventTrigger.Entry();
		e.eventID = EventTriggerType.PointerClick;
		e.callback.AddListener (delegate {
			pl.ShapeShift(Shapes.circle);	
		});
		circle.delegates.Add (e);

	    e = new EventTrigger.Entry();
		e.eventID = EventTriggerType.PointerClick;
		e.callback.AddListener (delegate {
			pl.ShapeShift(Shapes.square);	
		});
		square.delegates.Add (e);

		e = new EventTrigger.Entry();
		e.eventID = EventTriggerType.PointerClick;
		e.callback.AddListener (delegate {
			pl.ShapeShift(Shapes.triangle);	
		});
		triangle.delegates.Add (e);

		e = new EventTrigger.Entry();
		e.eventID = EventTriggerType.PointerClick;
		e.callback.AddListener (delegate {
			pl.ShapeShift(Shapes.pentagon);	
		});
		pentagon.delegates.Add (e);
	}

	public void removetriggers()
	{
		left.delegates.Clear();
		right.delegates.Clear();
		shoot.delegates.Clear();
		circle.delegates.Clear();
		square.delegates.Clear ();
		triangle.delegates.Clear ();
		pentagon.delegates.Clear ();
	}
}