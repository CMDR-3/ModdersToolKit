using System;
using System.Text.RegularExpressions;
using MelonLoader;
using UnityEngine;

namespace ModdersToolKit
{
    public class Hook
    { 
        public string HookName { get; private set; }
        public string HookType { get; private set; }
        public string[] Arguments { get; private set; }
        public Action<string[]> Action { get; private set; }
        public Hook(string _name, string _type, Action<string[]> _action)
        {
            this.HookName = _name;
            this.HookType = _type;
            this.Action = _action;
        }
    }

    public class Hooks : MelonMod
    {
        public static GameObject GetChatTextObject()
        {
            GameObject systems = GameObject.Find("Systems");
            bool flag = systems != null;
            if (flag)
            {
                GameObject ui = systems.transform.Find("UI").gameObject;
                bool flag2 = ui != null;
                if (flag2)
                {
                    GameObject canvas = ui.transform.Find("Canvas").gameObject;
                    bool flag3 = canvas != null;
                    if (flag3)
                    {
                        GameObject ingameplayerhud = canvas.transform.Find("IngamePlayerHUD").gameObject;
                        bool flag4 = ingameplayerhud != null;
                        if (flag4)
                        {
                            GameObject bottomleftcorner = ingameplayerhud.transform.Find("BottomLeftCorner").gameObject;
                            bool flag5 = bottomleftcorner != null;
                            if (flag5)
                            {
                                return bottomleftcorner.transform.Find("ChatText").gameObject;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private static void AddHookToTable(Hook hook)
        {
            Array.Resize<Hook>(ref Hooks.hooks, Hooks.hooks.Length + 1);
            Hooks.hooks[Hooks.hooks.Length - 1] = hook;
        }

        public override void OnInitializeMelon()
        {
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
        }

        public static void FireHook(string HookFireOn, string[] args)
        {
            foreach (Hook hook in Hooks.hooks)
            {
                bool flag = hook.HookType == HookFireOn;
                if (flag)
                {
                    hook.Action(args);
                }
            }
        }

        public static MatchCollection getAllMessages(string currentText)
        {
            string pattern = "'.*'";
            return Regex.Matches(currentText, pattern);
        }

        public static MatchCollection getUsername(string currentText)
        {
            string pattern = ">.*\\b<";
            return Regex.Matches(currentText, pattern);
        }

        public static void AddHook(Action<string[]> HookFunction, string HookName, string FireOn)
        {
            Hook newHook = new Hook(HookName, FireOn, HookFunction);
            Hooks.AddHookToTable(newHook);
        }

        private static Hook[] hooks = new Hook[0];
    }
}
