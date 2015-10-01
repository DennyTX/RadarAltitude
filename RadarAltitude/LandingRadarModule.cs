using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using LandingHeight;


namespace RadarAltitude
{
    public class LandingRadarModule : PartModule, IModuleInfo
    {

        [KSPField(guiActive = true, guiActiveEditor = true, guiName = "Landing radar", isPersistant = true)]
        [UI_Toggle(controlEnabled = true, enabledText = "On", disabledText = "Off", scene = UI_Scene.All)]
        public bool IsEnabled;

        [KSPField(guiActive = true, guiActiveEditor = true, guiUnits = "m", guiFormat = "##", isPersistant = true)]
        [UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, minValue = 0, maxValue = 8, stepIncrement = .5f)]
        public float Altitude;

        [KSPField(guiActive = true, guiActiveEditor = true, guiUnits = "m/s", guiFormat = "##.#", guiName = "Max speed", isPersistant = true)]
        [UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, minValue = 0, maxValue = 4, stepIncrement = .5f)]
        public float MaxSpeed;

        //[KSPField(guiActive = true, guiActiveEditor = true, guiName = "Presser", isPersistant = true)]
        //[UI_Toggle(controlEnabled = true, enabledText = "On", disabledText = "Off", scene = UI_Scene.All)]
        //public bool IsPresser;

        //[KSPField]
        //public bool ActivateOnLanding;

        private bool _isActive;
        private bool _isListen;

        //public override void OnAwake()
        public override void OnStart(StartState state)
        {
            if (!_isListen)
            {
                GameEvents.onVesselSituationChange.Add(SituationChanged);
                _isListen = true;
            }

        }

        public override void OnInactive()
        {
            base.OnInactive();
        }

        //public override void OnStart()
        //{
        //    GameEvents.onVesselSituationChange.Add(SituationChanged);
        //}

        //private void LoadConfig()
        //{
            //var gamePath = Assembly.GetEntryAssembly().FullName;
            //var gameDataPath = Path.Combine(gamePath, "Gamedata");

            //var listOfAllCFG = GetAllConfig(gameDataPath);
            //var listOfNeededCFG = new List<string>();
            //foreach (var current in listOfAllCFG)
            //{
            //    var currentConfig = ConfigNode.Load(current);
            //    var nodes = currentConfig.GetNodes("MODULE");
            //    var myNodes = nodes.Select(a => a.GetValue("name") == "LandingRadarModule");
            //    if (myNodes.Any())
            //    {
            //        listOfNeededCFG.Add(current);
            //    }
            //}
            //

            //foreach (var node in nodes)
            //{
            //    var item = new Item
            //    {
            //        ModuleName = node.GetValue("moduleName"),
            //        Biome = node.GetValue("biome") ?? string.Empty,
            //        Situation = node.GetValue("situation") ?? string.Empty,
            //        ExperimentTitle = node.GetValue("experimentTitle").ToLower()
            //    };
            //    _items.Add(item);
            //}
        //}

        //private List<string> GetAllConfig(string folderPath)
        //{
        //    List<string> results = new List<string>();
        //    foreach (var currentDirectory in new DirectoryInfo(folderPath).GetDirectories())
        //    {
        //        results.AddRange(GetAllConfig(currentDirectory.FullName));
        //    }
        //    results.AddRange(new DirectoryInfo(folderPath)
        //        .GetFiles("*.cfg")
        //        .Select(a=>a.FullName));
        //    return results;
        //}

            //public override void OnStart(StartState state)
        //{
        //    if (state != StartState.Flying) return;

        //    GameEvents.onVesselSituationChange.Add(SituationChanged);
        //}

        LHFlight lh = new LHFlight();
        
        //ModuleEngines workedEngine;

        public override void OnFixedUpdate()
        {
            _isActive = vessel.verticalSpeed + 0.01 < 0;

            if (!_isActive || !IsEnabled) return;
 
            if (lh.heightToLand() <= Altitude && Math.Abs(vessel.verticalSpeed) <= MaxSpeed)
            //if (vessel.heightFromTerrain <= Altitude && Math.Abs(vessel.verticalSpeed) <= MaxSpeed)
            {
                //shutdown engine
                //F(x)  (x) => y
                //we can use var instead List<ModuleEngines>
                List<ModuleEngines> engineModules = vessel.FindPartModulesImplementing<ModuleEngines>().Where(x => x.EngineIgnited).ToList();
                engineModules.ForEach(x => x.Shutdown());
                //if (engineModules.Any())
                //{
                //    workedEngine = engineModules.FirstOrDefault();
                //}
            }
        }

        private void SituationChanged(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> data)
        {
            //if (!IsPresser) return;
            if (data.host == vessel)
            {
                //if (workedEngine == null) return;
                if (data.from == Vessel.Situations.FLYING && data.to == Vessel.Situations.LANDED)
                {

                    //var landingEnginesModules = vessel.FindPartModulesImplementing<LandingRadarModule>().Where(a=> a.ActivateOnLanding);
                    //var AAAAA = Vessel.FindObjectsOfType<ModuleEngines>().Where(a => a.moduleName.Contains("ModuleEngines") && a.moduleName.Contains("LandingRadarModule"));
                    //List<ModuleEngines> _listOfPresserEngines = new List<ModuleEngines>(); 
                    foreach (var part in vessel.parts)
                    {
                        var _listOfPresserEngines = part.Modules.GetModules<PresserEngineMark>().Where(a => a.IsPresser).ToList(); //isPresser = bool

                        _listOfPresserEngines.ForEach(x => x.part.GetComponent<ModuleEngines>().Activate());

                        //var modules1 = part.Modules.GetModules<ModuleEngines>();
                        //var modules2 = part.Modules.GetModules<LandingRadarModule>();
                        //if (modules1.Count > 0 && modules2.Count > 0 && modules2[0].ActivateOnLanding == true)
                        //{
                        //    foreach (var module in modules1)
                        //    {
                        //        _listOfPresserEngines.Add(module);
                        //    }
                        //    //все условия выполнены
                        //    //_listOfPresserEngines.Add(modules1);
                        //}
                    }
                    //_listOfPresserEngines.ForEach(x => x.Activate());

                    //part.GetComponent<ModuleEngines>().Activate();

                    //if (IsPresser)
                    //{
                    //    current.vessel.FindPartModulesImplementing<ModuleEngines>().Activate();
                    //}
                    //workedEngine.transform.localPosition.z
                    //извлекаем модули прижимных движков и активируем их
                }
            }
        }

        public string GetModuleTitle()
        {
            return "Landing radar";
        }

        public Callback<Rect> GetDrawModulePanelCallback()
        {
            return null;
        }

        public string GetPrimaryField()
        {
            return "Landing radar for close distance";
        }

        public override string GetInfo()
        {
            return string.Format("<color=red>Altitude: </color>{0}<color=red> Speed: </color>{1}", Altitude, MaxSpeed);
            //return $"<color=red>Altitude:</color>{Altitude}<color=red>Speed:</color>{MaxSpeed}";
        }
    }
}
