using System;
using System.Globalization;

namespace NanoSoft.Maths
{
    public class NumberToWord
    {
        private static readonly string[] EnglishOnes =
        {
            "Zero",
            "One",
            "Two",
            "Three",
            "Four",
            "Five",
            "Six",
            "Seven",
            "Eight",
            "Nine",
            "Ten",
            "Eleven",
            "Twelve",
            "Thirteen",
            "Fourteen",
            "Fifteen",
            "Sixteen",
            "Seventeen",
            "Eighteen",
            "Nineteen"
        };

        private static readonly string[] EnglishTens =
        {
            "Twenty",
            "Thirty",
            "Forty",
            "Fifty",
            "Sixty",
            "Seventy",
            "Eighty",
            "Ninety"
        };

        private static readonly string[] EnglishGroup =
        {
            "Hundred",
            "Thousand",
            "Million",
            "Billion",
            "Trillion",
            "Quadrillion",
            "Quintillion",
            "Sextillian",
            "Septillion",
            "Octillion",
            "Nonillion",
            "Decillion",
            "Undecillion",
            "Duodecillion",
            "Tredecillion",
            "Quattuordecillion",
            "Quindecillion",
            "Sexdecillion",
            "Septendecillion",
            "Octodecillion",
            "Novemdecillion",
            "Vigintillion",
            "Unvigintillion",
            "Duovigintillion",
            "10^72",
            "10^75",
            "10^78",
            "10^81",
            "10^84",
            "10^87",
            "Vigintinonillion",
            "10^93",
            "10^96",
            "Duotrigintillion",
            "Trestrigintillion"
        };

        private static readonly string[] ArabicOnes =
        {
            string.Empty,
            "واحد",
            "اثنان",
            "ثلاثة",
            "أربعة",
            "خمسة",
            "ستة",
            "سبعة",
            "ثمانية",
            "تسعة",
            "عشرة",
            "أحد عشر",
            "اثنا عشر",
            "ثلاثة عشر",
            "أربعة عشر",
            "خمسة عشر",
            "ستة عشر",
            "سبعة عشر",
            "ثمانية عشر",
            "تسعة عشر"
        };

        public static readonly string[] ArabicFeminineOnes =
        {
            string.Empty,
            "إحدى",
            "اثنتان",
            "ثلاث",
            "أربع",
            "خمس",
            "ست",
            "سبع",
            "ثمان",
            "تسع",
            "عشر",
            "إحدى عشرة",
            "اثنتا عشرة",
            "ثلاث عشرة",
            "أربع عشرة",
            "خمس عشرة",
            "ست عشرة",
            "سبع عشرة",
            "ثماني عشرة",
            "تسع عشرة"
        };

        private static readonly string[] ArabicTens =
        {
            "عشرون",
            "ثلاثون",
            "أربعون",
            "خمسون",
            "ستون",
            "سبعون",
            "ثمانون",
            "تسعون"
        };

        private static readonly string[] ArabicHundreds =
        {
            "",
            "مائة",
            "مئتان",
            "ثلاثمائة",
            "أربعمائة",
            "خمسمائة",
            "ستمائة",
            "سبعمائة",
            "ثمانمائة",
            "تسعمائة"
        };

        private static readonly string[] ArabicAppendedTwos =
        {
            "مئتا",
            "ألفا",
            "مليونا",
            "مليارا",
            "تريليونا",
            "كوادريليونا",
            "كوينتليونا",
            "سكستيليونا"
        };

        private static readonly string[] ArabicTwos =
        {
            "مئتان",
            "ألفان",
            "مليونان",
            "ملياران",
            "تريليونان",
            "كوادريليونان",
            "كوينتليونان",
            "سكستيليونان"
        };

        private static readonly string[] ArabicGroup =
        {
            "مائة",
            "ألف",
            "مليون",
            "مليار",
            "تريليون",
            "كوادريليون",
            "كوينتليون",
            "سكستيليون"
        };

        private static readonly string[] ArabicAppendedGroup =
        {
            "",
            "ألفاً",
            "مليوناً",
            "ملياراً",
            "تريليوناً",
            "كوادريليوناً",
            "كوينتليوناً",
            "سكستيليوناً"
        };

        private static readonly string[] ArabicPluralGroups =
        {
            "",
            "آلاف",
            "ملايين",
            "مليارات",
            "تريليونات",
            "كوادريليونات",
            "كوينتليونات",
            "سكستيليونات"
        };

        private long _intergerValue;
        private int _decimalValue;

        public decimal Number { get; set; }

        public CurrencyInfo Currency { get; set; }

        public string EnglishPrefixText { get; set; }

        public string EnglishSuffixText { get; set; }

        public string ArabicPrefixText { get; set; }

        public string ArabicSuffixText { get; set; }

        public NumberToWord(decimal number)
        {
            InitializeClass(number, new CurrencyInfo(CurrencyInfo.Currencies.Libya),
                string.Empty, "only.", "فقط", " لا غير.");
        }

        public NumberToWord(decimal number, CurrencyInfo currency)
        {
            InitializeClass(number, currency, string.Empty, "only.", "فقط", " لا غير.");
        }

        public NumberToWord(decimal number, CurrencyInfo currency, string englishPrefixText,
            string englishSuffixText, string arabicPrefixText, string arabicSuffixText)
        {
            InitializeClass(number, currency, englishPrefixText, englishSuffixText, arabicPrefixText,
                arabicSuffixText);
        }

        private void InitializeClass(decimal number, CurrencyInfo currency, string englishPrefixText,
            string englishSuffixText, string arabicPrefixText, string arabicSuffixText)
        {
            Number = number;
            Currency = currency;
            EnglishPrefixText = englishPrefixText;
            EnglishSuffixText = englishSuffixText;
            ArabicPrefixText = arabicPrefixText;
            ArabicSuffixText = arabicSuffixText;
            ExtractIntegerAndDecimalParts();
        }

        private string GetDecimalValue(string decimalPart)
        {
            string str;
            if (Currency.PartPrecision != decimalPart.Length)
                str = Math.Round(Convert.ToDecimal(string.Format("{0}.{1}", new object[]
                {
                    decimalPart.Substring(0, Currency.PartPrecision),
                    decimalPart.Substring(Currency.PartPrecision,
                        decimalPart.Length - Currency.PartPrecision)
                }))).ToString(CultureInfo.InvariantCulture);
            else
                str = decimalPart;
            for (var index = 0; index < (int)Currency.PartPrecision - str.Length; ++index)
                str += "0";
            return str;
        }

        private void ExtractIntegerAndDecimalParts()
        {
            var strArray = Number.ToString(CultureInfo.InvariantCulture).Split('.');
            _intergerValue = Convert.ToInt32(strArray[0]);
            if (strArray.Length <= 1)
                return;
            _decimalValue = Convert.ToInt32(GetDecimalValue(strArray[1]));
        }

        private string ProcessGroup(int groupNumber)
        {
            var index1 = groupNumber % 100;
            var index2 = groupNumber / 100;
            var str = string.Empty;
            if (index2 > 0)
                str = string.Format("{0} {1}", new object[]
                {
                    EnglishOnes[index2],
                    EnglishGroup[0]
                });
            if (index1 > 0)
            {
                if (index1 < 20)
                {
                    str = str + (str != string.Empty ? " " : string.Empty) + EnglishOnes[index1];
                }
                else
                {
                    var index3 = index1 % 10;
                    var index4 = index1 / 10 - 2;
                    str = str + (str != string.Empty ? " " : string.Empty) + EnglishTens[index4];
                    if (index3 > 0)
                        str = str + (str != string.Empty ? " " : string.Empty) + EnglishOnes[index3];
                }
            }

            return str;
        }

        public string ConvertToEnglish()
        {
            var number = Number;
            if (number == decimal.Zero)
                return "Zero";
            var str1 = ProcessGroup(_decimalValue);
            var str2 = string.Empty;
            var index = 0;
            if (number < decimal.One)
            {
                str2 = EnglishOnes[0];
            }
            else
            {
                while (number >= decimal.One)
                {
                    var groupNumber = (int)(number % new decimal(1000));
                    number /= new decimal(1000);
                    var str3 = ProcessGroup(groupNumber);
                    if (str3 != string.Empty)
                    {
                        if (index > 0)
                            str2 = string.Format("{0} {1}", new object[]
                            {
                                EnglishGroup[index],
                                str2
                            });
                        str2 = string.Format("{0} {1}", new object[]
                        {
                            str3,
                            str2
                        });
                    }

                    ++index;
                }
            }

            var empty = string.Empty;
            var str4 = EnglishPrefixText == string.Empty ? string.Empty : string.Format("{0} ", EnglishPrefixText);
            var str5 = empty + str4 + (str2 != string.Empty ? str2 : string.Empty) +
                       (str2 != string.Empty
                           ? (_intergerValue == 1L
                               ? Currency.EnglishCurrencyName
                               : Currency.EnglishPluralCurrencyName)
                           : string.Empty) + (str1 != string.Empty ? " and " : string.Empty) +
                       (str1 != string.Empty ? str1 : string.Empty) + (str1 != string.Empty
                           ? " " + (_decimalValue == 1
                                 ? Currency.EnglishCurrencyPartName
                                 : Currency.EnglishPluralCurrencyPartName)
                           : string.Empty);
            var str6 = EnglishSuffixText == string.Empty ? string.Empty : string.Format(" {0}", EnglishSuffixText);
            return str5 + str6;
        }

        private string GetDigitFeminineStatus(int digit, int groupLevel)
        {
            switch (groupLevel)
            {
                case -1:
                    if (Currency.IsCurrencyPartNameFeminine)
                        return ArabicFeminineOnes[digit];
                    return ArabicOnes[digit];
                case 0:
                    if (Currency.IsCurrencyNameFeminine)
                        return ArabicFeminineOnes[digit];
                    return ArabicOnes[digit];
                default:
                    return ArabicOnes[digit];
            }
        }

        private string ProcessArabicGroup(int groupNumber, int groupLevel, decimal remainingNumber)
        {
            var digit1 = groupNumber % 100;
            var index1 = groupNumber / 100;
            var str = string.Empty;
            if (index1 > 0)
            {
                if (digit1 == 0 && index1 == 2)
                    str = string.Format("{0}", ArabicAppendedTwos[0]);
                else
                    str = string.Format("{0}", ArabicHundreds[index1]);
            }

            if (digit1 > 0)
            {
                if (digit1 < 20)
                {
                    if (digit1 == 2 && index1 == 0 && groupLevel > 0)
                    {
                        if (_intergerValue == 2000L || _intergerValue == 2000000L ||
                            (_intergerValue == 2000000000L || _intergerValue == 2000000000000L) ||
                            (_intergerValue == 2000000000000000L || _intergerValue == 2000000000000000000L))
                            str = string.Format("{0}", ArabicAppendedTwos[groupLevel]);
                        else
                            str = string.Format("{0}", ArabicTwos[groupLevel]);
                    }
                    else
                    {
                        if (str != string.Empty)
                            str += " و ";
                        str = digit1 != 1 || groupLevel <= 0
                            ? (digit1 != 1 && digit1 != 2 || groupLevel != 0 && groupLevel != -1 ||
                               (index1 != 0 || !(remainingNumber == decimal.Zero))
                                ? str + GetDigitFeminineStatus(digit1, groupLevel)
                                : str + string.Empty)
                            : str + ArabicGroup[groupLevel];
                    }
                }
                else
                {
                    var digit2 = digit1 % 10;
                    var index2 = digit1 / 10 - 2;
                    if (digit2 > 0)
                    {
                        if (str != string.Empty)
                            str += " و ";
                        str += GetDigitFeminineStatus(digit2, groupLevel);
                    }

                    if (str != string.Empty)
                        str += " و ";
                    str += ArabicTens[index2];
                }
            }

            return str;
        }

        public string ConvertToArabic()
        {
            var number = Number;
            if (number == decimal.Zero)
                return "صفر";
            var str1 = ProcessArabicGroup(_decimalValue, -1, decimal.Zero);
            var str2 = string.Empty;
            byte num1 = 0;
            while (number >= decimal.One)
            {
                var groupNumber = (int)(number % new decimal(1000));
                number /= new decimal(1000);
                var str3 = ProcessArabicGroup(groupNumber, num1, Math.Floor(number));
                if (str3 != string.Empty)
                {
                    if (num1 > 0)
                    {
                        if (str2 != string.Empty)
                            str2 = string.Format("{0} {1}", new object[]
                            {
                                "و",
                                str2
                            });
                        if (groupNumber != 2 && groupNumber % 100 != 1)
                        {
                            if (groupNumber >= 3 && groupNumber <= 10)
                                str2 = string.Format("{0} {1}", new object[]
                                {
                                    ArabicPluralGroups[num1],
                                    str2
                                });
                            else if (str2 != string.Empty)
                                str2 = string.Format("{0} {1}", new object[]
                                {
                                    ArabicAppendedGroup[num1],
                                    str2
                                });
                            else
                                str2 = string.Format("{0} {1}", new object[]
                                {
                                    ArabicGroup[num1],
                                    str2
                                });
                        }
                    }

                    str2 = string.Format("{0} {1}", new object[]
                    {
                        str3,
                        str2
                    });
                }

                ++num1;
            }

            var empty = string.Empty;
            var str4 = ArabicPrefixText == string.Empty ? string.Empty : string.Format("{0} ", ArabicPrefixText);
            var str5 = empty + str4 + (str2 != string.Empty ? str2 : string.Empty);
            if (_intergerValue != 0L)
            {
                var num2 = (int)(_intergerValue % 100L);
                switch (num2)
                {
                    case 0:
                        str5 += Currency.Arabic1CurrencyName;
                        break;
                    case 1:
                        str5 += Currency.Arabic1CurrencyName;
                        break;
                    case 2:
                        str5 = _intergerValue != 2L
                            ? str5 + Currency.Arabic1CurrencyName
                            : str5 + Currency.Arabic2CurrencyName;
                        break;
                    default:
                        if (num2 >= 3 && num2 <= 10)
                        {
                            str5 += Currency.Arabic310CurrencyName;
                            break;
                        }

                        if (num2 >= 11 && num2 <= 99)
                        {
                            str5 += Currency.Arabic1199CurrencyName;
                        }

                        break;
                }
            }

            var str6 = str5 + (_decimalValue != 0 ? " و " : string.Empty) +
                       (_decimalValue != 0 ? str1 : string.Empty);
            if (_decimalValue != 0)
            {
                str6 += " ";
                var num2 = _decimalValue % 100;
                switch (num2)
                {
                    case 0:
                        str6 += Currency.Arabic1CurrencyPartName;
                        break;
                    case 1:
                        str6 += Currency.Arabic1CurrencyPartName;
                        break;
                    case 2:
                        str6 += Currency.Arabic2CurrencyPartName;
                        break;
                    default:
                        if (num2 >= 3 && num2 <= 10)
                        {
                            str6 += Currency.Arabic310CurrencyPartName;
                            break;
                        }

                        if (num2 >= 11 && num2 <= 99)
                        {
                            str6 += Currency.Arabic1199CurrencyPartName;
                        }

                        break;
                }
            }

            var str7 = str6;
            var str8 = ArabicSuffixText == string.Empty ? string.Empty : string.Format(" {0}", ArabicSuffixText);
            return str7 + str8;
        }

        public class CurrencyInfo
        {
            public CurrencyInfo(Currencies currency)
            {
                switch (currency)
                {
                    case Currencies.Syria:
                        IsCurrencyNameFeminine = true;
                        EnglishCurrencyName = "Syrian Pound";
                        EnglishPluralCurrencyName = "Syrian Pounds";
                        EnglishCurrencyPartName = "Piaster";
                        EnglishPluralCurrencyPartName = "Piasteres";
                        Arabic1CurrencyName = "ليرة سورية";
                        Arabic2CurrencyName = "ليرتان سوريتان";
                        Arabic310CurrencyName = "ليرات سورية";
                        Arabic1199CurrencyName = "ليرة سورية";
                        Arabic1CurrencyPartName = "قرش";
                        Arabic2CurrencyPartName = "قرشان";
                        Arabic310CurrencyPartName = "قروش";
                        Arabic1199CurrencyPartName = "قرشاً";
                        PartPrecision = 2;
                        IsCurrencyPartNameFeminine = false;
                        break;
                    case Currencies.Uae:
                        IsCurrencyNameFeminine = false;
                        EnglishCurrencyName = "UAE Dirham";
                        EnglishPluralCurrencyName = "UAE Dirhams";
                        EnglishCurrencyPartName = "Fils";
                        EnglishPluralCurrencyPartName = "Fils";
                        Arabic1CurrencyName = "درهم إماراتي";
                        Arabic2CurrencyName = "درهمان إماراتيان";
                        Arabic310CurrencyName = "دراهم إماراتية";
                        Arabic1199CurrencyName = "درهماً إماراتياً";
                        Arabic1CurrencyPartName = "فلس";
                        Arabic2CurrencyPartName = "فلسان";
                        Arabic310CurrencyPartName = "فلوس";
                        Arabic1199CurrencyPartName = "فلساً";
                        PartPrecision = 2;
                        IsCurrencyPartNameFeminine = false;
                        break;
                    case Currencies.SaudiArabia:
                        IsCurrencyNameFeminine = false;
                        EnglishCurrencyName = "Saudi Riyal";
                        EnglishPluralCurrencyName = "Saudi Riyals";
                        EnglishCurrencyPartName = "Halala";
                        EnglishPluralCurrencyPartName = "Halalas";
                        Arabic1CurrencyName = "ريال سعودي";
                        Arabic2CurrencyName = "ريالان سعوديان";
                        Arabic310CurrencyName = "ريالات سعودية";
                        Arabic1199CurrencyName = "ريالاً سعودياً";
                        Arabic1CurrencyPartName = "هللة";
                        Arabic2CurrencyPartName = "هللتان";
                        Arabic310CurrencyPartName = "هللات";
                        Arabic1199CurrencyPartName = "هللة";
                        PartPrecision = 2;
                        IsCurrencyPartNameFeminine = true;
                        break;
                    case Currencies.Gold:
                        IsCurrencyNameFeminine = false;
                        EnglishCurrencyName = "Gram";
                        EnglishPluralCurrencyName = "Grams";
                        EnglishCurrencyPartName = "Milligram";
                        EnglishPluralCurrencyPartName = "Milligrams";
                        Arabic1CurrencyName = "جرام";
                        Arabic2CurrencyName = "جرامان";
                        Arabic310CurrencyName = "جرامات";
                        Arabic1199CurrencyName = "جراماً";
                        Arabic1CurrencyPartName = "ملجرام";
                        Arabic2CurrencyPartName = "ملجرامان";
                        Arabic310CurrencyPartName = "ملجرامات";
                        Arabic1199CurrencyPartName = "ملجراماً";
                        PartPrecision = 2;
                        IsCurrencyPartNameFeminine = false;
                        break;
                    case Currencies.Libya:
                        IsCurrencyNameFeminine = false;
                        EnglishCurrencyName = "Libyan Dinar";
                        EnglishPluralCurrencyName = "Libyan Dinars";
                        EnglishCurrencyPartName = "dirham";
                        EnglishPluralCurrencyPartName = "dirhams";
                        Arabic1CurrencyName = "دينار ليبي";
                        Arabic2CurrencyName = "ديناران ليبيان";
                        Arabic310CurrencyName = "دنانير ليبية";
                        Arabic1199CurrencyName = "ديناراً ليبياً";
                        Arabic1CurrencyPartName = "درهم";
                        Arabic2CurrencyPartName = "درهمان";
                        Arabic310CurrencyPartName = "دراهم";
                        Arabic1199CurrencyPartName = "درهماً";
                        PartPrecision = 3;
                        IsCurrencyPartNameFeminine = false;
                        break;
                    case Currencies.Dinar:
                        IsCurrencyNameFeminine = false;
                        EnglishCurrencyName = "Dinar";
                        EnglishPluralCurrencyName = "Dinars";
                        EnglishCurrencyPartName = "dirham";
                        EnglishPluralCurrencyPartName = "dirhams";
                        Arabic1CurrencyName = "دينار";
                        Arabic2CurrencyName = "ديناران";
                        Arabic310CurrencyName = "دنانير";
                        Arabic1199CurrencyName = "ديناراً";
                        Arabic1CurrencyPartName = "درهم";
                        Arabic2CurrencyPartName = "درهمان";
                        Arabic310CurrencyPartName = "دراهم";
                        Arabic1199CurrencyPartName = "درهماً";
                        PartPrecision = 2;
                        IsCurrencyPartNameFeminine = false;
                        break;
                }
            }

            public bool IsCurrencyNameFeminine { get; set; }

            public string EnglishCurrencyName { get; set; }

            public string EnglishPluralCurrencyName { get; set; }

            public string Arabic1CurrencyName { get; set; }

            public string Arabic2CurrencyName { get; set; }

            public string Arabic310CurrencyName { get; set; }

            public string Arabic1199CurrencyName { get; set; }

            public byte PartPrecision { get; set; }

            public bool IsCurrencyPartNameFeminine { get; set; }

            public string EnglishCurrencyPartName { get; set; }

            public string EnglishPluralCurrencyPartName { get; set; }

            public string Arabic1CurrencyPartName { get; set; }

            public string Arabic2CurrencyPartName { get; set; }

            public string Arabic310CurrencyPartName { get; set; }

            public string Arabic1199CurrencyPartName { get; set; }

            public enum Currencies
            {
                Syria,
                Uae,
                SaudiArabia,
                Dinar,
                Tunisia,
                Gold,
                Libya
            }
        }
    }
}