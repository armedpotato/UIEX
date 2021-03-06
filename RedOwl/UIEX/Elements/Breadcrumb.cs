﻿#if UNITY_EDITOR
#pragma warning disable 0649 // UXMLReference variable declared but not assigned to.
using System;
using UnityEngine;
using UnityEditor.UIElements;
using RedOwl.Editor;

namespace RedOwl.Editor
{
	[UXML, USSClass("container", "row")]
	public class Breadcrumb : RedOwlVisualElement
	{
		[UXMLReference]
		private ToolbarButton button;
		
		[UXMLReference]
		private FontAwesome separator;

		private int index;
        private string text;
		private Action<int> callback;
		    	
		public Breadcrumb(int index, string text, Action<int> callback) : base()
		{
			this.index = index;
            this.text = text;
			this.callback = callback;
			button.text = text;
			button.clickable.clicked += () => { callback(index); };
		}

		public void ShowSeparator()
		{
			separator.Show();
		}
    }
}
#endif
