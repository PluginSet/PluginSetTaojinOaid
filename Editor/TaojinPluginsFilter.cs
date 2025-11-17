using PluginSet.Core.Editor;
using UnityEditor;

namespace PluginSet.Taojin.Editor
{
    [InitializeOnLoad]
    public static class TaojinPluginsFilter
    {
        static TaojinPluginsFilter()
        {
            var filter = PluginFilter.IsBuildParamsEnable<BuildTaojinParams>();
            PluginFilter.RegisterFilter("com.pluginset.taojin/Plugins/Android", filter);
        }
    }
}