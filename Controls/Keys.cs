namespace Controls
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows;

    public static class Keys
    {
        private static readonly Dictionary<string, ComponentResourceKey> _keys = new Dictionary<string, ComponentResourceKey>(); 
        public static readonly ComponentResourceKey RedBrushKey = new ComponentResourceKey(typeof(Keys), "RedBrushKey");

        public static ComponentResourceKey GreenBrushKey
        {
            get { return Get(); }
        }

        private static ComponentResourceKey Get([CallerMemberName] string caller = null)
        {
            ComponentResourceKey key;
            if (!_keys.TryGetValue(caller, out key))
            {
                key = new ComponentResourceKey(typeof(Keys),caller);
                _keys.Add(caller, key);
            }
            return key;
        }
    }
}
