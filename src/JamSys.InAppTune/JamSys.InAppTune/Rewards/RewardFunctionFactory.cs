using JamSys.InAppTune.Agent;

namespace JamSys.InAppTune.Rewards
{
    public class RewardFunctionFactory
    {
        public RewardFunctionFactory()
        {
            
        }

        public List<Type> EnumerateRewardFunctions()
        {
            var types = this.GetType().Assembly.GetExportedTypes().Where(t => t.GetInterface("JamSys.InAppTune.Rewards.IRewardFunction") != null).ToList();
            return types;
        }

        public IRewardFunction CreateRewardFunction(string name)
        {
            IRewardFunction result = null;
            var type = EnumerateRewardFunctions().FirstOrDefault(t => t.Name.Equals(name));
            if(type != null)
            {
                result = (IRewardFunction) this.GetType().Assembly.CreateInstance(type.FullName);
            }
            return result;
        }
    }
}
