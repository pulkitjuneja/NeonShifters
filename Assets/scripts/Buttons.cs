using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Buttons : MonoBehaviour {


	void Start () {
	   Button[] buttons = GetComponentsInChildren<Button>();
       foreach (Button butt in buttons)
       {
           switch (butt.gameObject.name)
           {
               case "Classic": butt.onClick.AddListener(() => ClassicListener()); break;
               case "Survival": butt.onClick.AddListener(() => SurvivalListener()); break;
               case "Exit": butt.onClick.AddListener(() => ExitListener()); break;
           }
       }
	}

    void ClassicListener()
    {

        manager.Instance.SetState(states.Classic);
        
    }

    void SurvivalListener()
    {
        manager.Instance.SetState(states.Survial);
    }

    void ExitListener()
    {
        Application.Quit();
    }
	
}
