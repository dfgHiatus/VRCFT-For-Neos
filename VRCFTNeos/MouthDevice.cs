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
                OSCListener.ExpressionsWithAddress["/JawRight"] - OSCListener.ExpressionsWithAddress["/JawLeft"],
                OSCListener.ExpressionsWithAddress["/JawOpen"],
                OSCListener.ExpressionsWithAddress["/JawForward"]
            );
            _mouth.Tongue = new float3(
                OSCListener.ExpressionsWithAddress["/TongueRight"] - OSCListener.ExpressionsWithAddress["/TongueLeft"],
                OSCListener.ExpressionsWithAddress["/TongueUp"] - OSCListener.ExpressionsWithAddress["/TongueDown"],
                OSCListener.ExpressionsWithAddress["/TongueLongStep1"] + OSCListener.ExpressionsWithAddress["/TongueLongStep2"]);

            _mouth.JawOpen = 0f;
            _mouth.MouthPout = OSCListener.ExpressionsWithAddress["/MouthPout"];
            _mouth.TongueRoll = OSCListener.ExpressionsWithAddress["/TongueRoll"];

            _mouth.LipBottomOverUnder = OSCListener.ExpressionsWithAddress["/MouthLowerInside"];
            _mouth.LipBottomOverturn = OSCListener.ExpressionsWithAddress["/MouthLowerOverturn"];
            _mouth.LipTopOverUnder = OSCListener.ExpressionsWithAddress["/MouthUpperInside"];
            _mouth.LipTopOverturn = OSCListener.ExpressionsWithAddress["/MouthUpperOverturn"];

            _mouth.LipLowerHorizontal = 0f;
            _mouth.LipUpperHorizontal = 0f;

            _mouth.LipLowerLeftRaise = OSCListener.ExpressionsWithAddress["/MouthLowerLeft"];
            _mouth.LipLowerRightRaise = OSCListener.ExpressionsWithAddress["/MouthLowerRight"];
            _mouth.LipUpperRightRaise = OSCListener.ExpressionsWithAddress["/MouthUpperRight"];
            _mouth.LipUpperLeftRaise = OSCListener.ExpressionsWithAddress["/MouthUpperUpLeft"];

            _mouth.MouthRightSmileFrown = OSCListener.ExpressionsWithAddress["/MouthSmileRight"] - OSCListener.ExpressionsWithAddress["/MouthSadRight"];
            _mouth.MouthLeftSmileFrown = OSCListener.ExpressionsWithAddress["/MouthSmileLeft"] - OSCListener.ExpressionsWithAddress["/MouthSadLeft"];            _mouth.CheekLeftPuffSuck = 0f;
            _mouth.CheekRightPuffSuck = OSCListener.ExpressionsWithAddress["/CheekPuffRight"] - OSCListener.ExpressionsWithAddress["/CheekSuck"];
            _mouth.CheekLeftPuffSuck = OSCListener.ExpressionsWithAddress["/CheekPuffLeft"] - OSCListener.ExpressionsWithAddress["/CheekSuck"];
        }
    }
}
