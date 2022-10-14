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
                OSCListener.VRCFTExpression.GetByKey1("JawRight").Value - OSCListener.VRCFTExpression.GetByKey1("JawLeft").Value,
                OSCListener.VRCFTExpression.GetByKey1("JawOpen").Value,
                OSCListener.VRCFTExpression.GetByKey1("JawForward").Value
            );
            _mouth.Tongue = new float3(
                OSCListener.VRCFTExpression.GetByKey1("TongueRight").Value - OSCListener.VRCFTExpression.GetByKey1("TongueLeft").Value,
                OSCListener.VRCFTExpression.GetByKey1("TongueUp").Value - OSCListener.VRCFTExpression.GetByKey1("TongueDown").Value,
                OSCListener.VRCFTExpression.GetByKey1("TongueLongStep1").Value + OSCListener.VRCFTExpression.GetByKey1("TongueLongStep2").Value);

            _mouth.JawOpen = 0f;
            _mouth.MouthPout = OSCListener.VRCFTExpression.GetByKey1("MouthPout").Value;
            _mouth.TongueRoll = OSCListener.VRCFTExpression.GetByKey1("TongueRoll").Value;

            _mouth.LipBottomOverUnder = OSCListener.VRCFTExpression.GetByKey1("MouthLowerInside").Value;
            _mouth.LipBottomOverturn = OSCListener.VRCFTExpression.GetByKey1("MouthLowerOverturn").Value;
            _mouth.LipTopOverUnder = OSCListener.VRCFTExpression.GetByKey1("MouthUpperInside").Value;
            _mouth.LipTopOverturn = OSCListener.VRCFTExpression.GetByKey1("MouthUpperOverturn").Value;

            _mouth.LipLowerHorizontal = 0f;
            _mouth.LipUpperHorizontal = 0f;

            _mouth.LipLowerLeftRaise = OSCListener.VRCFTExpression.GetByKey1("MouthLowerLeft").Value;
            _mouth.LipLowerRightRaise = OSCListener.VRCFTExpression.GetByKey1("MouthLowerRight").Value;
            _mouth.LipUpperRightRaise = OSCListener.VRCFTExpression.GetByKey1("MouthUpperRight").Value;
            _mouth.LipUpperLeftRaise = OSCListener.VRCFTExpression.GetByKey1("MouthUpperUpLeft").Value;

            _mouth.MouthRightSmileFrown = OSCListener.VRCFTExpression.GetByKey1("MouthSmileRight").Value - OSCListener.VRCFTExpression.GetByKey1("MouthSadRight").Value;
            _mouth.MouthLeftSmileFrown = OSCListener.VRCFTExpression.GetByKey1("MouthSmileLeft").Value - OSCListener.VRCFTExpression.GetByKey1("MouthSadLeft").Value;            _mouth.CheekLeftPuffSuck = 0f;
            _mouth.CheekRightPuffSuck = OSCListener.VRCFTExpression.GetByKey1("CheekPuffRight").Value - OSCListener.VRCFTExpression.GetByKey1("CheekSuck").Value;
            _mouth.CheekLeftPuffSuck = OSCListener.VRCFTExpression.GetByKey1("CheekPuffLeft").Value - OSCListener.VRCFTExpression.GetByKey1("CheekSuck").Value;
        }
    }
}
