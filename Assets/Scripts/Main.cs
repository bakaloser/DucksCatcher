using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour 
{
	[SerializeField]
	protected GameObject maskot;
	[SerializeField]
	protected List<Sprite> maskot_sprites;
	[SerializeField]
	protected GameObject chair;
	[SerializeField]
	protected Transform duckContainer;
	[SerializeField]
	protected GameObject duck_pref;
	[SerializeField]
	protected GameObject end_bg;
	[SerializeField]
	protected GameObject restartBtn;
	[SerializeField]
	protected Text timer;

	public Text pointLabel;
	public List<GameObject> lifes;

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
        centerX = 0;
        centerY = -2.19F;
        chairX = chair.transform.position.x;
        chair.transform.position = new Vector3(chairX, -10, 0);
        end_bg.SetActive(false);
        restartBtn.SetActive(false);
		heroSprite = maskot.GetComponent<SpriteRenderer>();
    }

	private void OnDestroy()
	{
		instance = null;
	}

	private void Update()
    {
        if (restartBtn.activeInHierarchy == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
                Move(PositionLevel.LeftUp);
            else if (Input.GetKeyDown(KeyCode.R))
                Move(PositionLevel.LeftDown);
            else if (Input.GetKeyDown(KeyCode.U))
                Move(PositionLevel.RightDown);
            else if (Input.GetKeyDown(KeyCode.I))
                Move(PositionLevel.RigthUp);
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

			HeroSpriteUpdate(point);

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
                    DuckGenerating();
                    gener_allow++;
                }
                else if (second % gen_time >= 0.5)
                    gener_allow = 0;

                GameTime += 0.01;
                double GameSecond = System.Math.Round(GameTime % 60, 3);
                GameTime -= GameTime % 60;
                double GameMinute = GameTime / 60;
                GameTime = (GameMinute * 60) + GameSecond;
				string str;
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
				heroSprite.sprite = maskot_sprites[4];
                end_bg.SetActive(true);
                restartBtn.SetActive(true);
                maskot.transform.position = new Vector3(0, -2.19F, 0);
                chair.transform.position = new Vector3(chairX, -10F, 0);
            }
        }
    }
    private void Flip(GameObject obj)
    {
        Vector3 scale = obj.transform.localScale;
		scale.x = isFacingRight ? -1 : 1;
        obj.transform.localScale = scale;
    }

	public void Move(PositionLevel posLvl)
	{
		if (restartBtn.activeInHierarchy == false)
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
				Flip(maskot);
			}
			else if (heroPosLevel == PositionLevel.LeftUp || heroPosLevel == PositionLevel.LeftDown)
			{
				isFacingRight = false;
				transformX *= -1;
				Flip(maskot);
			}
			maskot.transform.position = new Vector3(centerX + transformX, centerY + transformY, maskot.transform.position.z);
		}
	}

    private void DuckGenerating()
    {
        int r = Random.Range(0, 4);//0-ru, 1-rd, 2-lu, 3-ld
        if (r == 0)
            Instantiate(duck_pref, new Vector3(5.15F, 0.2F, 0), Quaternion.identity, duckContainer);
        else if (r == 1)
            Instantiate(duck_pref, new Vector3(5.15F, -1.19F, 0), Quaternion.identity, duckContainer);
        else if (r == 2)
            Instantiate(duck_pref, new Vector3(-5.1F, 0.2F, 0), Quaternion.Euler(0, 180, 0), duckContainer);
        else if (r == 3)
            Instantiate(duck_pref, new Vector3(-5.1F, -1.19F, 0), Quaternion.Euler(0, 180, 0), duckContainer);
    }
    public void Restart()
    {
        end_bg.SetActive(false);
        restartBtn.SetActive(false);
        Timer = 0;
        GameTime = 0;
        lifeNum = 3;
        deltaTime = 0.03F;
        gener_allow = 0;
        point_crit = 10;
        static_second = 0;
		points = 0;
		pointLabel.text = points.ToString();
		heroSprite.sprite = maskot_sprites[0];
        for (int i = 0; i < 3; i++)
        {
			lifes[i].SetActive(true);
        }
    }

	private void HeroSpriteUpdate(int point)
	{
		if (point > 49 && point < 100)
			heroSprite.sprite = maskot_sprites[5];
		else if (point > 99 && point < 200)
			heroSprite.sprite = maskot_sprites[1];
		else if (point > 199 && point < 350)
			heroSprite.sprite = maskot_sprites[2];
		else if (point > 349)
			heroSprite.sprite = maskot_sprites[3];
	}

	#region Buttons
	public void BtnLeftUp()
	{
		Move(PositionLevel.LeftUp);
	}
	public void BtnLeftDown()
	{
		Move(PositionLevel.LeftDown);
	}
	public void BtnRightUp()
	{
		Move(PositionLevel.RigthUp);
	}
	public void BtnRightDown()
	{
		Move(PositionLevel.RightDown);
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
	#endregion
}
