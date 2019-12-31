using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChroniCalc
{
    class PasteBinClient
    {
        private const string _apiPostUrl = "https://pastebin.com/api/api_post.php";
        private const string _apiLoginUrl = "https://pastebin.com/api/api_login.php";
        private const string _apiRawUrl = "https://pastebin.com/raw/";

        private readonly string _apiDevKey;
        private string _userName;
        private string _apiUserKey; // = "198f4c8f747865b8c24f6289f2b4a27c";

        static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";

        private readonly string[] DevKeysBuildShare =
        {
            "9074d08a3c19871f793663a0361c6976",
            "ae626aa58df38f743f69d73ae20055a6",
            "066efa650ae5d795cb698c5dbeb69a01",
            "5effc482dd5a3518e4bba2c7619f4c4b",
            "34e63562a182235bc302665f28508ba9"
        };

        private readonly string[] DevKeysError =
        {
            "b927c689c6948e55594cd751f1e22a33",
            "e2fbe4e244169b76bfb4e4945a692544",
            "13cac883e311b4d9cad23a8f595a4460",
            "3332417fd39373a79e15794d4de0aaf0",
            "2b0643b10902af85ba9e004ddf9ae0ea"
        };

        public enum PBType
        {
            BuildShare,
            Error
        }

        public PasteBinClient(PBType pBType)
        {
            _apiDevKey = GetDevKey(pBType);
        }

        private string GetDevKey(PBType pBType)
        {
            string devKey = string.Empty;
            Random random = new Random();

            switch (pBType)
            {
                case PBType.BuildShare:
                    devKey = DevKeysBuildShare[random.Next(DevKeysBuildShare.Length)];
                    break;
                case PBType.Error:
                    devKey = DevKeysError[random.Next(DevKeysError.Length)];
                    break;
                default:
                    break;
            }

            return devKey;
        }

        public string UserName
        {
            get { return _userName; }
        }

        public void Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            var parameters = GetBaseParameters();
            parameters[ApiParameters.UserName] = userName;
            parameters[ApiParameters.UserPassword] = password;

            WebClient client = new WebClient();
            byte[] bytes = client.UploadValues(_apiLoginUrl, parameters);
            string resp = GetResponseText(bytes);
            if (resp.StartsWith("Bad API request"))
                throw new PasteBinApiException(resp);

            _userName = userName;
            _apiUserKey = resp;
        }

        public void Logout()
        {
            _userName = null;
            _apiUserKey = null;
        }

        public string Paste(PasteBinEntry entry, bool encodeData = true)
        {
            byte[] pastebinRawBytes;
            string data = string.Empty;

            if (entry == null)
                throw new ArgumentNullException("entry");
            if (string.IsNullOrEmpty(entry.Text))
                throw new ArgumentException("The paste text must be set", "entry");
            if (string.IsNullOrEmpty(_apiDevKey))
                throw new ArgumentNullException("apiDevKey");

            if (encodeData)
            {
                // Compress and Encode the Build data so it meets Pastebin's file limit
                pastebinRawBytes = Zip(entry.Text);
                data = Convert.ToBase64String(pastebinRawBytes);
            }
            else
            {
                // Don't Compress and Encode the data (ie. it may be an error message and not Build data)
                data = entry.Text;
            }

            // Setup all parameters for the Pastebin call to create the Paste
            var parameters = GetBaseParameters();
            parameters[ApiParameters.Option] = "paste";
            parameters[ApiParameters.PasteCode] = data;
            SetIfNotEmpty(parameters, ApiParameters.PasteName, entry.Title);
            SetIfNotEmpty(parameters, ApiParameters.PasteFormat, entry.Format);
            SetIfNotEmpty(parameters, ApiParameters.PastePrivate, entry.Private ? "1" : "0");
            SetIfNotEmpty(parameters, ApiParameters.PasteExpireDate, FormatExpireDate(entry.Expiration));
            SetIfNotEmpty(parameters, ApiParameters.UserKey, _apiUserKey);

            // Upload the data to Pastebin
            WebClient client = new WebClient();
            byte[] bytes = client.UploadValues(_apiPostUrl, parameters);
            string resp = GetResponseText(bytes);
            if (resp.StartsWith("Bad API request"))
                throw new PasteBinApiException(resp);
            return resp;
        }

        public string Extract(string pastebinURL)
        {
            byte[] pastebinRawBytes;
            string decodedData = string.Empty; ;
            string pastebinRawURL;
            string pastebinRawText;

            if (string.IsNullOrEmpty(pastebinURL))
                throw new ArgumentException("The pastebin URL must be specified", "pastebinURL");

            // Setup the Pastebin Raw URL to pull the data from
            pastebinRawURL = pastebinURL.Replace("pastebin.com/", "pastebin.com/raw/");

            // Retrieve the data from the URL
            WebClient client = new WebClient();
            pastebinRawText = client.DownloadString(pastebinRawURL);

            // Decode and Uncompress the data back into its XML
            pastebinRawBytes = Convert.FromBase64String(pastebinRawText);
            decodedData = Unzip(pastebinRawBytes);

            // Send back the Build data as XML in string format
            return decodedData;
        }

        private static string FormatExpireDate(PasteBinExpiration expiration)
        {
            switch (expiration)
            {
                case PasteBinExpiration.Never:
                    return "N";
                case PasteBinExpiration.TenMinutes:
                    return "10M";
                case PasteBinExpiration.OneHour:
                    return "1H";
                case PasteBinExpiration.OneDay:
                    return "1D";
                case PasteBinExpiration.OneMonth:
                    return "1M";
                default:
                    throw new ArgumentException("Invalid expiration date");
            }
        }

        private static void SetIfNotEmpty(NameValueCollection parameters, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
                parameters[name] = value;
        }

        private NameValueCollection GetBaseParameters()
        {
            var parameters = new NameValueCollection();
            parameters[ApiParameters.DevKey] = _apiDevKey;

            return parameters;
        }

        private static string GetResponseText(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            using (var reader = new StreamReader(ms))
            {
                return reader.ReadToEnd();
            }
        }

        private static class ApiParameters
        {
            public const string DevKey = "api_dev_key";
            public const string UserKey = "api_user_key";
            public const string Option = "api_option";
            public const string UserName = "api_user_name";
            public const string UserPassword = "api_user_password";
            public const string PasteCode = "api_paste_code";
            public const string PasteName = "api_paste_name";
            public const string PastePrivate = "api_paste_private";
            public const string PasteFormat = "api_paste_format";
            public const string PasteExpireDate = "api_paste_expire_date";
        }

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
    }

    public class PasteBinApiException : Exception
    {
        public PasteBinApiException(string message)
            : base(message)
        {
        }
    }

    public class PasteBinEntry
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Format { get; set; }
        public bool Private { get; set; }
        public PasteBinExpiration Expiration { get; set; }
    }

    public enum PasteBinExpiration
    {
        Never,
        TenMinutes,
        OneHour,
        OneDay,
        OneMonth
    }
}
