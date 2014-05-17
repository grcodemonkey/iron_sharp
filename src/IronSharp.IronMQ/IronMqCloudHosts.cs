namespace IronSharp.IronMQ
{
    /// <summary>
    /// http://dev.iron.io/mq/reference/clouds/
    /// </summary>
    public static class IronMqCloudHosts
    {
        public const string DEFAULT = AWS_US_EAST_HOST;

        /// <summary>
        /// Default (Amazon US East)
        /// </summary>
        public const string AWS_US_EAST_HOST = "mq-aws-us-east-1.iron.io";

        /// <summary>
        /// Amazon Western Europe (Ireland)
        /// </summary>
        public const string AWS_EU_WEST_HOST = "mq-aws-eu-west-1.iron.io";

        /// <summary>
        /// Rackspace London Datacenter
        /// </summary>
        public const string RACKSPACE_LON = "mq-rackspace-lon.iron.io";

        /// <summary>
        /// Rackspace Chicago Datacenter
        /// </summary>
        public const string RACKSPACE_ORD = "mq-rackspace-ord.iron.io";

        /// <summary>
        /// Rackspace Dallas-Fortworth Datacenter - Pro Plans Only 
        /// </summary>
        public const string RACKSPACE_DFW = "mq-rackspace-dfw.iron.io";
    }
}