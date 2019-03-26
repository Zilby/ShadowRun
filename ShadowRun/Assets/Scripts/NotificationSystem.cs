using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Notification system for creating notifications for the player. 
/// </summary>
public class NotificationSystem : MonoBehaviour
{

	public delegate void NotificationAction(string s, float dur = 2f);
	/// <summary>
	/// Displays the given notification text. 
	/// </summary>
	public static NotificationAction DisplayNotification;

	private Fadeable fadeable;
	private TextMeshProUGUI text;
	private Button button;
	private Coroutine co;


	/*
	private void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			// Example notification system call. 
			NotificationSystem.DisplayNotification("This is a test");
		}
	}
	*/

	private void Awake()
	{
		DisplayNotification = NewNotification;
		fadeable = GetComponentInChildren<Fadeable>();
		text = GetComponentInChildren<TextMeshProUGUI>();
		button = GetComponentInChildren<Button>();
		button.onClick.AddListener(StopDisplay);
	}

	/// <summary>
	/// Creates a new notification. 
	/// </summary>
	private void NewNotification(string s, float dur)
	{
		StopDisplay();
		co = StartCoroutine(ShowNotification(s, dur));
	}

	/// <summary>
	/// Stops displaying a notification. 
	/// </summary>
	private void StopDisplay()
	{
		if (co != null)
		{
			StopCoroutine(co);
			co = null;
			fadeable.Hide();
		}
	}

	/// <summary>
	/// Shows the notification.
	/// </summary>
	private IEnumerator ShowNotification(string s, float dur)
	{
		text.text = s;
		yield return fadeable.FadeIn(dur: 0.2f);
		yield return new WaitForSecondsRealtime(dur);
		yield return fadeable.FadeOut(dur: 0.2f);
	}
}
