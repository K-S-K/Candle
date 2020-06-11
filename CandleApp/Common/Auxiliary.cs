using System;
using System.Xml.Linq;
using System.Globalization;

namespace CandleApp.Common
{
    public static partial class Auxiliary
    {
        #region Константы
        public const string dtFMT = @"yyyy.MM.dd HH:mm:ss.fff";
        public const string dtFMTSQL = @"yyyyMMdd HH:mm:ss.fff";
        public const string tsFMTMS = @"hh\:mm\:ss\.fff";
        public const string tsFMT = @"hh\:mm\:ss";
        public const string dyFMT = @"yyyy.MM.dd";
        public static CultureInfo SettingsCultureInfo = CultureInfo.InvariantCulture;
        #endregion /Константы

        /// <summary>
        /// Получение нервущегося эквивалента строки
        /// </summary>
        /// <param name="str">Исходная строка</param>
        /// <returns>Нервущаяся строка</returns>
        public static string NBSP(this string str) { return str.Replace(" ", "\u00A0").Replace("-", "\u2011"); }

        public static int DayOfWeekNumber(this DateTime DT)
        {
            DayOfWeek dow = DT.DayOfWeek;
            return DayOfWeekNumber(dow);
        }

        public static int DayOfWeekNumber(this DayOfWeek dow)
        {
            int number = 0;
            switch (dow)
            {
                case DayOfWeek.Monday: number = 1; break;
                case DayOfWeek.Tuesday: number = 2; break;
                case DayOfWeek.Wednesday: number = 3; break;
                case DayOfWeek.Thursday: number = 4; break;
                case DayOfWeek.Friday: number = 5; break;
                case DayOfWeek.Saturday: number = 6; break;
                case DayOfWeek.Sunday: number = 7; break;

                default: number = 0; break;
            }

            return number;
        }

        public static string ToStringSQL(this TimeSpan time, bool withMS = false, bool forBulk = false)
        {
            string dtStr = time.ToString(withMS ? @"hh\:mm\:ss\.fff" : @"hh\:mm\:ss");
            return string.Format(forBulk ? "{0}" : "'{0}'", dtStr);
        }

        public static string ToStringSQL(this DateTime dt, bool withMS = false, bool forBulk = false)
        {
            if (dt == default(DateTime)) return "NULL";
            string dtStr = dt.ToString(withMS ? "yyyyMMdd HH:mm:ss.fff" : "yyyyMMdd HH:mm:ss");
            return string.Format(forBulk ? "{0}" : "'{0}'", dtStr);
        }

        public static string ToStringDateSQL(this DateTime dt, bool forBulk = false)
        {
            if (dt == default(DateTime)) return "NULL";
            string dtStr = dt.ToString("yyyyMMdd");
            return string.Format(forBulk ? "{0}" : "'{0}'", dtStr);
        }

        public static string ToStringUI(this DateTime dt, bool withMS = false)
        {
            if (dt == default(DateTime)) return "--";
            return dt.ToString(withMS ? "yyyy.MM.dd HH:mm:ss.fff" : "yyyy.MM.dd HH:mm:ss");
        }

        public static string ToStringUI(this double val, bool noZero = false)
        {
            if (val == double.MinValue) return string.Empty;
            if (val == double.MaxValue) return string.Empty;

            if (noZero && Math.Abs(val) < 0.00001) return string.Empty;

            return val.ToString("### ### ##0.##").Trim().Replace("- ", "-").Replace("- ", "-");
        }

        public static string ToStringSQL(this double val)
        {
            return val.ToString().Replace(",", ".");
        }

        public static string ToStringUI(this int val, bool empty0 = false)
        {
            return ((long)val).ToStringUI(empty0);
        }

        public static string ToStringUI(this long val, bool empty0 = false)
        {
            if (val == 0 && empty0) return string.Empty;
            return val.ToString("### ### ###");
        }

        public static string ToStringDateUI(this DateTime dt)
        {
            if (dt == default(DateTime)) return string.Empty;
            if (dt == DateTime.MaxValue) return string.Empty;
            if (dt == DateTime.MinValue) return string.Empty;
            return dt.ToString("yyyy.MM.dd");
        }

        public static string ToStringTimeUI(this DateTime dt, bool bWithMS = false)
        {
            if (dt == default(DateTime)) return string.Empty;
            return dt.ToString(bWithMS ? "HH:mm:ss.fff" : "HH:mm:ss");
        }

        /// <summary>
        /// Интервал в виде строки
        /// </summary>
        /// <param name="d">Интервал</param>
        /// <param name="withLabels">Признак необходимости отображения меток hms</param>
        /// <returns></returns>
        public static string ToStringUI(this TimeSpan d, bool withLabels = false, bool withMs = false)
        {
            string fmt = withMs ?
                withLabels ? "{0,0000}h:{1:00}m:{2:00}s.{3:000}" : "{0,0000}:{1:00}:{2:00}.{3:000}" :
                withLabels ? "{0,0000}h:{1:00}m:{2:00}s" : "{0,0000}:{1:00}:{2:00}";
            string str = string.Format(fmt, (int)(d.TotalHours), d.Minutes, d.Seconds, d.Milliseconds);

            if (withLabels)
                while (str.StartsWith("0") || str.StartsWith(":") || str.StartsWith("h") || str.StartsWith("m") || str.StartsWith("s"))
                    str = str.TrimStart('0').TrimStart(':').TrimStart('h').TrimStart('m').TrimStart('s');
            else
                str = str.TrimStart('0').TrimStart(':');

            if (str.StartsWith(".")) str = "0" + str;

            return str;
        }

        public static string ToXMLString(this DateTime dt)
        {
            return dt.ToString(dtFMT, SettingsCultureInfo);
        }

        public static DateTime ToXMLDateTime(this string str)
        {
            return DateTime.ParseExact(str, dtFMT, SettingsCultureInfo);
        }

        public static DateTime ToSQLDateTime(this string str)
        {
            return DateTime.ParseExact(str, dtFMTSQL, SettingsCultureInfo);
        }

        public static string ToXMLDateString(this DateTime dt)
        {
            return dt.ToString(dyFMT, SettingsCultureInfo);
        }

        public static DateTime ToXMLDate(this string str)
        {
            return DateTime.ParseExact(str, dyFMT, SettingsCultureInfo).Date;
        }

        /// <summary>
        /// Сериализация метки времени в виде XML атрибута
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static XAttribute ToXmlAttr(this DateTime dt)
        {
            return new XAttribute("DT", dt.ToString("yyyy.MM.dd HH:mm:ss.fff"));
        }


        public static string ToXMLString(this TimeSpan t, bool bMS = false)
        {
            string fmt = bMS ? tsFMTMS : tsFMT;
            return t.ToString(fmt, SettingsCultureInfo);
        }

        public static TimeSpan ToXMLTime(this string str)
        {
            TimeSpan ts = TimeSpan.MinValue;

            try { ts = TimeSpan.ParseExact(str, tsFMTMS, SettingsCultureInfo); }
            catch (Exception)
            {
                try { ts = TimeSpan.ParseExact(str, tsFMT, SettingsCultureInfo); }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} \"{1}\"", ex.Message, str));
                }
            }

            return ts;
        }

        /// <summary>
        /// TODO: test it
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToXMLString(this double val)
        {
            return val.ToString("0.###");
        }

        /// <summary>
        /// TODO: test it
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToXMLDouble(this string str)
        {
            // NumberFormatInfo format = new NumberFormatInfo();
            // format.NumberDecimalSeparator = ".";
            double vlue = double.Parse(str.Replace(",", "."), CultureInfo.InvariantCulture);

            return vlue;
        }

        // /// <summary>
        // /// Управление соединением с БД
        // /// </summary>
        // /// <param name="conn">Соединение</param>
        // /// <param name="open">Открыть или закрыть</param>
        // public static void VerifyConn(SqlConnection conn, bool open = true)
        // {
        //     if (open)
        //     {
        //         conn.Open();
        //         return;
        //     }
        //
        //     if (conn.State == System.Data.ConnectionState.Open) conn.Close();
        // }
    }
}
