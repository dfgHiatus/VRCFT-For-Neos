using HarmonyLib;
using NeosModLoader;
using FrooxEngine;

namespace VRCFT.Neos
{
    public class VRCFTNeos : NeosMod
	{
		public override string Name => "VRCFTNeos";
		public override string Author => "dfgHiatus";
		public override string Version => "1.0.0";
		public override string Link => "https://github.com/dfgHiatus/VRCFT-For-Neos";

		private static OSCListener osc;
        
		public static ModConfiguration Config;
        
		[AutoRegisterConfigKey]
		public static ModConfigurationKey<int?> OptionalPort =
			new ModConfigurationKey<int?>("optionalPort", "Optional port number. Leave null to use the default port (9000), changing requires a restart.", () => null);
        
		[AutoRegisterConfigKey]
		public static ModConfigurationKey<float> Alpha = new ModConfigurationKey<float>("alpha", "Eye Swing Multiplier X", () => 1.0f);

		[AutoRegisterConfigKey]
		public static ModConfigurationKey<float> Beta = new ModConfigurationKey<float>("beta", "Eye Swing Multiplier Y", () => 1.0f);


		public override void OnEngineInit()
		{
			new Harmony("net.dfgHiatus.VRCFTNeos").PatchAll();
			Config = GetConfiguration();
            osc = new OSCListener(Config.GetValue(OptionalPort));

            if (OSCListener.ExpressionsWithAddress["/EyeTrackingActive"] != 0)
                Engine.Current.InputInterface.RegisterInputDriver(new EyeDevice());

            if (OSCListener.ExpressionsWithAddress["/MouthTrackingActive"] != 0)
				Engine.Current.OnReady += () => Engine.Current.InputInterface.RegisterInputDriver(new MouthDevice());
            
			Engine.Current.OnShutdown += () => osc.Teardown();

		}
	}
}
