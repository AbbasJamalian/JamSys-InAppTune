using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamSys.InAppTune.DQN
{
    public interface IDeepQNet : IDisposable
    {
        public float TrainSingle(List<float> currentState, int action, float reward, List<float> nextState, float learningRate, float discountFactor);
        public long GetAction(List<float> state);

        public void CopyQNetToTarget();
        void Reset();
    }
}
