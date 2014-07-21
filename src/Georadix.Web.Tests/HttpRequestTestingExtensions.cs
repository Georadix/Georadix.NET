namespace System.Web
{
    using System.Collections;
    using System.Reflection;

    /// <summary>
    /// Defines methods that extends <see cref="HttpRequest"/> to facilitate testing.
    /// </summary>
    public static class HttpRequestTestingExtensions
    {
        /// <summary>
        /// Adds the specified header to an <see cref="HttpRequest"/>.
        /// </summary>
        /// <remarks>
        /// This is a necessary workaround to adding directly to the header collection on an 
        /// <see cref="HttpRequest"/> which throws a <b>PlatformNotSupportedException</b>.
        /// </remarks>
        /// <param name="request">The request.</param>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        public static void AddHeader(this HttpRequest request, string name, params string[] values)
        {
            var headers = request.Headers;
            var t = headers.GetType();
            var item = new ArrayList();

            t.InvokeMember(
                "MakeReadWrite",
                BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                headers,
                null);

            t.InvokeMember(
                "InvalidateCachedArrays",
                BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                headers,
                null);

            item.AddRange(values);

            t.InvokeMember(
                "BaseAdd",
                BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                headers,
                new object[] { name, item });

            t.InvokeMember(
                "MakeReadOnly",
                BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                headers,
                null);
        }
    }
}