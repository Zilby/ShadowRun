namespace UnityWeld.Binding.Adapters
{
    /// <summary>
    /// Adapter that converts a float to a string.
    /// </summary>
    [Adapter(typeof(float), typeof(string), typeof(FloatToStringAdapterOptions))]
    public class FloatToStringAdapter : IAdapter
    {
        public object Convert(object valueIn, AdapterOptions options)
        {
            var ftsOptions = options as FloatToStringAdapterOptions;
            var format = ftsOptions ? ftsOptions.Format : "{0}";
            var val = valueIn as float?;
            return val.HasValue ? val.Value.ToString(format) : "";
        }
    }
}
