using UnityEngine;
using UnityEngine.UI;

public class DuckController : MonoBehaviour 
{
	[SerializeField]
	protected Sprite die_sprite;

	private int check_time = 0;
	private int cur_time = 0;
	private float move_time = 5;
	private int step_num = 0;
	private float startDownYPos = -1.19f; 
	private float dieYPos = -3f;
	private float x_add = 1.15F;
	private float y_add = -0.03F;
	private int rotationAngle = 60;
	private Main.PositionLevel duckPosLevel;
	private bool isRight => transform.position.x > 0;

	private void Start ()
 {
		if (transform.position.y <= startDownYPos)
			duckPosLevel = Main.PositionLevel.Down;
		else
			duckPosLevel = Main.PositionLevel.Up;
	}

	private void Update()
	{
		if (Main.lifeNum == 0)
			Destroy(gameObject);
		else
			MoveDuck();
	}

	private void MoveDuck()
	{ 
		cur_time = Main.static_second;
		if (check_time != cur_time)
		{
			check_time = cur_time;
			if (check_time % move_time != 0) //check time to move
			{
				if (step_num > 0) 
				{
					x_add = 0.75F;
					y_add = -0.15F;
					rotationAngle = 90;
				}
				if (isRight)
					x_add *= -1;
				if (step_num < 4)
				{
					transform.position = new Vector3(transform.position.x + x_add, transform.position.y + y_add, transform.position.z);
					transform.Rotate(new Vector3(0, 0, transform.position.z + rotationAngle));
				}
				else
				{
					if (Main.lifeNum > 0)
					{
						GameObject life = Main.instance.lifes[3 - Main.lifeNum];
						life.SetActive(false);
						Main.lifeNum--;
						Destroy(gameObject);
						return;
					}
				}
				step_num++;
			}
			else if (step_num == 4)
			{
				transform.position = new Vector3(transform.position.x, dieYPos, transform.position.y);
				GetComponent<Image>().sprite = die_sprite;
			}
		}
	}
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (((Main.heroPosLevel == Main.PositionLevel.LeftUp || Main.heroPosLevel == Main.PositionLevel.RigthUp) && duckPosLevel == Main.PositionLevel.Up)
			|| ((Main.heroPosLevel == Main.PositionLevel.LeftDown || Main.heroPosLevel == Main.PositionLevel.RightDown) && duckPosLevel == Main.PositionLevel.Down))

		{
			Main.instance.points++;
            Main.instance.pointLabel.text = Main.instance.points.ToString();
            Destroy(gameObject, 0.06F);
        }
    }
}
