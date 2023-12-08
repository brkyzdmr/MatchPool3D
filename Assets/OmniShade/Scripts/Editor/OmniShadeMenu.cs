//------------------------------------
//             OmniShade
//     Copyright© 2023 OmniShade     
//------------------------------------

using UnityEditor;

/**
 * This class creates a menu in Edit->OmniShade to switch between normal and fallback versions.
 **/
public static class OmniShadeMenu {   
    const string MENU_NORMAL = "Tools/" + OmniShade.NAME + "/Switch To Normal";
    const string MENU_FALLBACK = "Tools/" + OmniShade.NAME + "/Switch To Fallback";

    [MenuItem(MENU_NORMAL, true)]
    static bool SwitchNormalValidate() {
        OmniShadeMenu.CheckSelected();
        return true;
    }

    [MenuItem(MENU_FALLBACK, true)]
    static bool SwitchFallbackValidate() {
        OmniShadeMenu.CheckSelected();
        return true;
    }

    static void CheckSelected() {
        bool isFallback = OmniShade.IsFallbackShader();
        Menu.SetChecked(MENU_NORMAL, !isFallback);
        Menu.SetChecked(MENU_FALLBACK, isFallback);
    }

    [MenuItem(MENU_NORMAL)]
    static void SwitchNormal() {
        OmniShade.SetNormalShader();
    }

    [MenuItem(MENU_FALLBACK)]
    static void SwitchFallback() {
        OmniShade.SetFallbackShader();
    }
}
