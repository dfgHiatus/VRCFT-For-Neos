using BaseX;
using FrooxEngine;

namespace VRCFT.Neos
{
    public class EyeDevice : IInputDriver
    {
        private Eyes _eyes;
        public int UpdateOrder => 100;

        public EyeDevice()
        {
            Engine.Current.OnShutdown += Teardown;
        }

        private void Teardown()
        {
            
        }

        public void CollectDeviceInfos(DataTreeList list)
        {
            var eyeDataTreeDictionary = new DataTreeDictionary();
            eyeDataTreeDictionary.Add("Name", "VRCFT Eye Tracking");
            eyeDataTreeDictionary.Add("Type", "Eye Tracking");
            eyeDataTreeDictionary.Add("Model", "VRCFT Eye Model");
            list.Add(eyeDataTreeDictionary);
        }

        public void RegisterInputs(InputInterface inputInterface)
        {
            _eyes = new Eyes(inputInterface, "VRCFT Eye Tracking");
        }

        // No dictionary entries in this method should be null
        public void UpdateInputs(float deltaTime)
        {
            _eyes.IsEyeTrackingActive = _eyes.IsEyeTrackingActive;

            UpdateEye(
                Project2DTo3D(OSCListener.VRCFTExpression.GetByKey1("EyeLeftX"), OSCListener.VRCFTExpression.GetByKey1("EyeLeftY")),
                float3.Zero, 
                true,
                OSCListener.VRCFTExpression.GetByKey1("EyesPupilDiameter"),
                OSCListener.VRCFTExpression.GetByKey1("LeftEyeLid"),
                OSCListener.VRCFTExpression.GetByKey1("LeftEyeWiden"),
                OSCListener.VRCFTExpression.GetByKey1("LeftEyeSqueeze"),
                0f, 
                deltaTime, 
                _eyes.LeftEye);
            
            UpdateEye(
                Project2DTo3D(OSCListener.VRCFTExpression.GetByKey1("EyeRightX"), OSCListener.VRCFTExpression.GetByKey1("EyeRightY")),
                float3.Zero, 
                true,
                OSCListener.VRCFTExpression.GetByKey1("EyesPupilDiameter"),
                OSCListener.VRCFTExpression.GetByKey1("RightEyeLid"),
                OSCListener.VRCFTExpression.GetByKey1("RightEyeWiden"),
                OSCListener.VRCFTExpression.GetByKey1("RightEyeSqueeze"),
                0f, 
                deltaTime, 
                _eyes.RightEye);

            UpdateEye(
                Project2DTo3D(OSCListener.VRCFTExpression.GetByKey1("EyesX"), OSCListener.VRCFTExpression.GetByKey1("EyesY")),
                float3.Zero,
                true,
                OSCListener.VRCFTExpression.GetByKey1("EyesPupilDiameter"),
                OSCListener.VRCFTExpression.GetByKey1("CombinedEyeLid"),
                OSCListener.VRCFTExpression.GetByKey1("EyesWiden"),
                OSCListener.VRCFTExpression.GetByKey1("EyesSqueeze"),
                0f,
                deltaTime, 
                _eyes.CombinedEye);
            
            _eyes.ComputeCombinedEyeParameters();

            _eyes.ConvergenceDistance = 0f;
            _eyes.Timestamp += deltaTime;
            _eyes.FinishUpdate();
        }

        private void UpdateEye(float3 gazeDirection, float3 gazeOrigin, bool status, float pupilSize, float openness,
            float widen, float squeeze, float frown, float deltaTime, Eye eye)
        {
            eye.IsDeviceActive = Engine.Current.InputInterface.VR_Active;
            eye.IsTracking = status;

            if (eye.IsTracking)
            {
                eye.UpdateWithDirection(gazeDirection);
                eye.RawPosition = gazeOrigin;
                eye.PupilDiameter = pupilSize;
            }

            eye.Openness = openness;
            eye.Widen = widen;
            eye.Squeeze = squeeze;
            eye.Frown = frown;
        }

        private static float3 Project2DTo3D(float x, float y)
        {
            return new float3(MathX.Tan(VRCFTNeos.Config.GetValue(VRCFTNeos.Alpha) * x),
                              MathX.Tan(VRCFTNeos.Config.GetValue(VRCFTNeos.Beta) * y),
                              1f).Normalized;
        }
    }
}
