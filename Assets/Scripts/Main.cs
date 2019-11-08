using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour 
{
	[SerializeField]
	protected GameObject hero;
	[SerializeField]
	protected GameObject chair;
	[SerializeField]
	protected GameObject duck;
	[SerializeField]
	protected GameObject end;
	[SerializeField]
	protected GameObject restart;
	[SerializeField]
	protected Image emotions;
	[SerializeField]
	protected Text timer;
	[SerializeField]
	protected Transform duckContainer;

	public Text pointLabel;

	[HideInInspector]
	public int points = 0;

	public enum PositionLevel
	{
		LeftUp,
		LeftDown,
		RigthUp,
		RightDown,
		Down,
		Up
	}

	public static Main instance;
	public static PositionLevel heroPosLevel;
	public static int lifeNum = 3;
	public static int static_second = 0;

	private SpriteRenderer heroSprite;
	private bool isFacingRight;
	private bool duckIsFacingRight;
	private bool pause = false;
	private float centerX = 0;
	private float centerY = -2.19F;
	private float chairX;
	private float deltaTime = 0.03F;
	private float deltaPlus = 0.01F;
	private double Timer;
	private double GameTime;
	private int gen_time = 5, point_crit = 10, gener_allow = 0;

    private void Start()
 {
		instance = this;
        isFacingRight = false;
        duckIsFacingRight = false;
        centerX = 0;
        centerY = -2.19F;
        chairX = chair.transform.position.x;
        chair.transform.position = new Vector3(chairX, -10, 0);
        end.SetActive(false);
        restart.SetActive(false);
		heroSprite = hero.GetComponent<SpriteRenderer>();
    }

	private void OnDestroy()
	{
		instance = null;
	}

	private void Update()
    {
        if (restart.activeInHierarchy == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
                Go(PositionLevel.LeftUp);
            else if (Input.GetKeyDown(KeyCode.R))
                Go(PositionLevel.LeftDown);
            else if (Input.GetKeyDown(KeyCode.U))
                Go(PositionLevel.RightDown);
            else if (Input.GetKeyDown(KeyCode.I))
                Go(PositionLevel.RigthUp);
        }
    }
    private void FixedUpdate()
    {
        if (!pause)
        {
			int point = points;

            if(point == point_crit)
            {
                if (point % 50 == 0)
                    deltaPlus -= (deltaPlus / 3F);
                point_crit += 10;
                deltaTime += deltaPlus;
            }

			Sprite emotSprite = null;
            if (point == 10)
				emotSprite = Resources.Load<Sprite>("emot4");
            else if (point == 25)
				emotSprite = Resources.Load<Sprite>("emot8");
            else if (point == 50)
				emotSprite = Resources.Load<Sprite>("emot1");
            else if (point == 100)
				emotSprite = Resources.Load<Sprite>("emot7");
            else if (point == 150)
				emotSprite = Resources.Load<Sprite>("emot2");
            else if (point == 200)
				emotSprite = Resources.Load<Sprite>("emot3");
            else if (point == 250)
				emotSprite = Resources.Load<Sprite>("emot5");
            else if (point == 350)
				emotSprite = Resources.Load<Sprite>("emot6");
			emotions.sprite = emotSprite;

            if (point > 49 && point < 100)
                heroSprite.sprite = Resources.Load<Sprite>("olya6");
            else if (point > 99 && point < 200)
				heroSprite.sprite = Resources.Load<Sprite>("olya2");
            else if (point > 199 && point < 350)
				heroSprite.sprite = Resources.Load<Sprite>("olya3");
            else if (point > 349)
				heroSprite.sprite = Resources.Load<Sprite>("olya4");

            if (lifeNum != 0)
            {
                Timer += deltaTime;
                double second = System.Math.Round(Timer % 60, 3);
                Timer -= Timer % 60;
                double minute = Timer / 60;
                Timer = (minute * 60) + second;
                static_second = (int)second;
                if (second % gen_time < 0.5 && gener_allow == 0)
                {
                    Generating();
                    gener_allow++;
                }
                else if (second % gen_time >= 0.5)
                    gener_allow = 0;

                GameTime += 0.01;
                double GameSecond = System.Math.Round(GameTime % 60, 3);
                GameTime -= GameTime % 60;
                double GameMinute = GameTime / 60;
                GameTime = (GameMinute * 60) + GameSecond;
                string str = "";
                if (GameMinute < 10)
                    str = "0" + GameMinute + ":";
                else
                    str = GameMinute + ":";
                if (GameSecond < 10)
                    str += "0" + (int)GameSecond;
                else
                    str += (int)GameSecond;
                timer.text = str;
            }
            else
            {
				heroSprite.sprite = Resources.Load<Sprite>("olya5");
                end.SetActive(true);
                restart.SetActive(true);
                hero.transform.position = new Vector3(0, -2.19F, 0);
                chair.transform.position = new Vector3(chairX, -10F, 0);
            }
        }
    }
    private void Flip(GameObject obj)
    {
        //if (obj.name == "Duck")
        //    duckIsFacingRight = !duckIsFacingRight;
        Vector3 scale = obj.transform.localScale;
		scale.x = isFacingRight ? -1 : 1;
        obj.transform.localScale = scale;
    }

	public void Go(PositionLevel posLvl)
	{
		if (restart.activeInHierarchy == false)
		{
			heroPosLevel = posLvl;
			float transformX = 0.73F, transformY = 1.18F;

			if (heroPosLevel == PositionLevel.LeftUp || heroPosLevel == PositionLevel.RigthUp)
				chair.transform.position = new Vector3(chairX, -2.72F, 0);
			else if (heroPosLevel == PositionLevel.LeftDown || heroPosLevel == PositionLevel.RightDown)
			{
				transformY = 0;
				chair.transform.position = new Vector3(chairX, -10F, 0);
			}
			if (heroPosLevel == PositionLevel.RightDown || heroPosLevel == PositionLevel.RigthUp)
			{
				isFacingRight = true;
				Flip(hero);
			}
			else if (heroPosLevel == PositionLevel.LeftUp || heroPosLevel == PositionLevel.LeftDown)
			{
				isFacingRight = false;
				transformX *= -1;
				Flip(hero);
			}
			hero.transform.position = new Vector3(centerX + transformX, centerY + transformY, hero.transform.position.z);
		}
	}

    private void Generating()
    {
        int r = Random.Range(0, 4);//0-ru, 1-rd, 2-lu, 3-ld
        if (r == 0)
            Instantiate(duck, new Vector3(5.15F, 0.2F, 0), Quaternion.identity, duckContainer);
        else if (r == 1)
            Instantiate(duck, new Vector3(5.15F, -1.19F, 0), Quaternion.identity, duckContainer);
        else if (r == 2)
            Instantiate(duck, new Vector3(-5.1F, 0.2F, 0), Quaternion.Euler(0, 180, 0), duckContainer);
        else if (r == 3)
            Instantiate(duck, new Vector3(-5.1F, -1.19F, 0), Quaternion.Euler(0, 180, 0), duckContainer);
    }
    public void Restart()
    {
        end.SetActive(false);
        restart.SetActive(false);
        Timer = 0;
        GameTime = 0;
        lifeNum = 3;
        deltaTime = 0.03F;
        gener_allow = 0;
        point_crit = 10;
        static_second = 0;
		points = 0;
		pointLabel.text = points.ToString();
        emotions.sprite = Resources.Load<Sprite>("shark");
		heroSprite.sprite = Resources.Load<Sprite>("olya1");
        for (int i = 1; i < 4; i++)
        {
            GameObject life = GameObject.Find("life" + i);
            life.transform.position = new Vector3(life.transform.position.x, 264, life.transform.position.z);
        }
    }

	public void BtnLeftUp()
	{
		Go(PositionLevel.LeftUp);
	}
	public void BtnLeftDown()
	{
		Go(PositionLevel.LeftDown);
	}
	public void BtnRightUp()
	{
		Go(PositionLevel.RigthUp);
	}
	public void BtnRightDown()
	{
		Go(PositionLevel.RightDown);
	}

	public void Pause()
    {
        if (!pause)
            pause = true;
        else
            pause = false;
    }
    public void Exit()
    {
        Restart();
        SceneManager.LoadScene("StartMenu");
    }
}
