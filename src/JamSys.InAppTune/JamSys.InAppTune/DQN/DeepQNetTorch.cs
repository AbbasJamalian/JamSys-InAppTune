using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;
using static TorchSharp.torch.nn.functional;
using static TorchSharp.TensorExtensionMethods;
using TorchSharp.Modules;

namespace JamSys.InAppTune.DQN
{
    public class DeepQNetTorch : IDeepQNet
    {
        private int _numKnobs;
        private int _numOutput;
        private Sequential _qNetWork;
        private Sequential _targetNetWork;

        private string _toplogy;

        public Sequential QNetwork { get { return _qNetWork; } }
        public Sequential TargetNetwork { get { return _targetNetWork; } }


        /// <summary>
        /// Output is two times of input, for each knob there will be two output to increas or decrease it
        /// </summary>
        /// <param name="numKnobs"></param>
        public DeepQNetTorch(int numKnobs)
        {
            _numKnobs = numKnobs;
            _numOutput = _numKnobs * 2;
            _toplogy = null;
            Initialize();
        }

        public DeepQNetTorch(int numKnobs, string topology)
        {
            _numKnobs = numKnobs;
            _numOutput = _numKnobs * 2;
            _toplogy = topology;
            Initialize();
        }

        private void Initialize()
        {
            _qNetWork = CreateNetwork(_numKnobs, _numOutput);

            _targetNetWork = CreateNetwork(_numKnobs, _numOutput);

            CopyQNetToTarget();

        }

        Sequential CreateNetwork(int numInput, int numOutput)
        {
            Sequential seq = null;
            if(!string.IsNullOrEmpty(_toplogy))
            {
                //Create network from setting

                FullyConnectedNetworkFactory factory = new FullyConnectedNetworkFactory(numInput * 101, numOutput);
                //var modules = factory.GetModuleList("Linear(250);LeakyReLU(0.2);Dropout(0.1);Linear(150);LeakyReLU(0.2);Dropout(0.1);Linear(100);LeakyReLU(0.2);Dropout(0.1);Linear();");
                var modules = factory.GetModuleList(_toplogy);
                seq = Sequential(modules.ToArray());
            }
            else
            {

            int totalInputs = numInput * 101;
            int hiddenLayer1 = 250;
            int hiddenlayer2 = 150;
            int hiddenlayer3 = 100;
            seq = Sequential(
                Linear(totalInputs, hiddenLayer1),
                LeakyReLU(negativeSlope: 0.2),
                //Dropout(0.1),
                Linear(hiddenLayer1, hiddenlayer2),
                LeakyReLU(negativeSlope: 0.2),
                //Dropout(0.1),
                Linear(hiddenlayer2, hiddenlayer3),
                LeakyReLU(negativeSlope: 0.2),
                //Dropout(0.1),
                Linear(hiddenlayer3, numOutput)
                );

            }
            /*
            var seq = nn.Sequential(
               nn.Linear(numKnobs * 101, 256),
               nn.LeakyReLU(negativeSlope: 0.2),
               nn.Linear(256, 256),
               nn.Tanh(),
               nn.Dropout(0.3),
               nn.Linear(256, 128),
               nn.Tanh(),
               nn.Linear(128, 64),
               nn.Linear(64, numKnobs * 2)
               );
            */

            
            return seq;
        }

        public void CopyQNetToTarget()
        {
            _targetNetWork.load_state_dict(_qNetWork.state_dict());
        }

        public void Dispose()
        {
            if (_qNetWork != null)
            {
                _qNetWork.Dispose();
                _qNetWork = null;
            }

            if (_targetNetWork != null)
            {
                _targetNetWork.Dispose();
                _targetNetWork = null;
            }
        }

        public long GetAction(List<float> state)
        {
            var input = StateToOneHotVector(state);
            var result = _qNetWork.forward(torch.from_array(input));
            var resultArray = result.data<float>().ToArray();
            var outp = result.argmax();
            long iResult = outp.data<long>()[0];
            return iResult;
        }

        public float TrainSingle(List<float> currentState, int action, float reward, List<float> nextState, float learningRate, float discountFactor)
        {
            float learning_rate = learningRate;

            float gamma = discountFactor;

            var input = StateToOneHotVector(currentState);


            var predictedValues = _qNetWork.forward(torch.from_array(input));
            float predictedQValue = predictedValues.data<float>()[action];
            float nextStateQValue = GetMaxQValue(_targetNetWork, nextState);
            float targetQValue = reward + (gamma * nextStateQValue);


            var targetQArray = new float[_numOutput];
            for (int i = 0; i < targetQArray.Length; i++)
            {
                //targetQArray[i] = predictedValues.data<float>()[i]; 
                targetQArray[i] = 0;
            }
            targetQArray[action] = targetQValue;    //We only want to change the output node responsible for the action


            var resultBatch = torch.from_array(targetQArray);

            //_qNetWork.train(true);

            var optimizer = torch.optim.Adam(_qNetWork.parameters(), learning_rate);
            
            var loss = nn.functional.mse_loss();

            // Compute the loss
            var lossTensor = loss(predictedValues, resultBatch);

            // Clear the gradients before doing the back-propagation
            _qNetWork.zero_grad();

            // Do back-propagation, which computes all the gradients.
            lossTensor.backward();

            optimizer.step();

            //var result = loss(_qNetWork.forward(dataBatch), resultBatch).item<float>();
            //var result = loss(predictedValues, resultBatch).item<float>();

            var result = lossTensor.item<float>();

            predictedValues.Dispose();
            resultBatch.Dispose();
            optimizer.Dispose();
            lossTensor.Dispose();

            return result;
        }

        private float[] StateToOneHotVector(List<float> state)
        {
            int hotVectorSize = 101;
            float[] output = new float[state.Count * hotVectorSize];

            for (int i = 0; i < state.Count; i++)
            {
                int hotIndex = (int) Math.Round(state[i]);
                if (hotIndex > 100)
                    hotIndex = 100;
                for (int j = 0; j < hotVectorSize; j++)
                {
                    output[(i * hotVectorSize) + j] = j == hotIndex ? 1 : 0;
                    //output[(i * hotVectorSize) + j] = j <= hotIndex ? 1 : 0;
                }
            }

            return output;

        }

        private float GetMaxQValue(Sequential model, List<float> input)
        {
            var oneHot = StateToOneHotVector(input);
            var result = model.forward(torch.from_array(oneHot));
            var outmax = result.argmax();
            long maxIndex = outmax.data<long>()[0];
            float qValue = result.data<float>()[maxIndex];
            outmax.Dispose();
            result.Dispose();
            return qValue;
        }

        public void Reset()
        {
            if (_qNetWork != null)
            {
                _qNetWork.Dispose();
                _qNetWork = null;
            }

            if (_targetNetWork != null)
            {
                _targetNetWork.Dispose();
                _targetNetWork = null;
            }

            Initialize();
        }
    }
}
