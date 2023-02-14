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

    public bool IsMenuOpen { get => isMenuOpen; }
    public bool IsMessageHistoryOpen { get => isMessageHistoryOpen; }
    public bool IsInventoryOpen { get => isInventoryOpen; }
    public bool IsDropMenuOpen { get => isDropMenuOpen; }

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
    private void Start() => AddMessage("Turbo Drake rise once again to reclaim his powers!", "#f222ff");


    // setting up the Player's max hp
    public void SetHealthMax(int maxHp)
    {
        hpSlider.maxValue = maxHp;
    }

    // setting up current HP and text to show on the HP bar
    public void SetHealth(int hp, int maxHP)
    {
        hpSlider.value = hp;
        hpSliderText.text = $"HP: {hp}/{maxHP}";
    }

    // toggling Menu
    public void ToggleMenu()
    {
        if (isMenuOpen)
        {
            isMenuOpen = !isMenuOpen;

            if (isMessageHistoryOpen)
            {
                ToggleMessageHistory();
            }

            if (isInventoryOpen)
            {
                ToggleInventory();
            }

            if (isDropMenuOpen)
            {
                ToggleDropMenu();
            }

            return;
        }
    }

    // message history menu
    public void ToggleMessageHistory()
    {
        messageHistory.SetActive(!messageHistory.activeSelf);
        isMessageHistoryOpen = messageHistory.activeSelf;
    }

    // inventory
    public void ToggleInventory(Actor actor = null)
    {
        inventory.SetActive(!inventory.activeSelf);
        isMenuOpen = inventory.activeSelf;
        isInventoryOpen = inventory.activeSelf;

        // if menu is open - update the content - same for drop menu
        if (isMenuOpen)
        {
            UpdateMenu(actor, inventoryContent);
        }
    }

    public void ToggleDropMenu(Actor actor = null)
    {
        dropMenu.SetActive(!dropMenu.activeSelf);
        isMenuOpen = dropMenu.activeSelf;
        isDropMenuOpen = dropMenu.activeSelf;

        if (isMenuOpen)
        {
            UpdateMenu(actor, dropMenuContent);
        }
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
            lastFiveHistoryLastChild.text = $"{newMessage} (x{sameMessageCount}";

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
        for (int i = 0; i < menuContent.transform.childCount; i++)
        {
            GameObject menuContentChild = menuContent.transform.GetChild(i).gameObject;
            menuContentChild.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            menuContentChild.GetComponent<Button>().onClick.RemoveAllListeners();
            menuContentChild.SetActive(false);
        }

        char c = 'a';

        for (int i = 0; i < actor.Inventory.Items.Count; i++)
        {
            GameObject menuContentChild = menuContent.transform.GetChild(i).gameObject;
            menuContentChild.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"({c++}) {actor.Inventory.Items[i].name}";
            menuContentChild.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (menuContent == inventoryContent)
                {
                    Action.UseAction(actor, i - 1);
                }
                else if (menuContent == dropMenuContent)
                {
                    Action.DropAction(actor, actor.Inventory.Items[i - 1]);
                }

                UpdateMenu(actor, menuContent);
            });
            menuContentChild.SetActive(true);
        }

        eventSystem.SetSelectedGameObject(menuContent.transform.GetChild(0).gameObject);
    }
}
