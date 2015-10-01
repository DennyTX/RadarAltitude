using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP.IO;
using UnityEngine;

namespace RadarAltitude
{
    public class  PresserEngineMark : PartModule
    {
        [KSPField(guiActive = true, guiActiveEditor = true, guiName = "Presser", isPersistant = true)]
        [UI_Toggle(controlEnabled = true, enabledText = "On", disabledText = "Off", scene = UI_Scene.All)]
        public bool IsPresser;

        public override void OnSave(ConfigNode node)
        {
            var config = PluginConfiguration.CreateForType<PresserEngineMark>();

            config.SetValue("isPresser", IsPresser);

            config.save();   
            //base.OnSave(node);
        }

        public override void OnLoad(ConfigNode node)
        {
            //base.OnLoad(node);
            var config = PluginConfiguration.CreateForType<PresserEngineMark>();

            config.load();

            IsPresser = config.GetValue<bool>("isPresser");
        }

    }

}
