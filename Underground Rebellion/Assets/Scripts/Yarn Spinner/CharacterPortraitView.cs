using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yarn.Unity
{
    public class CharacterPortraitView : DialogueViewBase
	{
		[Serializable]
		public class CharacterPortraitData
		{
			public string characterName;
			public Sprite displayPortrait;
		}

		[SerializeField] private List<CharacterPortraitData> portraitData;
		[SerializeField] private Image portraitImage;
		private Dictionary<string, Sprite> portraitDictionary = new();

		private static HorizontalLayoutGroup horizontalLayoutGroup;

		void Awake()
		{
			foreach(var portrait in portraitData)
			{
				portraitDictionary.Add(portrait.characterName, portrait.displayPortrait);
			}

			portraitData.Clear();

			horizontalLayoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
		}

		public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
		{
			var characterName = dialogueLine.CharacterName;

			var tempColor = portraitImage.color;

			if (string.IsNullOrEmpty(characterName) == false)
			{
				tempColor.a = 1;
				portraitImage.color = tempColor;
				portraitImage.sprite = portraitDictionary[characterName];
			}
			else
			{
				tempColor.a = 0;
				portraitImage.color = tempColor;
			}

			onDialogueLineFinished();
		}
		/// <summary>
		/// Changes the portrait side.
		/// </summary>
		[YarnCommand("change_portrait_side")]
		public static void ChangePortraitSide(string side)
		{
			side = side.ToLower();
			horizontalLayoutGroup.reverseArrangement = side.Equals("right") ? true : false;
		}
	}
}
