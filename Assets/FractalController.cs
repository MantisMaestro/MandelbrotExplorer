using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class FractalController : MonoBehaviour
{

	public Material mat;

	public TextMeshProUGUI text;

	public Vector2 pos;
	public float scale, angle, speed, colour, colourRepeat;

	private Vector2 smoothPos;
	private float smoothScale, smoothAngle;

	private bool showDebug = false;


	private void Start()
	{
		text.enabled = showDebug;
	}

	private void UpdateShader()
	{
		smoothPos = Vector2.Lerp(smoothPos, pos, 0.03f);
		smoothScale = Mathf.Lerp(smoothScale, scale, 0.03f);
		smoothAngle = Mathf.Lerp(smoothAngle, angle, 0.03f);

		float aspect = (float)Screen.width / (float)Screen.height;

		float scaleX = smoothScale;
		float scaleY = smoothScale;

		if (aspect > 1f)
		{
			scaleY /= aspect;
		}
		else
		{
			scaleX *= aspect;
		}

		colour = Mathf.Clamp01(colour);

		mat.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
		mat.SetFloat("_Angle", smoothAngle);
		mat.SetFloat("_Speed", speed);
		mat.SetFloat("_Colour", colour);
		mat.SetFloat("_Repeat", colourRepeat);
	}

	private void HandleInputs()
	{
		//Zoom
		if (Input.GetKey(KeyCode.KeypadPlus))
		{
			scale *= 0.99f;
		}

		if (Input.GetKey(KeyCode.KeypadMinus))
		{
			scale *= 1.01f;
		}

		//Move
		Vector2 dir = new Vector2(0.01f * scale, 0);
		float s = Mathf.Sin(angle);
		float c = Mathf.Cos(angle);
		dir = new Vector2(dir.x * c, dir.x * s);

		if (Input.GetKey(KeyCode.A))
		{
			pos -= dir;
		}

		if (Input.GetKey(KeyCode.D))
		{
			pos += dir; ;
		}

		dir = new Vector2(-dir.y, dir.x);

		if (Input.GetKey(KeyCode.W))
		{
			pos += dir;
		}

		if (Input.GetKey(KeyCode.S))
		{
			pos -= dir;
		}

		//Rotate
		if (Input.GetKey(KeyCode.Q))
		{
			angle += 0.01f;
		}

		if (Input.GetKey(KeyCode.E))
		{
			angle -= 0.01f;
		}

		//Speed
		if (Input.GetKey(KeyCode.Z))
		{
			speed -= 0.01f;
		}

		if (Input.GetKey(KeyCode.X))
		{
			speed += 0.01f;
		}

		//Colour
		if (Input.GetKey(KeyCode.UpArrow))
		{
			colour += 0.01f;
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			colour -= 0.01f;
		}

		//Colour
		if (Input.GetKey(KeyCode.RightArrow))
		{
			colourRepeat += 0.01f;
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			colourRepeat -= 0.01f;
		}
	}

	private void HandleInputsEveryFrame()
	{
		//Debug Text
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			showDebug = !showDebug;
			Debug.Log("Toggle Debug");
		}
	}

	private void UpdateDebugText()
	{
		text.enabled = showDebug;

		StringBuilder sb = new StringBuilder();
		sb.AppendFormat("Smooth Pos X:{0} Y:{1}", smoothPos.x, smoothPos.y);
		sb.AppendLine();
		sb.AppendFormat("Raw Pos X:{0} Y:{1}", pos.x, pos.y);
		sb.AppendLine();
		sb.AppendFormat("Smooth Scale {0}", smoothScale);
		sb.AppendLine();
		sb.AppendFormat("Raw Scale {0}", scale);
		sb.AppendLine();
		sb.AppendFormat("Smooth Angle {0}", smoothAngle);
		sb.AppendLine();
		sb.AppendFormat("Raw Angle {0}", angle);
		sb.AppendLine();
		sb.AppendFormat("Speed {0}", speed);
		sb.AppendLine();
		sb.AppendFormat("Colour {0}", colour);
		sb.AppendLine();
		sb.AppendFormat("Colour Repeat {0}", colourRepeat);
		sb.AppendLine();

		text.text = sb.ToString();
	}

	void FixedUpdate()
	{
		HandleInputs();
		UpdateShader();
	}

	private void Update()
	{
		HandleInputsEveryFrame();
		UpdateDebugText();
	}
}
