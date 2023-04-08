using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace JamSys.InAppTune.DQN
{
    public class FullyConnectedNetworkFactory
    {
        private int _inputs;
        private int _outputs;

        private int _previousLayerSize;

        public FullyConnectedNetworkFactory(int inputs, int outputs)
        {
            _inputs = inputs;
            _outputs = outputs;

            _previousLayerSize = _inputs;
        }

        public List<Module> GetModuleList(string settings)
        {
            List<Module> _result = new();

            string[] moduleSettings = settings.Split(";");

            foreach (var moduleDef in moduleSettings)
            {
                if (!string.IsNullOrEmpty(moduleDef))
                {
                    var module = ParseModule(moduleDef);
                    if(module != null)
                        _result.Add(module);
                    else
                    {
                        Console.WriteLine($"ERROR: Unable to parse module {moduleDef}");
                    }
                }
            }

            return _result;
        }

        Module ParseModule(string moduleDef)
        {
            Module module = null;

            try
            {
                int startParamIndex = moduleDef.IndexOf('(');
                int endParamIndex = moduleDef.IndexOf(')');

                string moduleName = moduleDef.Substring(0, startParamIndex);
                string parameter = moduleDef.Substring(startParamIndex + 1, endParamIndex - startParamIndex - 1).Trim();
                switch (moduleName)
                {
                    case "Linear":
                    int layerSize;
                    if (string.IsNullOrEmpty(parameter))
                    {
                        //Last layer                        
                        layerSize = _outputs;
                    }
                    else
                    {
                        layerSize = int.Parse(parameter);
                    }
                    module = Linear(_previousLayerSize, layerSize);
                    _previousLayerSize = layerSize;
                    break;
                    case "LeakyReLU":
                    float slope = float.Parse(parameter);
                    module = LeakyReLU(negativeSlope: slope);
                    break;
                    case "Dropout":
                    float probability = float.Parse(parameter);
                    module = Dropout(probability);
                    break;
                    case "ReLU":
                    module = ReLU();
                    break;
                    case "Tanh":
                    module = Tanh();
                    break;
                    case "Sigmoid":
                    module = Sigmoid();
                    break;
                }

            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error building DQN - {ex.Message}");
                throw;
            }

            return module;
        }

    }
}