using PluginSet.Core.Editor;
using UnityEngine;

namespace PluginSet.Taojin.Editor
{
    [BuildChannelsParams("Taojin", "Taojin OAID Settings")]
    public class BuildTaojinParams: ScriptableObject
    {
        [Tooltip("Enable Taojin OAID Plugin")]
        public bool Enable;
    }
}