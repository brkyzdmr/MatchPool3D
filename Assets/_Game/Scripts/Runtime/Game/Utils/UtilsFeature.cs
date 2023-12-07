
    public class UtilsFeature : Feature
    {
        public UtilsFeature(Contexts contexts)
        {
            Add(new DebugLogMessageSystem(contexts));
        }
    }
