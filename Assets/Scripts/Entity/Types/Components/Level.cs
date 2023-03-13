using UnityEngine;

[RequireComponent(typeof(Actor))]
public class Level : MonoBehaviour
{
    [SerializeField] private int currentLevel = 1, currentXp, xpToNextLevel, levelUpBase = 200, levelUpFactor = 150, xpGiven;

    public int CurrentLevel { get => currentLevel; }
    public int CurrentXp { get => currentXp; }
    public int XpToNextLevel { get => xpToNextLevel; }
    public int XpGiven { get => xpGiven; }

    public void OnValidate() => xpToNextLevel = ExperienceToNextLevel();

    private int ExperienceToNextLevel() => levelUpBase + currentLevel * levelUpFactor;
    private bool RequiresLevelUp() => currentXp >= xpToNextLevel;

    public void AddExperience(int xp)
    {
        if (xp == 0 || levelUpBase == 0) return;

        currentXp += xp;

        UIManager.instance.AddMessage($"You gain {xp} experience points.", "#FFFFFF");

        if (RequiresLevelUp())
        {
            UIManager.instance.ToggleLevelUpMenu(GetComponent<Actor>());
            UIManager.instance.AddMessage($"You grow stronger and reach the {currentLevel + 1} level!", "#00FF00");
        }
    }

    private void IncreaseLevel()
    {
        currentXp -= xpToNextLevel;
        currentLevel++;
        xpToNextLevel = ExperienceToNextLevel();
    }

    public void IncreaseMaxHp(int amount = 20)
    {
        GetComponent<Actor>().Fighter.MaxHp += amount;
        GetComponent<Actor>().Fighter.Hp += amount;

        UIManager.instance.AddMessage("Your vitals improves!", "#00FF00");
        IncreaseLevel();
    }

    public void IncreasePower(int amount = 1)
    {
        GetComponent<Actor>().Fighter.Power += amount;

        UIManager.instance.AddMessage("You feel stronger!", "#00FF00");
        IncreaseLevel();
    }

    public void IncreaseDefense(int amount = 1)
    {
        GetComponent<Actor>().Fighter.Defense += amount;

        UIManager.instance.AddMessage("Your defenses improves!", "#00FF00");
        IncreaseLevel();
    }

    public LevelState SaveState() => new(
        currentLevel: currentLevel,
        currentXp: currentXp,
        xpToNextLevel: xpToNextLevel);

    public void LoadState(LevelState state)
    {
        currentLevel = state.CurrentLevel;
        currentXp = state.CurrentXp;
        xpToNextLevel = state.XpToNextLevel;
    }
}

public class LevelState
{
    [SerializeField] private int currentLevel = 1, currentXp, xpToNextLevel;

    public int CurrentLevel { get => currentLevel; }
    public int CurrentXp { get => currentXp; }
    public int XpToNextLevel { get => xpToNextLevel; }

    public LevelState(int currentLevel, int currentXp, int xpToNextLevel)
    {
        this.currentLevel = currentLevel;
        this.currentXp = currentXp;
        this.xpToNextLevel = xpToNextLevel;
    }
}
