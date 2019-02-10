using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace NanoSoft.Extensions
{
    [PublicAPI]
    public static class SerializationExtensions
    {
        [NotNull]
        public static Task<string> ToSerializedObjectAsync([NotNull] this object obj, Format format = Format.Normal)
            => Task.Run(() => ToSerializedObject(obj, format));

        [NotNull]
        public static string ToSerializedObject([NotNull] this object obj, Format format = Format.Normal)
        {
            var setttings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return format == Format.Normal
                           ? JsonConvert.SerializeObject(obj, Formatting.None, setttings)
                           : JsonConvert.SerializeObject(obj, Formatting.Indented, setttings);
        }

        [CanBeNull]
        public static Task<TObject> ToDeserializedObjectAsync<TObject>([NotNull] this string serializedObject)
            where TObject : class
            => Task.Run(() => ToDeserializedObject<TObject>(serializedObject));

        [CanBeNull]
        public static TObject ToDeserializedObject<TObject>([NotNull] this string serializedObject)
            where TObject : class
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<TObject>(serializedObject);
                return obj;
            }
            catch
            {
                // ignored
            }
            return null;
        }
    }

    public enum Format
    {
        Normal,
        CamelCase
    }
}