using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private bool isMenuOpen = false;
    [SerializeField] private TextMeshProUGUI dungeonFloorText;

    [Header("Health UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpSliderText;

    [Header("Message UI")]
    [SerializeField] private int sameMessageCount = 0; // read only
    [SerializeField] private string lastMessage;
    [SerializeField] private bool isMessageHistoryOpen = false; // read only
    [SerializeField] private GameObject messageHistory;
    [SerializeField] private GameObject messageHistoryContent;
    [SerializeField] private GameObject lastFiveMessagesContent;

    [Header("Inventory UI")]
    [SerializeField] private bool isInventoryOpen = false; // read only
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject inventoryContent;

    [Header("Drop Menu UI")]
    [SerializeField] private bool isDropMenuOpen = false; // read only
    [SerializeField] private GameObject dropMenu;
    [SerializeField] private GameObject dropMenuContent;

    [Header("Escape Menu UI")]
    [SerializeField] private bool isEscapeMenuOpen = false; // read only
    [SerializeField] private GameObject escapeMenu;

    [Header("Character Information Menu UI")]
    [SerializeField] private bool isCharInfoMenuOpen = false; // read only
    [SerializeField] private GameObject charInfoMenu;

    [Header("Level Up Menu UI")]
    [SerializeField] private bool isLevelUpMenuOpen = false; // read only
    [SerializeField] private GameObject levelUpMenu;
    [SerializeField] private GameObject levelUpMenuContent;

    public bool IsMenuOpen { get => isMenuOpen; }
    public bool IsMessageHistoryOpen { get => isMessageHistoryOpen; }
    public bool IsInventoryOpen { get => isInventoryOpen; }
    public bool IsDropMenuOpen { get => isDropMenuOpen; }
    public bool IsEscapeMenuOpen { get => isEscapeMenuOpen; }
    public bool IsCharInfoMenuOpen { get => isCharInfoMenuOpen; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // First message to the player
    private void Start()
    {
        SetDungeonFloorText(SaveManager.instance.CurrentFloor);

        if (SaveManager.instance.Save.SavedFloor is 0)
        {
            AddMessage("Turbo Drake (also known as the Player) rise once again to reclaim his powers!", "#f222ff");
            AddMessage("In the upper levels of the dungeon, a community of [C]ommoners thrives.", "#59CBE8");
            AddMessage("Resourceful and tenacious, they have carved out a living amongst the ruins and forgotten treasures.", "#59CBE8");

        }
    }


    // setting up the Player's max hp
    public void SetHealthMax(int maxHp)
    {
        hpSlider.maxValue = maxHp;
    }

    // setting up current HP and text to show on the HP bar
    public void SetHealth(int hp, int maxHp)
    {
        hpSlider.value = hp;
        hpSliderText.text = $"HP: {hp}/{maxHp}";
    }

    public void SetDungeonFloorText(int floor)
    {
        if (SaveManager.instance.Save.SavedFloor < 2)
        {
            dungeonFloorText.text = $"Floor: {floor} (Upper Dungeon)";
        }
        else if (SaveManager.instance.Save.SavedFloor < 6)
        {
            dungeonFloorText.text = $"Floor: {floor} (Templar Outpost)";
        }
        else if (SaveManager.instance.Save.SavedFloor < 8)
        {
            dungeonFloorText.text = $"Floor: {floor} (Neon Mines)";
        }
        else
        {
            dungeonFloorText.text = $"Floor: {floor} (Neon Sanctuary)";
        }
        
    }

    // toggling Menu
    public void ToggleMenu()
    {
        if (isMenuOpen)
        {
            isMenuOpen = !isMenuOpen;

            switch (true)
            {
                case bool _ when isMessageHistoryOpen:
                    ToggleMessageHistory();
                    break;
                case bool _ when isInventoryOpen:
                    ToggleInventory();
                    break;
                case bool _ when isDropMenuOpen:
                    ToggleDropMenu();
                    break;
                case bool _ when isEscapeMenuOpen:
                    ToggleEscapeMenu();
                    break;
                case bool _ when isCharInfoMenuOpen:
                    ToggleCharacterInformationMenu();
                    break;
                default:
                    break;
            }
        }
    }

    // message history menu
    public void ToggleMessageHistory()
    {
        isMessageHistoryOpen = !isMessageHistoryOpen;
        SetBooleans(messageHistory, isMessageHistoryOpen);
    }

    // inventory
    public void ToggleInventory(Actor actor = null)
    {
        isInventoryOpen = !isInventoryOpen;
        SetBooleans(inventory, isInventoryOpen);

        // if menu is open - update the content - same for drop menu
        if (isMenuOpen)
        {
            UpdateMenu(actor, inventoryContent);
        }
    }

    public void ToggleDropMenu(Actor actor = null)
    {
        isDropMenuOpen = !isDropMenuOpen;
        SetBooleans(dropMenu, isDropMenuOpen);

        if (isMenuOpen)
        {
            UpdateMenu(actor, dropMenuContent);
        }
    }

    public void ToggleEscapeMenu()
    {
        isEscapeMenuOpen = !isEscapeMenuOpen;
        SetBooleans(escapeMenu, isEscapeMenuOpen);

        eventSystem.SetSelectedGameObject(escapeMenu.transform.GetChild(0).gameObject);

    }

    public void ToggleLevelUpMenu(Actor actor)
    {
        isLevelUpMenuOpen = !isLevelUpMenuOpen;
        SetBooleans(levelUpMenu, isLevelUpMenuOpen);

        GameObject healthButton = levelUpMenuContent.transform.GetChild(0).gameObject;
        GameObject powerButton = levelUpMenuContent.transform.GetChild(1).gameObject;
        GameObject defenseButton = levelUpMenuContent.transform.GetChild(2).gameObject;

        healthButton.GetComponent<TextMeshProUGUI>().text = $"a) Vitality (+20 HP, from {actor.GetComponent<Fighter>().MaxHp})";
        powerButton.GetComponent<TextMeshProUGUI>().text = $"b) Power (+1 attack, from {actor.GetComponent<Fighter>().Power()})";
        defenseButton.GetComponent<TextMeshProUGUI>().text = $"c) Toughness (+1 defense, from {actor.GetComponent<Fighter>().Defense()})";

        foreach (Transform child in levelUpMenuContent.transform)
        {
            child.GetComponent<Button>().onClick.RemoveAllListeners();

            child.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (healthButton == child.gameObject)
                {
                    actor.GetComponent<Level>().IncreaseMaxHp();
                }
                else if (powerButton == child.gameObject)
                {
                    actor.GetComponent<Level>().IncreasePower();
                }
                else if (defenseButton == child.gameObject)
                {
                    actor.GetComponent<Level>().IncreaseDefense();
                }
                else
                {
                    Debug.LogError("No button found");
                }
                ToggleLevelUpMenu(actor);
            });
        }

        eventSystem.SetSelectedGameObject(levelUpMenuContent.transform.GetChild(0).gameObject);
    }

    public void ToggleCharacterInformationMenu(Actor actor = null)
    {
        isCharInfoMenuOpen = !isCharInfoMenuOpen;
        SetBooleans(charInfoMenu, isCharInfoMenuOpen);

        if (actor is not null)
        {
            charInfoMenu.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Level: {actor.GetComponent<Level>().CurrentLevel}";
            charInfoMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"XP: {actor.GetComponent<Level>().CurrentXp}";
            charInfoMenu.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"XP to next level: {actor.GetComponent<Level>().XpToNextLevel}";
            charInfoMenu.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"Attack: {actor.GetComponent<Fighter>().Power()}";
            charInfoMenu.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"Defense: {actor.GetComponent<Fighter>().Defense()}";
        }
    }

    private void SetBooleans(GameObject menu, bool menuBool)
    {
        isMenuOpen = menuBool;
        menu.SetActive(menuBool);
    }

    public void Save()
    {
        SaveManager.instance.SaveGame(false);
        AddMessage("You have saved your progress... delaying the inevitable.", "#0DA2FF");
    }

    public void Load()
    {
        SaveManager.instance.LoadGame();
        AddMessage("Looks like time travel is possible... but will that be enough?", "#0DA2FF");
        ToggleMenu();
    }

    public void Quit()
    {
        Application.Quit();
    }

    // adding a messages to player
    public void AddMessage(string newMessage, string colorHex)
    {
        // checking if last message is the same as new message
        if (lastMessage == newMessage)
        {
            // getting the last message from the history and from last five messages
            TextMeshProUGUI messageHistoryLastChild = messageHistoryContent.transform.GetChild(messageHistoryContent.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI lastFiveHistoryLastChild = lastFiveMessagesContent.transform.GetChild(lastFiveMessagesContent.transform.childCount - 1).GetComponent<TextMeshProUGUI>();

            // setting the message by adding the counter if the messages are the same
            messageHistoryLastChild.text = $"{newMessage} (x{++sameMessageCount})";
            lastFiveHistoryLastChild.text = $"{newMessage} (x{sameMessageCount})";

            return;
        }
        // if same message count is more than 0 - set as 0
        else if (sameMessageCount > 0)
        {
            sameMessageCount = 0;
        }

        lastMessage = newMessage;

        // loading the message resource and setting up text color and position based on message history
        TextMeshProUGUI messagePrefab = Instantiate(Resources.Load<TextMeshProUGUI>("Message")) as TextMeshProUGUI;
        messagePrefab.text = newMessage;
        messagePrefab.color = GetColorFromHex(colorHex);
        messagePrefab.transform.SetParent(messageHistoryContent.transform, false);

        for (int i = 0; i < lastFiveMessagesContent.transform.childCount; i++)
        {
            if (messageHistoryContent.transform.childCount - 1 < i)
            {
                return;
            }

            TextMeshProUGUI lastFiveHistoryChild = lastFiveMessagesContent.transform.GetChild(lastFiveMessagesContent.transform.childCount - 1 - i).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI messageHistoryChild = messageHistoryContent.transform.GetChild(messageHistoryContent.transform.childCount - 1 - i).GetComponent<TextMeshProUGUI>();
            lastFiveHistoryChild.text = messageHistoryChild.text;
            lastFiveHistoryChild.color = messageHistoryChild.color;
        }
    }

    private Color GetColorFromHex(string v)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(v, out color))
        {
            return color;
        }
        else
        {
            Debug.Log("GetColorFromHex : Could not parse color from string");
            return Color.white;
        }
    }

    // updating the inventory and drop menus
    private void UpdateMenu(Actor actor, GameObject menuContent)
    {
        for (int resetNum = 0; resetNum < menuContent.transform.childCount; resetNum++)
        {
            GameObject menuContentChild = menuContent.transform.GetChild(resetNum).gameObject;
            menuContentChild.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            menuContentChild.GetComponent<Button>().onClick.RemoveAllListeners();
            menuContentChild.SetActive(false);
        }

        char c = 'a';

        for (int itemNum = 0; itemNum < actor.Inventory.Items.Count; itemNum++)
        {
            GameObject menuContentChild = menuContent.transform.GetChild(itemNum).gameObject;
            Item item = actor.Inventory.Items[itemNum];
            menuContentChild.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"({c++}) {item.name}";
            menuContentChild.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (menuContent == inventoryContent)
                {
                    if (item.Consumable is not null)
                    {
                        Action.UseAction(actor, item);
                    }
                    else if (item.Equippable is not null)
                    {
                        Action.EquipAction(actor, item);
                    }
                }
                else if (menuContent == dropMenuContent)
                {
                    Action.DropAction(actor, item);
                }

                UpdateMenu(actor, menuContent);
            });
            menuContentChild.SetActive(true);
        }

        eventSystem.SetSelectedGameObject(menuContent.transform.GetChild(0).gameObject);
    }
}
