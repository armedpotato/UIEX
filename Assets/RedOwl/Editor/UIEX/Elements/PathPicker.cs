﻿#pragma warning disable 0649 // UXMLReference variable declared but not assigned.
using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.UIElements;

namespace RedOwl.Editor
{
	public enum PathPickerTypes {
		FullPath = 0,
		ResourcePath,
		AssetsPath,
	}
	
	[UXML]
	public class PathPicker : RedOwlVisualElement
	{		
		public new class UxmlFactory : UxmlFactory<PathPicker> {}
				
		[UXMLReference("Path")]
		Label path;
		
		[UXMLReference("Button")]
		Button button;
		
		private PathPickerTypes PickerType;

		public string value {
			get {
				return path.text;
			}
		}
		public string fullPath { get; protected set; }
		
		public PathPicker() : base() {}
		
		public void Init(PathPickerTypes pickerType)
		{
			PickerType = pickerType;
		}
		
		[UICallback(true, 1)]
		private void InitUI()
		{
			button.clickable.clicked += OpenPicker;
		}
		
		private void OpenPicker()
		{
			string folder = fullPath = EditorUtility.SaveFolderPanel("Choose Folder", Application.dataPath, "");
			switch (PickerType)
			{
			case PathPickerTypes.ResourcePath:
				folder = folder.GetResourcesPath();
				if (string.IsNullOrEmpty(folder)) Debug.LogWarning($"The path choosen '{folder}' needs to be within a Resources folder!");
				break;
			case PathPickerTypes.AssetsPath:
				folder = folder.Replace(Application.dataPath, "Assets");
				break;
			case PathPickerTypes.FullPath:
			default:
				break;
			}
			path.text = folder;
		}
	}
}
