using PluginSet.Core.Editor;
using UnityEditor;

namespace PluginSet.Taojin.Editor
{
    [BuildTools]
    public static class BuildTaojinTools
    {
        [OnSyncEditorSetting]
        public static void OnSyncEditorSetting(BuildProcessorContext context)
        {
            if (!context.BuildTarget.Equals(BuildTarget.Android))
                return;

            var buildParams = context.BuildChannels.Get<BuildTaojinParams>();
            if (!buildParams.Enable)
                return;

            context.Symbols.Add("TOAJIN_OAID_ENABLE");
            context.AddLinkAssembly("PluginSet.Taojin");
        }
    }
}