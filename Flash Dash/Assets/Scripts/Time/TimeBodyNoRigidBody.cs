using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBodyNoRigidBody : MonoBehaviour
{
	bool isRewinding = false;

	public float recordTime = 5f;

	List<PointInTime> pointsInTime;

	void Start()
	{
		pointsInTime = new List<PointInTime>();
	}

	void FixedUpdate()
	{
		if (isRewinding)
			Rewind();
		else
			Record();
	}

	void Rewind()
	{
		if (pointsInTime.Count > 0)
		{
			PointInTime pointInTime = pointsInTime[0];
			transform.position = pointInTime.position;
			transform.rotation = pointInTime.rotation;
			pointsInTime.RemoveAt(0);
		}
		else
		{
			StopRewind();
		}

	}

	void Record()
	{
		if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
		{
			pointsInTime.RemoveAt(pointsInTime.Count - 1);
		}

		pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
	}

	public void StartRewind()
	{
		isRewinding = true;
	}

	public void StopRewind()
	{
		isRewinding = false;
	}
}

