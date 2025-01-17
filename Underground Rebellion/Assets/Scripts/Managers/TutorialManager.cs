using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI tutorialText;
    private static int nParries;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UpdateParryProgress(int parryTimes)
    {
        nParries = parryTimes;
        if (parryTimes < 2)
        {
            tutorialText.text = string.Format("Press 'J' to parry the enemy at the right time, to lower their energy\n\nProgress: {0} / 2 Parries", nParries);
        }else if (parryTimes == 2)
        {
            tutorialText.text = string.Format("Press 'J' to parry the enemy at the right time, to lower their energy\n\nProgress: {0} / 2 Parries", nParries);
            ShowAttackBox();
        }
        
    }
    
    private void ShowAttackBox()
    {
        tutorialText.text = string.Format("Press 'K' to attack the enemy");
    }

    private void ShowMapTutorial()
    {
        tutorialText.text = string.Format("Press 'M' to show the map\nYour map is your guide—let the adventure begin!");
    }
}
