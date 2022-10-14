using BaseX;
using FrooxEngine;
using Rug.Osc;
using System;
using System.Net;
using System.Threading;
using VRCFT.Expressions;

namespace VRCFT.Neos
{
    public class OSCListener
    {
        public static TwoKeyDictionary<string, string, float> VRCFTExpression = new TwoKeyDictionary<string, string, float>();
        
        private static OscReceiver _receiver;
        private static Thread _thread;
        private const int DEFAULT_PORT = 9000;

        public OSCListener(int? port = null)
        {
            if (_receiver != null)
            {
                UniLog.Error("OSC connection already exists!");
                return;
            }

            IPAddress canidate;
            IPAddress.TryParse("127.0.0.1", out canidate);

            if (port.HasValue)
                _receiver = new OscReceiver(canidate, port.Value);
            else
                _receiver = new OscReceiver(canidate, DEFAULT_PORT);

            foreach (var shape in VRCFTExpressions.NormalizedEyeExpressions)
            {
                VRCFTExpression.Add(shape, VRCFTExpressions.VRCFT_OSC_PREFIX + shape, 0f);
            }
                
            foreach (var shape in VRCFTExpressions.NormalizedMouthExpressions)
            {
                VRCFTExpression.Add(shape, VRCFTExpressions.VRCFT_OSC_PREFIX + shape, 0f);
            }
                
            _thread = new Thread(new ThreadStart(ListenLoop));
            _receiver.Connect();
            _thread.Start();
        }

        public void SetupDevices()
        {
            // Where (value != 0) implies true
            if (Convert.ToBoolean(VRCFTExpression.GetByKey1("EyeTrackingActive")))
                Engine.Current.InputInterface.RegisterInputDriver(new EyeDevice());

            if (Convert.ToBoolean(VRCFTExpression.GetByKey1("MouthTrackingActive")))
                Engine.Current.OnReady += () => Engine.Current.InputInterface.RegisterInputDriver(new MouthDevice());
        }

        private static void ListenLoop()
        {
            OscPacket packet;
            OscMessage message;
            float candidate = 0;

            while (_receiver.State != OscSocketState.Closed)
            {
                try
                {
                    if (_receiver.State == OscSocketState.Connected)
                    {
                        packet = _receiver.Receive();
                        if (OscMessage.TryParse(packet.ToString(), out message))
                        {
                            if (VRCFTExpression.ContainsKey2(message.Address))
                            {
                                if (float.TryParse(message[0].ToString(), out candidate))
                                {
                                    VRCFTExpression.SetByKey2(message.Address, candidate);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    if (_receiver.State == OscSocketState.Connected)
                        UniLog.Error(e.Message);
                }
            }
        }

        public void Teardown()
        {
            _receiver.Close();
            _thread.Join();
        }
    }
}