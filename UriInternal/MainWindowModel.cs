using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UriInternal
{
    public class MainWindowModel : ModelBase
    {
        private Uri _uri;
        private string _absoluteUri;
        private string _fragment;
        private string _host;
        private string _hostAndPort;
        private string _httpRequestUrl;
        private string _keepDelimiter;
        private string _normalizedHost;
        private string _path;
        private string _scheme;
        private string _userInfo;
        private string _input;
        private string _segments;
        private string _authority;
        private string _loadResult;

        public string Input
        {
            get { return _input; }
            set
            {
                if (value == _input) return;

                _input = value.Replace(@"\\",@"\");
                
                Uri = new Uri(_input);
                OnPropertyChanged();
            }
        }
        
        private Uri Uri
        {
            get { return _uri; }
            set
            {
                if (Equals(value, _uri)) return;
                _uri = value;
                OnPropertyChanged();
            }
        }

        public string AbsoluteUri
        {
            get { return _absoluteUri; }
            private set
            {
                if (value == _absoluteUri) return;
                _absoluteUri = value;
                OnPropertyChanged();
            }
        }

        public string Fragment
        {
            get { return _fragment; }
            private set
            {
                if (value == _fragment) return;
                _fragment = value;
                OnPropertyChanged();
            }
        }

        public string Host
        {
            get { return _host; }
            private set
            {
                if (value == _host) return;
                _host = value;
                OnPropertyChanged();
            }
        }

        public string HostAndPort
        {
            get { return _hostAndPort; }
            private set
            {
                if (value == _hostAndPort) return;
                _hostAndPort = value;
                OnPropertyChanged();
            }
        }

        public string HttpRequestUrl
        {
            get { return _httpRequestUrl; }
            private set
            {
                if (value == _httpRequestUrl) return;
                _httpRequestUrl = value;
                OnPropertyChanged();
            }
        }

        public string KeepDelimiter
        {
            get { return _keepDelimiter; }
            private set
            {
                if (value == _keepDelimiter) return;
                _keepDelimiter = value;
                OnPropertyChanged();
            }
        }

        public string NormalizedHost
        {
            get { return _normalizedHost; }
            private set
            {
                if (value == _normalizedHost) return;
                _normalizedHost = value;
                OnPropertyChanged();
            }
        }

        public string Path
        {
            get { return _path; }
            private set
            {
                if (value == _path) return;
                _path = value;
                OnPropertyChanged();
            }
        }

        public string Scheme
        {
            get { return _scheme; }
            private set
            {
                if (value == _scheme) return;
                _scheme = value;
                OnPropertyChanged();
            }
        }

        public string UserInfo
        {
            get { return _userInfo; }
            private set
            {
                if (value == _userInfo) return;
                _userInfo = value;
                OnPropertyChanged();
            }
        }

        public string Segments
        {
            get { return _segments; }
            set
            {
                if (value == _segments) return;
                _segments = value;
                OnPropertyChanged();
            }
        }

        public string Authority
        {
            get { return _authority; }
            private set
            {
                if (value == _authority) return;
                _authority = value;
                OnPropertyChanged();
            }
        }

        public string LoadResult
        {
            get { return _loadResult; }
            private set
            {
                if (value == _loadResult) return;
                _loadResult = value;
                OnPropertyChanged();
            }
        }

        public void RefreshFields()
        {
            if (Uri == null || ! Uri.IsWellFormedOriginalString())
            {
                return;
            }

            AbsoluteUri = Uri.GetComponents(UriComponents.AbsoluteUri, UriFormat.SafeUnescaped);
            Authority = Uri.Authority;
            Fragment = Uri.GetComponents(UriComponents.Fragment, UriFormat.SafeUnescaped);
            Host = Uri.GetComponents(UriComponents.Host, UriFormat.SafeUnescaped);
            HostAndPort = Uri.GetComponents(UriComponents.HostAndPort, UriFormat.SafeUnescaped);
            HttpRequestUrl = Uri.GetComponents(UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped);
            KeepDelimiter = Uri.GetComponents(UriComponents.KeepDelimiter, UriFormat.SafeUnescaped);
            NormalizedHost = Uri.GetComponents(UriComponents.NormalizedHost, UriFormat.SafeUnescaped);
            Path = Uri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped);
            Scheme = Uri.GetComponents(UriComponents.Scheme, UriFormat.SafeUnescaped);
            UserInfo = Uri.GetComponents(UriComponents.UserInfo, UriFormat.SafeUnescaped);

            StringBuilder sb = new StringBuilder();
            bool first = true;
            Array.ForEach(Uri.Segments, part =>
            {
                if (first)
                {
                    sb.Append(part);
                    first = false;
                }
                else
                {
                    sb.Append(" || ");
                    sb.Append(part);
                }
            });
            Segments = sb.ToString();

            try
            {
                Application.LoadComponent(Uri);
                LoadResult = "Success !";
            }
            catch (Exception ex)
            {
                LoadResult = "Exception: " + ex.ToString();
            }
        }
    }
}
