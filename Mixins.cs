using HarmonyLib;

namespace ModdersToolKit
{
    public class Mixins
    {
        public enum MixinType
        {
            Inject,
            Overwrite
        }

        public enum MixinInjectionPosition
        {
            Head,
            Tail
        }

        private static HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("com.zetalasis.lethal_company_mtk_patches");

        /**<summary>Stolen from Java, Mixins allow you to "mix into" the games code, allowing you to add custom functions.
         <para>Be REALLY careful with this function - you WILL break mods if you aren't. (or even break the game)</para>
        <para>More documentation available on the GitHub wiki.</para></summary>
         */
        public static void CreateMixin(MixinType type, MixinInjectionPosition position, object mixInto, object mixFrom)
        {
            var mOriginal = AccessTools.Method(mixInto.GetType(), nameof(mixInto));

            if (type == MixinType.Inject)
            {
                if (position == MixinInjectionPosition.Head)
                {
                    var mPrefix = SymbolExtensions.GetMethodInfo(() => mixFrom.GetType());
                    harmony.Patch(mOriginal, new HarmonyMethod(mPrefix), null);
                }
                if (position == MixinInjectionPosition.Head)
                {
                    var mPostFix = SymbolExtensions.GetMethodInfo(() => mixFrom.GetType());
                    harmony.Patch(mOriginal, null, new HarmonyMethod(mPostFix));
                }
            }
        }
    }
}
