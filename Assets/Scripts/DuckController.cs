using UnityEngine;
using UnityEngine.UI;

public class DuckController : MonoBehaviour {
	private int time = 0;
	private int thistime = 0;
	private int direction = 0;
	private int rotationAngle = 60;
	private float x = 1.15F;
	private float y = -0.03F;
	private Main.PositionLevel duckPosLevel;

	private void Start ()
 {
		if (transform.position.y <= -1.19)
			duckPosLevel = Main.PositionLevel.Down;
		else
			duckPosLevel = Main.PositionLevel.Up;
	}

	// Update is called once per frame
	private void Update()
	{
		if (Main.lifeNum == 0)
			Destroy(gameObject);

		thistime = Main.static_second;
		if (time != thistime)
		{
			time = thistime;
			if (time % 5 != 0)
			{
				if (direction > 0)
				{
					x = 0.75F;
					y = -0.15F;
					rotationAngle = 90;
				}
				if (transform.position.x > 0)
					x *= -1;
				if (direction < 4)
				{
					transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
					transform.Rotate(new Vector3(0, 0, transform.position.z + rotationAngle));
				}
				else
				{
					if (Main.lifeNum > 0)
					{
						Destroy(gameObject);
						GameObject life = GameObject.Find("life" + (4 - Main.lifeNum));
						life.transform.position = new Vector3(life.transform.position.x, -150, life.transform.position.z);
						Main.lifeNum--;
					}
				}
				direction++;
			}
			else if (direction == 4)
			{
				transform.position = new Vector3(transform.position.x, -3, transform.position.y);
				GetComponent<Image>().sprite = Resources.Load<Sprite>("die_duck");
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
