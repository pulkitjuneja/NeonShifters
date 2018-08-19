using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public delegate IEnumerator State();

public enum states { StartMenu, Classic , Survial , GameOver} ;
public class manager : MonoBehaviour {

    State Current;
    Dictionary<states, State> Transitions;
    Vector2 LastlevPos;
    int score;
    bool classic;
    public GameObject[] Shapes ;
    public GameObject playerPrefab, Player;
    public static manager Instance;

    //ui elements
    public GameObject inGameui,GameOverUi,StartMenuUi;
    Text[] InGameTexts;
    Text FinalScore;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);
        Instance = this;
        InitStates();
    }
	IEnumerator Start () {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(Constants.camera.orthographicSize * Constants.camera.aspect*2, 0.1f);
        transform.position = new Vector3(Constants.camera.transform.position.x, Constants.camera.transform.position.y - Constants.camera.orthographicSize, 0);
        Current = StartMenu;
	    while(Current!=null)
        {
            yield return StartCoroutine(Current());
        }
	}

    void InitStates()
    {
        Transitions = new Dictionary<states, State>();
        Transitions.Add(states.Classic, Classic);
        Transitions.Add(states.GameOver, GameOver);
        Transitions.Add(states.Survial, Survival);
        Transitions.Add(states.StartMenu, StartMenu);
    }

    public void SetState(states state)
    {
        if(Transitions.ContainsKey(state))
        {
            Current = Transitions[state];
        }
    }
    //Game states
    IEnumerator StartMenu()
    {
        StartMenuUi.SetActive(true);
        float sptime = 3.0f,sptimer=0;
        while(Current == StartMenu)
        {
            sptimer += Time.unscaledDeltaTime;
            if(sptimer>=sptime)
            {
                Spawn();
                sptimer = 0.0f;
            }
            yield return null;
        }
        RemoveShapes();
        StartMenuUi.SetActive(false);

    }
    IEnumerator Survival()
    {
        inGameui.SetActive(true);
        score = 0;
        Constants.GameSpeed = 5f;
        classic = false;
        InGameTexts = inGameui.GetComponentsInChildren<Text>();
        InGameTexts[0].text = "0";
         float SptimeMax = 3.5f,SpawnTimer=0,levelClock = 0f;
         InGameTexts[2].text = levelClock.ToString("0");
        float Sptime = Random.Range(0.5f,SptimeMax),LevelTimer=0;
        var Spawnposy = Constants.BottomBound + playerPrefab.GetComponent<BoxCollider2D>().size.y*playerPrefab.transform.localScale.y/2;
        Player = Instantiate(playerPrefab, new Vector3(Constants.camera.transform.position.x, Spawnposy, 0), Quaternion.identity) as GameObject;
        while(Current == Survival)
        {
            SpawnTimer += Time.unscaledDeltaTime;
            LevelTimer += Time.unscaledDeltaTime;
            levelClock += Time.unscaledDeltaTime;
            if (SpawnTimer > Sptime)
            {
                Spawn();
                SpawnTimer = 0.0f;
                Sptime = Random.Range(0.5f, SptimeMax);
            }
            if (LevelTimer > 1.0f && SptimeMax > 2.0f)
            {
                SptimeMax -= 0.0167f;
                InGameTexts[2].text = levelClock.ToString("0");
                LevelTimer = 0.0f;
            }
            if(score<0)
            {
                SetState(states.GameOver);
            }
            yield return null;
        }
        if (Player != null)
            Destroy(Player);
        score = (int)levelClock;
        inGameui.SetActive(false);
        RemoveShapes();
        InGameTexts = null;
    }
    IEnumerator Classic()
    {
        inGameui.SetActive(true);
        score = 0;
        Constants.GameSpeed = 5f;
        classic = true;
        InGameTexts = inGameui.GetComponentsInChildren<Text>();
        InGameTexts[0].text = "0";
        float SptimeMax = 3.5f,SpawnTimer=0,levelClock = 120f;
        InGameTexts[2].text = levelClock.ToString("0");
        float Sptime = Random.Range(0.5f,SptimeMax),LevelTimer=0;
        var Spawnposy = Constants.BottomBound + playerPrefab.GetComponent<BoxCollider2D>().size.y*playerPrefab.transform.localScale.y/2;
        Player = Instantiate(playerPrefab, new Vector3(Constants.camera.transform.position.x, Spawnposy, 0), Quaternion.identity) as GameObject;
        while(Current == Classic)
        {
            SpawnTimer+=Time.unscaledDeltaTime;
            LevelTimer+=Time.unscaledDeltaTime;
            levelClock -= Time.unscaledDeltaTime;
            if(SpawnTimer>Sptime)
            {
                Spawn();
                SpawnTimer = 0.0f;
                Sptime = Random.Range(0.5f, SptimeMax);
            }
            if(LevelTimer>1.0f && SptimeMax > 1.0f)
            {
                SptimeMax -= 0.0167f;
                InGameTexts[2].text = levelClock.ToString("0") ;
                LevelTimer = 0.0f;
            }
            if(levelClock<=0)
            {
                SetState(states.GameOver);
            }
            yield return null;
 
        }
        if (Player != null)
            Destroy(Player);
        inGameui.SetActive(false);
        RemoveShapes();
        InGameTexts = null;
    }

    IEnumerator GameOver()
    {
        GameOverUi.SetActive(true);
        FinalScore = GameOverUi.GetComponentInChildren<Text>();
        if (classic)
            FinalScore.text = "Final score " + score;
        else
            FinalScore.text = score + " seconds survived";
        while(Current == GameOver)
        {
			if(EventSystem.current.currentSelectedGameObject!=null && EventSystem.current.currentSelectedGameObject.name == "Restart")
			{
			             if (classic)
				               SetState(states.Classic);
				         else
				               SetState(states.Survial);
				EventSystem.current.SetSelectedGameObject (null);
			}
			else  if(EventSystem.current.currentSelectedGameObject!=null && EventSystem.current.currentSelectedGameObject.name == "Exit")
            {
                SetState(states.StartMenu);
				EventSystem.current.SetSelectedGameObject (null);
            }
            yield return null;
        }
        FinalScore = null;
        GameOverUi.SetActive(false);
    }


    //States end 

    void Spawn()
    {
        float HorPadding = 0.7f;
        float spxmin = Constants.leftBound + HorPadding;
        float spxmax =Constants.RightBound - HorPadding;
        float spy = Constants.TopBound + 1.0f,spx;
        if (LastlevPos == Vector2.zero)
            spx = Random.Range(spxmin, spxmax);
        else
        {
            int r = Random.Range(0, 2);
            if (r == 0)
                spx = Random.Range(spxmin, LastlevPos.x - 0.5f);
            else
                spx = Random.Range(LastlevPos.x + 0.5f, spxmax);
        }
        var prob = Random.Range(0, 8);
        if (prob > 0)
            Instantiate(Shapes[Random.Range(0, Shapes.Length - 1)], new Vector3(spx, spy, 0), Quaternion.identity);
        else
            Instantiate(Shapes[Shapes.Length - 1], new Vector3(spx, spy, 0), Quaternion.identity);
    }

    void RemoveShapes()
    {
        var gs = GameObject.FindGameObjectsWithTag("Respawn");
        foreach(var g in gs)
        {
            Destroy(g);
        }
    }

    public void UpdateBulletUi(int bullets)
    {
        if (InGameTexts != null)
            InGameTexts[1].text = bullets.ToString();
    }
    public void setScore(int val)
    {
        score += val;
        if(InGameTexts!=null)
           InGameTexts[0].text = score.ToString();
    }
	
}
