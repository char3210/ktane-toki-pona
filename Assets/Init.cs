using Assets.Scripts.Rules;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Init : MonoBehaviour {

	public TextAsset default_tsv;

	static List<string[]> ParseCsv(string text)
	{
		var lines = new List<string[]>();
		var currentRow = new List<string>();
		var currentField = new StringBuilder();
		bool inQuotes = false;

		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];

			if (inQuotes)
			{
				if (c == '"')
				{
					// handle peek for escaped quotes ("")
					if (i + 1 < text.Length && text[i + 1] == '"')
					{
						currentField.Append('"');
						i++; // skip next quote
					}
					else
					{
						inQuotes = false; // closing quote
					}
				}
				else
				{
					currentField.Append(c);
				}
			}
			else
			{
				if (c == '"')
				{
					inQuotes = true; // opening quote
				}
				else if (c == ',')
				{
					currentRow.Add(currentField.ToString());
					currentField.Length = 0; // clear builder
				}
				else if (c == '\r')
				{
					// look ahead for \n
					if (i + 1 < text.Length && text[i + 1] == '\n') i++;

					currentRow.Add(currentField.ToString());
					currentField.Length = 0;
					lines.Add(currentRow.ToArray());
					currentRow.Clear();
				}
				else if (c == '\n')
				{
					currentRow.Add(currentField.ToString());
					currentField.Length = 0;
					lines.Add(currentRow.ToArray());
					currentRow.Clear();
				}
				else
				{
					currentField.Append(c);
				}
			}
		}

		// grab any leftover data at the end of the file
		if (currentField.Length > 0 || currentRow.Count > 0)
		{
			currentRow.Add(currentField.ToString());
			lines.Add(currentRow.ToArray());
		}

		return lines;
	}

	// Use this for initialization
	void Start()
	{
		List<string[]> content = ParseCsv(default_tsv.text);
		if (content[0][0] != "Keys" || content[0][1] != "toki pona [tok]")
		{
			Debug.LogWarning("[toki] invalid csv header (should be Keys and toki pona [tok])");
			return;
		}
		content.RemoveAt(0);

		Debug.Log("[toki] loaded " + content.Count + " rows");
		using (List<LanguageSourceData>.Enumerator enumerator = I2.Loc.LocalizationManager.Sources.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LanguageSourceData languageSourceData = enumerator.Current;
				foreach (string[] row in content)
				{
					if (row[1].Length == 0)
					{
						continue;
					}
					TermData termData = languageSourceData.GetTermData(row[0], false);
					if (termData != null)
					{
						termData.Languages[0] = row[1];
					}
					else
					{
						Debug.Log("[toki] term " + row[0] + " not found");
					}
				}
			}
		}
		Debug.Log("[toki] loaded translations");

		// set the password rule set cause localization changed
		PasswordRuleSet newPasswordRuleSet = new PasswordRuleSet(possibleTerms);
		// print alphabet
		List<char> alphabet = newPasswordRuleSet.alphabet;
		for (int i = 0; i < alphabet.Count; i++)
		{
			Debug.Log("[toki] alphabet[" + i + "] = " + alphabet[i]);
		}
		foreach (string term in newPasswordRuleSet.possibleTerms)
		{
			Debug.Log("[toki] possible term: " + term);
		}
		foreach (string word in newPasswordRuleSet.possibleWords)
		{
			Debug.Log("[toki] possible word: " + word);
		}
		//newPasswordRuleSet.TestRuleSet();
		RuleManager.instance.CurrentRules.PasswordRuleSet = newPasswordRuleSet;

	}


	private static List<string> possibleTerms = new List<string>
		{
			"PasswordComponent/rule_Option01",
			"PasswordComponent/rule_Option02",
			"PasswordComponent/rule_Option03",
			"PasswordComponent/rule_Option04",
			"PasswordComponent/rule_Option05",
			"PasswordComponent/rule_Option06",
			"PasswordComponent/rule_Option07",
			"PasswordComponent/rule_Option08",
			"PasswordComponent/rule_Option09",
			"PasswordComponent/rule_Option10",
			"PasswordComponent/rule_Option11",
			"PasswordComponent/rule_Option12",
			"PasswordComponent/rule_Option13",
			"PasswordComponent/rule_Option14",
			"PasswordComponent/rule_Option15",
			"PasswordComponent/rule_Option16",
			"PasswordComponent/rule_Option17",
			"PasswordComponent/rule_Option18",
			"PasswordComponent/rule_Option19",
			"PasswordComponent/rule_Option20",
			"PasswordComponent/rule_Option21",
			"PasswordComponent/rule_Option22",
			"PasswordComponent/rule_Option23",
			"PasswordComponent/rule_Option24",
			"PasswordComponent/rule_Option25",
			"PasswordComponent/rule_Option26",
			"PasswordComponent/rule_Option27",
			"PasswordComponent/rule_Option28",
			"PasswordComponent/rule_Option29",
			"PasswordComponent/rule_Option30",
			"PasswordComponent/rule_Option31",
			"PasswordComponent/rule_Option32",
			"PasswordComponent/rule_Option33",
			"PasswordComponent/rule_Option34",
			"PasswordComponent/rule_Option35"
		};

}
