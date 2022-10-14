using BaseX;
using FrooxEngine;

namespace VRCFT.Neos
{
    public class MouthDevice : IInputDriver
    {
        private Mouth _mouth;
        public int UpdateOrder => 100;

        public MouthDevice()
        {
            Engine.Current.OnShutdown += Teardown;
        }

        private void Teardown()
        {

        }

        public void CollectDeviceInfos(DataTreeList list)
        {
            var mouthDataTreeDictionary = new DataTreeDictionary();
            mouthDataTreeDictionary.Add("Name", "VRCFT Face Tracking");
            mouthDataTreeDictionary.Add("Type", "Face Tracking");
            mouthDataTreeDictionary.Add("Model", "VRCFT Face Model");
            list.Add(mouthDataTreeDictionary);
        }

        public void RegisterInputs(InputInterface inputInterface)
        {
            _mouth = new Mouth(inputInterface, "VRCFT Mouth Tracking");
        }

        public void UpdateInputs(float deltaTime)
        {
            _mouth.IsTracking = true;

            _mouth.Jaw = new float3(
                OSCListener.VRCFTExpression["/JawRight"] - OSCListener.VRCFTExpression["/JawLeft"],
                OSCListener.VRCFTExpression["/JawOpen"],
                OSCListener.VRCFTExpression["/JawForward"]
            );
            _mouth.Tongue = new float3(
                OSCListener.VRCFTExpression["/TongueRight"] - OSCListener.VRCFTExpression["/TongueLeft"],
                OSCListener.VRCFTExpression["/TongueUp"] - OSCListener.VRCFTExpression["/TongueDown"],
                OSCListener.VRCFTExpression["/TongueLongStep1"] + OSCListener.VRCFTExpression["/TongueLongStep2"]);

            _mouth.JawOpen = 0f;
            _mouth.MouthPout = OSCListener.VRCFTExpression["/MouthPout"];
            _mouth.TongueRoll = OSCListener.VRCFTExpression["/TongueRoll"];

            _mouth.LipBottomOverUnder = OSCListener.VRCFTExpression["/MouthLowerInside"];
            _mouth.LipBottomOverturn = OSCListener.VRCFTExpression["/MouthLowerOverturn"];
            _mouth.LipTopOverUnder = OSCListener.VRCFTExpression["/MouthUpperInside"];
            _mouth.LipTopOverturn = OSCListener.VRCFTExpression["/MouthUpperOverturn"];

            _mouth.LipLowerHorizontal = 0f;
            _mouth.LipUpperHorizontal = 0f;

            _mouth.LipLowerLeftRaise = OSCListener.VRCFTExpression["/MouthLowerLeft"];
            _mouth.LipLowerRightRaise = OSCListener.VRCFTExpression["/MouthLowerRight"];
            _mouth.LipUpperRightRaise = OSCListener.VRCFTExpression["/MouthUpperRight"];
            _mouth.LipUpperLeftRaise = OSCListener.VRCFTExpression["/MouthUpperUpLeft"];

            _mouth.MouthRightSmileFrown = OSCListener.VRCFTExpression["/MouthSmileRight"] - OSCListener.VRCFTExpression["/MouthSadRight"];
            _mouth.MouthLeftSmileFrown = OSCListener.VRCFTExpression["/MouthSmileLeft"] - OSCListener.VRCFTExpression["/MouthSadLeft"];            _mouth.CheekLeftPuffSuck = 0f;
            _mouth.CheekRightPuffSuck = OSCListener.VRCFTExpression["/CheekPuffRight"] - OSCListener.VRCFTExpression["/CheekSuck"];
            _mouth.CheekLeftPuffSuck = OSCListener.VRCFTExpression["/CheekPuffLeft"] - OSCListener.VRCFTExpression["/CheekSuck"];
        }
    }
}
