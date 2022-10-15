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

        // No dictionary entries in this method should be null
        public void UpdateInputs(float deltaTime)
        {
            _mouth.IsTracking = true;

            _mouth.Jaw = new float3(
                OSCListener.VRCFTExpression.GetByKey1("JawRight") - OSCListener.VRCFTExpression.GetByKey1("JawLeft"),
                OSCListener.VRCFTExpression.GetByKey1("JawOpen"),
                OSCListener.VRCFTExpression.GetByKey1("JawForward")
            );
            _mouth.Tongue = new float3(
                OSCListener.VRCFTExpression.GetByKey1("TongueRight") - OSCListener.VRCFTExpression.GetByKey1("TongueLeft"),
                OSCListener.VRCFTExpression.GetByKey1("TongueUp") - OSCListener.VRCFTExpression.GetByKey1("TongueDown"),
                OSCListener.VRCFTExpression.GetByKey1("TongueLongStep1") + OSCListener.VRCFTExpression.GetByKey1("TongueLongStep2"));

            _mouth.JawOpen = 0f;
            _mouth.MouthPout = OSCListener.VRCFTExpression.GetByKey1("MouthPout");
            _mouth.TongueRoll = OSCListener.VRCFTExpression.GetByKey1("TongueRoll");

            _mouth.LipBottomOverUnder = OSCListener.VRCFTExpression.GetByKey1("MouthLowerInside");
            _mouth.LipBottomOverturn = OSCListener.VRCFTExpression.GetByKey1("MouthLowerOverturn");
            _mouth.LipTopOverUnder = OSCListener.VRCFTExpression.GetByKey1("MouthUpperInside");
            _mouth.LipTopOverturn = OSCListener.VRCFTExpression.GetByKey1("MouthUpperOverturn");

            _mouth.LipLowerHorizontal = 0f;
            _mouth.LipUpperHorizontal = 0f;

            _mouth.LipLowerLeftRaise = OSCListener.VRCFTExpression.GetByKey1("MouthLowerLeft");
            _mouth.LipLowerRightRaise = OSCListener.VRCFTExpression.GetByKey1("MouthLowerRight");
            _mouth.LipUpperRightRaise = OSCListener.VRCFTExpression.GetByKey1("MouthUpperRight");
            _mouth.LipUpperLeftRaise = OSCListener.VRCFTExpression.GetByKey1("MouthUpperUpLeft");

            _mouth.MouthRightSmileFrown = OSCListener.VRCFTExpression.GetByKey1("MouthSmileRight") - OSCListener.VRCFTExpression.GetByKey1("MouthSadRight");
            _mouth.MouthLeftSmileFrown = OSCListener.VRCFTExpression.GetByKey1("MouthSmileLeft") - OSCListener.VRCFTExpression.GetByKey1("MouthSadLeft");            _mouth.CheekLeftPuffSuck = 0f;
            _mouth.CheekRightPuffSuck = OSCListener.VRCFTExpression.GetByKey1("CheekPuffRight") - OSCListener.VRCFTExpression.GetByKey1("CheekSuck");
            _mouth.CheekLeftPuffSuck = OSCListener.VRCFTExpression.GetByKey1("CheekPuffLeft") - OSCListener.VRCFTExpression.GetByKey1("CheekSuck");
        }
    }
}
