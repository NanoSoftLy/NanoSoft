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
            => format == Format.Normal
                ? JsonConvert.SerializeObject(obj)
                : JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });


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