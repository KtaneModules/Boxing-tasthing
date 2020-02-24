using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using rnd = UnityEngine.Random;
using System.Text.RegularExpressions;

public class boxing : MonoBehaviour
{
    public new KMAudio audio;
    public KMBombInfo bomb;
	public KMBombModule module;

    public KMSelectable boxingGlove;
	public KMSelectable hireButton;
	public KMSelectable abstainButton;
	public KMSelectable[] arrowButtons;
	public TextMesh[] screenTexts;
	public Color[] textColors;
	public Color solveColor;

	private int[] contestantStrengths = new int[5];
	private int[] contestantIndices = new int[5];
	private int[] lastNameIndices = new int[5];
	private int[] substituteIndices = new int[5];
	private int[] substituteLastNameIndices = new int[5];
	private int blueContestant;
	private int solution;
	private int chosenContestant;

	private bool[] animating = new bool[4];

	private static readonly float[] strengths = new float[5] { .5f, 1f, 2.5f, 5f, 8f };
	private static readonly Char[] alphabet = new Char[25] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y' };
	private static readonly string[] possibleNames = new string[25] { "Muhammed", "Mike", "Floyd", "Joe", "George", "Manny", "Sugar Ray", "Evander", "Oscar", "Roberto", "Jack", "Marvin", "Rocky", "Lennox", "Thomas", "Sonny", "Julio", "Roy", "Larry", "Archie", "Jake", "Bernard", "Gene", "Ken", "Wladimir" };
	private static readonly string[] possibleSubstituteNames = new string[] { "Liam", "Noah", "William", "James", "Oliver", "Benjamin", "Elijah", "Lucas", "Mason", "Logan", "Alexander", "Ethan", "Jacob", "Michael", "Daniel", "Harry", "Jackson", "Sebastian", "Aiden", "Matthew", "Samuel", "David", "Joseph", "Carter", "Owen", "Wyatt", "John", "Jack", "Luke", "Jayden", "Dylan", "Grayson", "Levi", "Isaac", "Gabriel", "Julian", "Mateo", "Anthony", "Jaxon", "Lincoln", "Joshua", "Christopher", "Andrew", "Theodore", "Caleb", "Ryan", "Asher", "Nathan", "Thomas", "Leo", "Isaiah", "Charles", "Josiah", "Hudson", "Christian", "Hunter", "Connor", "Eli", "Ezra", "Aaron", "Landon", "Adrian", "Jonathan", "Nolan", "Jeremiah", "Easton", "Elias", "Colton", "Cameron", "Carson", "Robert", "Angel", "Maverick", "Nicholas", "Dominic", "Jaxson", "Greyson", "Adam", "Ian", "Austin", "Santiago", "Jordan", "Cooper", "Brayden", "Roman", "Evan", "Ezekial", "Xavier", "Jose", "Jameson", "Leonardo", "Bryson", "Axel", "Everett", "Parker", "Kayden", "Miles", "Sawyer", "Jason", "Declan", "Weston", "Micah", "Ayden", "Wesley", "Luca", "Vincent", "Damien", "Zachary", "Silas", "Gavin", "Chase", "Kai", "Emmett", "Harrison", "Nathaniel", "Kingston", "Cole", "Tyler", "Bennett", "Bentley", "Ryker", "Tristan", "Brandon", "Kevin", "Luis", "George", "Ashton", "Rowan", "Braxton", "Ryder", "Gael", "Ivan", "Diego", "Maxwell", "Max", "Carlos", "Kaiden", "Juan", "Maddox", "Justin", "Waylon", "Calvin", "Giovanni", "Jonah", "Abel", "Jayce", "Jesus", "Amir", "King", "Beau", "Camden", "Alex", "Jasper", "Malachi", "Brody", "Jude", "Blake", "Emmanuel", "Eric", "Brooks", "Elliot", "Antonio", "Abraham", "Timothy", "Finn", "Rhett", "Kaito", "Xaq", "Kors", "Guennadi", "Elliott", "Edward", "August", "Xander", "Alan", "Dean", "Lorenzo", "Bryce", "Karter", "Victor", "Milo", "Miguel", "Hayden", "Graham", "Grant", "Zion", "Tucker", "Jesse", "Zayden", "Joel", "Richard", "Patrick", "Emiliano", "Avery", "Nicolas", "Brantley", "Dawson", "Myles", "Matteo", "River", "Steven", "Thiago", "Zane", "Bernie" };
	private static readonly string[] possibleLastNames = new string[] { "Hutchinson", "Boiko", "Mensa", "Jones", "Christ", "Smith", "Johnson", "Hall", "Stewart", "Price", "Allen", "Sanchez", "Bennett", "Shapiro", "Rodriguez", "Martinez", "Madhi", "Hussain", "Lee", "Lin", "Lao", "Savage", "Young", "Green", "Brown", "Morris", "Cook", "Taylor", "Walker", "Zimmerman", "King", "Davis", "Wright", "Henderson", "Miller", "Lopez", "Coleman", "Morgan", "Bell", "Moore", "Richardson", "Gonzalez", "Truck", "Hughes", "Patterson", "Jackson", "White", "Nelson", "Cox", "Dix", "Flores", "Howard", "Washington", "Zinn", "Torres", "Simmons", "Skinner", "Chalmers", "Martin", "Koppa", "Mogulescu", "Houshangi", "Tiveras", "Billy", "Mintzer", "Snyder", "Edwards", "Oh", "Kopek", "Bloom", "Hartstone", "Turner", "Tomas", "Cupaso", "Clark", "Sanders", "Lewis", "Phillips", "Gray", "Garcia", "Butler", "Ward", "Stein", "Joyner", "Crane", "Pickett", "Sears", "DeMayo", "Dunlap", "Universe", "McKay", "Ewing", "McCarthy", "Holder", "Stark", "Fulton", "Lynn", "Miranda", "Hooper", "Pollard", "Burch", "Mullen", "Duke", "O'Bryan", "Guy", "Britt", "Dillard", "Altson", "Jarvis", "Fitzpatrick", "Merrill", "Cote", "Raymond", "McGowan", "Craft", "Cleveland", "Clemons", "Wynn", "Nielsen", "Baird", "Stanton", "Snider", "Rosales", "Bright", "Witt", "Stuart", "Hays", "Holden", "Rutledge", "Kinney", "Clements", "Slater", "Hayder", "Pate", "Lancaster", "Burris", "Witcher", "McLovin", "Kidd", "Dale", "Hendrix", "Ramussen", "Sargeant", "Le", "Foreman", "Valencia", "Delacruz", "McMahon", "Vang", "Whitley", "Velazquez" };

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake()
    {
    	moduleId = moduleIdCounter++;
        boxingGlove.OnInteract += delegate () { PressBoxingGlove(); return false; };
		hireButton.OnInteract += delegate () { Hire(); return false; };
		abstainButton.OnInteract += delegate () { Abstain(); return false; };
		foreach (KMSelectable arrowButton in arrowButtons)
			arrowButton.OnInteract += delegate () { PressArrowButton(arrowButton); return false; };
        GetComponent<KMBombModule>().OnActivate += OnActivate;
    }


    void Start()
    {
		var dupliates = possibleSubstituteNames.Where(s => !possibleSubstituteNames.Distinct().Contains(s)).ToArray();
		foreach (string duplicate in dupliates)
			Debug.LogFormat("[Boxing #{0}] {1} is a duplicate.", moduleId, duplicate);
		contestantStrengths = Enumerable.Range(0,5).ToList().Shuffle().ToArray();
		contestantIndices = Enumerable.Range(0,25).ToList().Shuffle().Take(5).ToArray();
		blueContestant = rnd.Range(0,5);
		string[] ordinals = new string[5] { "1st", "2nd", "3rd", "4th", "5th" };
		int count = 0;
		List<Char> allCharacters = new List<Char>();
		var tempString = "";
		for (int i = 0; i < 5; i++)
		{
			lastNameIndices[i] = Array.IndexOf(possibleLastNames, possibleLastNames.PickRandom());
			substituteIndices[i] = Array.IndexOf(possibleSubstituteNames, possibleSubstituteNames.PickRandom());
			substituteLastNameIndices[i] = Array.IndexOf(possibleLastNames, possibleLastNames.PickRandom());
			tempString += possibleNames[contestantIndices[i]];
			tempString += possibleLastNames[lastNameIndices[i]];
			tempString += possibleSubstituteNames[substituteIndices[i]];
			tempString += possibleLastNames[substituteLastNameIndices[i]];
		}
		tempString = tempString.ToUpperInvariant();
		for (int i = 0; i < 5; i++)
		{
			var value1 = contestantStrengths[contestantIndices[i] / 5];
			var value2 = contestantStrengths[contestantIndices[i] % 5];
			var contestantChar = alphabet[5 * value1 + value2];
			allCharacters.Add(contestantChar);
			var addCount = tempString.Count(c => c == contestantChar);
			count += addCount;
			Debug.LogFormat("[Boxing #{0}] The {1} contestant is {2} {3}, with punch strength {4}, and his substitute is {5} {6}. His base 5 pair forms the letter {7}, so {8} is added to the total.", moduleId, ordinals[i], possibleNames[contestantIndices[i]], possibleLastNames[lastNameIndices[i]], contestantStrengths[i], possibleSubstituteNames[substituteIndices[i]], possibleLastNames[substituteLastNameIndices[i]], contestantChar, addCount);
		}
		count += Array.IndexOf(alphabet, allCharacters[blueContestant]);
		Debug.LogFormat("[Boxing #{0}] {1} has the blue name, so {2} is added.", moduleId, possibleNames[contestantIndices[blueContestant]], Array.IndexOf(alphabet, allCharacters[blueContestant]));
		int unmodifiedCount = count;
		count %= 6;
		Debug.LogFormat("[Boxing #{0}] The count is {1}, modulo 6 is {2}.", moduleId, unmodifiedCount, count % 6);
		if (count == 5)
		{
			solution = 10;
			Debug.LogFormat("[Boxing #{0}] Every contestant is on steroids. Abstain from participating.", moduleId);
		}
		else
		{
			if (count == 0)
				solution = Array.IndexOf(contestantStrengths, 4);
			else
			{
				int subtracted = 5 - count;
				solution = Array.IndexOf(contestantStrengths, subtracted);
			}
			Debug.LogFormat("[Boxing #{0}] The strongest contestant not on steroids is {1}.", moduleId, possibleNames[contestantIndices[solution]]);
		}
        screenTexts[0].text = "";
        screenTexts[1].text = "";
        screenTexts[2].text = "";
        screenTexts[3].text = "";
        /**screenTexts[0].text = possibleNames[contestantIndices[0]];
		screenTexts[1].text = possibleLastNames[lastNameIndices[0]];
		screenTexts[2].text = possibleSubstituteNames[substituteIndices[0]];
		screenTexts[3].text = possibleLastNames[substituteLastNameIndices[0]];
		foreach (TextMesh screenText in screenTexts)
			screenText.color = blueContestant == 0 ? textColors[1] : textColors[0];*/
    }

    void OnActivate()
    {
        string[] oldMessages = new string[4] { screenTexts[0].text, screenTexts[1].text, screenTexts[2].text, screenTexts[3].text };
        string[] newMessages = new string[4] { possibleNames[contestantIndices[chosenContestant]], possibleLastNames[lastNameIndices[chosenContestant]], possibleSubstituteNames[substituteIndices[chosenContestant]], possibleLastNames[substituteLastNameIndices[chosenContestant]] };
        for (int i = 0; i < 4; i++)
            StartCoroutine(CycleText(screenTexts[i], oldMessages[i], newMessages[i]));
    }

    void PressBoxingGlove()
    {
        if (moduleSolved || animating.Contains(true))
            return;
        boxingGlove.AddInteractionPunch(strengths[contestantStrengths[chosenContestant]]);
		audio.PlaySoundAtTransform("punch" + rnd.Range(0,6).ToString(), boxingGlove.transform);
    }

	void PressArrowButton(KMSelectable button)
	{
		audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, button.transform);
		button.AddInteractionPunch(.5f);
		if (moduleSolved || animating.Contains(true))
			return;
		int[] offsets = new int[2] { -1, 1 };
		int ix = Array.IndexOf(arrowButtons, button);
		if (!(chosenContestant == 0 && ix == 0) && !(chosenContestant == 4 && ix == 1))
		{
			chosenContestant += offsets[ix];
			string[] oldMessages = new string[4] { screenTexts[0].text, screenTexts[1].text, screenTexts[2].text, screenTexts[3].text };
			string[] newMessages = new string[4] { possibleNames[contestantIndices[chosenContestant]], possibleLastNames[lastNameIndices[chosenContestant]], possibleSubstituteNames[substituteIndices[chosenContestant]], possibleLastNames[substituteLastNameIndices[chosenContestant]] };
			for (int i = 0; i < 4; i++)
				StartCoroutine(CycleText(screenTexts[i], oldMessages[i], newMessages[i]));
		}
		else
			audio.PlaySoundAtTransform("error", button.transform);
	}

	void Hire()
	{
		audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, hireButton.transform);
		hireButton.AddInteractionPunch(.75f);
		if (moduleSolved || animating.Contains(true))
			return;
		if (chosenContestant == solution)
		{
			module.HandlePass();
			Debug.LogFormat("[Boxing #{0}] You submitted {1}. That is correct. Module solved!", moduleId, possibleNames[contestantIndices[chosenContestant]]);
			moduleSolved = true;
			string[] oldMessages = new string[4] { screenTexts[0].text, screenTexts[1].text, screenTexts[2].text, screenTexts[3].text };
			string[] newMessages = new string[4] { "Ready", "for", "the", "match!" };
			for (int i = 0; i < 4; i++)
				StartCoroutine(CycleText(screenTexts[i], oldMessages[i], newMessages[i]));
			audio.PlaySoundAtTransform("bell", hireButton.transform);
		}
		else
		{
			module.HandleStrike();
			Debug.LogFormat("[Boxing #{0}] You submitted {1}. That is incorrect. Strike!", moduleId, possibleNames[contestantIndices[chosenContestant]]);
			audio.PlaySoundAtTransform("strike", hireButton.transform);
		}
	}

	void Abstain()
	{
		audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, abstainButton.transform);
		abstainButton.AddInteractionPunch(.75f);
		if (moduleSolved || animating.Contains(true))
			return;
		if (solution == 10)
		{
			module.HandlePass();
			Debug.LogFormat("[Boxing #{0}] You abstained from participating. That is correct. Module solved!", moduleId);
			audio.PlaySoundAtTransform("abstain", abstainButton.transform);
			moduleSolved = true;
			string[] oldMessages = new string[4] { screenTexts[0].text, screenTexts[1].text, screenTexts[2].text, screenTexts[3].text };
			string[] newMessages = new string[4] { "No", "contest", "this", "time..." };
			for (int i = 0; i < 4; i++)
				StartCoroutine(CycleText(screenTexts[i], oldMessages[i], newMessages[i]));
		}
		else
		{
			module.HandleStrike();
			Debug.LogFormat("[Boxing #{0}] You abstained from participating. That is incorrect. Strike!", moduleId);
			audio.PlaySoundAtTransform("strike", abstainButton.transform);
		}
	}

	IEnumerator CycleText(TextMesh display, string oldMessage, string newMessage)
	{
		animating[Array.IndexOf(screenTexts, display)] = true;
		string currentMessage = oldMessage;
		int messageLength = currentMessage.Length;
		for (int i = 0; i < messageLength; i++)
		{
			currentMessage = currentMessage.Remove(currentMessage.Length - 1);
			display.text = currentMessage;
			audio.PlaySoundAtTransform("beep", display.transform);
			yield return new WaitForSeconds(.05f);
		}
		currentMessage = "";
		if (!moduleSolved)
			display.color = chosenContestant == blueContestant ? textColors[1] : textColors[0];
		else
			display.color = solveColor;
		for (int i = 0; i < newMessage.Length; i++)
		{
			currentMessage = currentMessage + newMessage[i];
			display.text = currentMessage;
			audio.PlaySoundAtTransform("beep", display.transform);
			yield return new WaitForSeconds(.05f);
		}
		animating[Array.IndexOf(screenTexts, display)] = false;
	}

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} punch [Use the power punch machine] | !{0} left/right [Presses the left or right arrow] | !{0} hire [Presses the hire button] | !{0} abstain [Presses the abstain button]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*punch\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            boxingGlove.OnInteract();
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*left\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            arrowButtons[0].OnInteract();
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*right\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            arrowButtons[1].OnInteract();
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*hire\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            hireButton.OnInteract();
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*abstain\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            abstainButton.OnInteract();
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        while (animating.Contains(true)) { yield return true; yield return new WaitForSeconds(0.1f); }
        if (solution == 10)
        {
            abstainButton.OnInteract();
        }
        else
        {
            if (chosenContestant < solution)
            {
                int times = solution - chosenContestant;
                for (int i = 0; i < times; i++)
                {
                    arrowButtons[1].OnInteract();
                    while (animating.Contains(true)) { yield return true; yield return new WaitForSeconds(0.1f); }
                }
            }
            else if (chosenContestant > solution)
            {
                int times = chosenContestant - solution;
                for (int i = 0; i < times; i++)
                {
                    arrowButtons[0].OnInteract();
                    while (animating.Contains(true)) { yield return true; yield return new WaitForSeconds(0.1f); }
                }
            }
            hireButton.OnInteract();
        }
    }
}
