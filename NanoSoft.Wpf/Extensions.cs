using NanoSoft.Wpf.Components;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Media.Imaging;

namespace NanoSoft.Wpf
{
    public static class Extensions
    {
        public static string GetCulture(this Language language)
        {
            switch (language)
            {
                case Language.Arabic:
                    return "ar-LY";
                case Language.English:
                    return "en-US";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language));
            }
        }


        public static Language GetLanguage(this string culture)
        {
            if (string.IsNullOrWhiteSpace(culture))
                return Language.Arabic;

            switch (culture)
            {
                case "en-US":
                    return Language.English;

                case "ar-LY":
                    return Language.Arabic;

                default:
                    throw new ArgumentOutOfRangeException(nameof(culture));
            }
        }

        public static DateTime? ArrangeDate(this DateTime? from, DateTime? to)
        {
            if (from == null)
                return to;

            if (to != null && to >= from)
                return to;

            return from;
        }


        public static BitmapImage ToBitmapImage(this byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;

            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        public static string ToUnsecureString(this SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
