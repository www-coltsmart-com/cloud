namespace ColtSmart.Core
{
    public class ConfigurationVariables
    {
        public string ConnectionString { get; set; }

        private static ConfigurationVariables defaultInstance = new ConfigurationVariables();
        public static ConfigurationVariables Default
        {
            get { return defaultInstance; }
            set { defaultInstance = value; }
        }


    }
}
