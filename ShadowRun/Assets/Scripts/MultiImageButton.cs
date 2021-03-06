﻿using System;
using UnityEngine;
using UnityEngine.UI;

// Did not write this class
// very useful for my upgrades though
public class MultiImageButton : Button
{

	private Graphic[] m_graphics;
	protected Graphic[] Graphics
	{
		get
		{
			if (m_graphics == null)
			{
				m_graphics = targetGraphic.transform.parent.GetComponentsInChildren<Graphic>();
			}
			return m_graphics;
		}
	}

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		Color color;
		switch (state)
		{
			case SelectionState.Normal:
				color = colors.normalColor;
				break;
			case SelectionState.Highlighted:
				color = colors.highlightedColor;
				break;
			case SelectionState.Pressed:
				color = colors.pressedColor;
				break;
			case SelectionState.Disabled:
				color = colors.disabledColor;
				break;
			default:
				color = Color.black;
				break;
		}
		if (gameObject.activeInHierarchy)
		{
			switch (transition)
			{
				case Transition.ColorTint:
					ColorTween(color * colors.colorMultiplier, instant);
					break;
				default:
					throw new NotSupportedException();
			}
		}
	}

	private void ColorTween(Color targetColor, bool instant)
	{
		if (targetGraphic == null)
		{
			return;
		}

		foreach (Graphic g in Graphics)
		{
			g.CrossFadeColor(targetColor, (!instant) ? colors.fadeDuration : 0f, true, true);
		}
	}
}