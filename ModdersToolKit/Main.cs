using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using HarmonyLib;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModdersToolKit
{
    public class Main : MelonMod
    {
        public static void SetModdedMusic(AudioClip[] setTo)
        {
            foreach (AudioClip clip in setTo)
            {
                bool flag = !Main.moddedMusic.Contains(clip);
                if (flag)
                {
                    int index = Main.moddedMusic.Length;
                    bool flag2 = index < 0;
                    if (flag2)
                    {
                        index = 0;
                    }
                    Array.Resize<AudioClip>(ref Main.moddedMusic, index + 1);
                    Main.moddedMusic[index] = clip;
                }
            }
        }
        public class AudioClipData
        {
            public AudioClip Clip { get; private set; }

            public AudioClip[] ClipArray { get; private set; }

            public AudioClipData(AudioClip singleClip, AudioClip[] clipArray)
            {
                this.Clip = singleClip;
                this.ClipArray = clipArray;
            }
        }

        public override void OnInitializeMelon()
        {

        }

        public override void OnUpdate()
        {
            this.chatObject = Hooks.GetChatTextObject();
            bool flag = this.chatObject != null;
            if (flag)
            {
                this.currentText = this.chatObject.GetComponent<TextMeshProUGUI>().text;
                MatchCollection collection = Hooks.getAllMessages(this.currentText);
                MatchCollection username_collection = Hooks.getUsername(this.currentText);
                bool flag2 = collection.Count > 0;
                if (flag2)
                {
                    int index = collection.Count - 1;
                    bool flag3 = index < 0;
                    if (flag3)
                    {
                        index = 0;
                    }
                    string username = username_collection[index].ToString();
                    username = username.Remove(0, 1);
                    username = username.Remove(username.Length - 1);
                    string chatMessage = collection[index].ToString();
                    chatMessage = chatMessage.Remove(0, 1);
                    chatMessage = chatMessage.Remove(chatMessage.Length - 1);
                    bool flag4 = chatMessage != this.oldText;
                    if (flag4)
                    {
                        string[] args = new string[]
                        {
                            username,
                            chatMessage
                        };
                        Hooks.FireHook("ChatMessageSend", args);
                    }
                    this.oldText = chatMessage;
                }
            }
            Main.PatchBoomboxes(Main.moddedMusic);
        }

        public static bool IsIngame()
        {
            GameObject env = GameObject.Find("Environment");
            if (env != null) {
                return true;
            }
            return false;
        }

        public override void OnSceneWasLoaded(int buildIndex, string _sceneName)
        {
            sceneName = _sceneName;
            bool flag = Main.IsIngame();
            if (flag)
            {
                Main.RegisterItemInTerminal(Main.CreateItem("Test Item", 1, new Mesh(), Main.moddedMusic[0], 124, 1));
            }
        }

        /**<summary>Gets the Main Menu's MenuManager component. Will return null if cant find/not in menu.</summary>*/
        public static MenuManager GetMenuManager()
        {
            GameObject menu = GameObject.Find("Canvas");
            if (menu != null)
            {
                GameObject menuManager = GameObject.Find("MenuManager");
                if (menuManager != null)
                {
                    return menuManager.GetComponent<MenuManager>();
                }
            }

            return null;
        }

        public static Item CreateItem(string name, int batteryUsage, Mesh itemMesh, AudioClip grabSFX, int itemID, int price)
        {
            Item newItem = new Item();
            newItem.name = name;
            newItem.batteryUsage = (float)batteryUsage;
            newItem.itemName = name;
            Mesh[] meshArray = new Mesh[]
            {
                itemMesh
            };
            newItem.meshVariants = meshArray;
            newItem.grabSFX = grabSFX;
            newItem.itemId = itemID;
            newItem.creditsWorth = price;
            return newItem;
        }

        public static void RegisterItemInTerminal(Item itemToRegister)
        {
            GameObject terminalScript = GameObject.Find("Environment").transform.Find("HangarShip").Find("Terminal").Find("TerminalTrigger").Find("TerminalScript").gameObject;
            Terminal script = terminalScript.GetComponent<Terminal>();
            int buyableItemIndex = script.buyableItemsList.Length;
            int salesPercentageIndex = script.itemSalesPercentages.Length;
            int keywordIndex = script.terminalNodes.allKeywords.Length;
            bool flag = buyableItemIndex < 0;
            if (flag)
            {
                buyableItemIndex = 0;
            }
            bool flag2 = salesPercentageIndex < 0;
            if (flag2)
            {
                salesPercentageIndex = 0;
            }
            bool flag3 = keywordIndex < 0;
            if (flag3)
            {
                keywordIndex = 0;
            }
            Array.Resize<Item>(ref script.buyableItemsList, buyableItemIndex + 1);
            Array.Resize<int>(ref script.itemSalesPercentages, salesPercentageIndex + 1);
            Array.Resize<TerminalKeyword>(ref script.terminalNodes.allKeywords, keywordIndex + 1);

            script.buyableItemsList[buyableItemIndex] = itemToRegister;
            script.itemSalesPercentages[salesPercentageIndex] = 100;

            TerminalKeyword keyword = new TerminalKeyword();
            keyword.word = itemToRegister.itemName.ToLower();

            script.terminalNodes.allKeywords[keywordIndex] = keyword;
            MelonLogger.Msg("Registered Item " + itemToRegister.itemName + " to terminal.");
        }

        public static void PatchBoomboxes(AudioClip[] moddedMusic)
        {
            BoomboxItem[] boomboxes = UnityEngine.Object.FindObjectsOfType<BoomboxItem>();
            foreach (BoomboxItem boombox in boomboxes)
            {
                foreach (AudioClip soundToPatch in moddedMusic)
                {
                    bool flag = !boombox.musicAudios.Contains(soundToPatch);
                    if (flag)
                    {
                        try
                        {
                            int index = boombox.musicAudios.Length;
                            Array.Resize<AudioClip>(ref boombox.musicAudios, boombox.musicAudios.Length + 1);
                            boombox.musicAudios[index - 1] = soundToPatch;
                            MelonLogger.Msg(soundToPatch.name);
                        }
                        catch (Exception e)
                        {
                            string str = "Error: ";
                            Exception ex = e;
                            MelonLogger.Error(str + ((ex != null) ? ex.ToString() : null));
                        }
                    }
                }
            }
        }

        public static AudioClip[] moddedMusic = new AudioClip[0];
        private GameObject chatObject = null;
        private string currentText = "";
        private string oldText = "";
        public string sceneName = "";
    }
}
