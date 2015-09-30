using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Resources;
using MoreLinq;

namespace UriInternal
{
    public class DlgResourceModel : ModelBase
    {
        public DlgResourceModel()
        {
            Resources = new ObservableCollection<string>();
        }

        private Assembly _assembly;
        private string _selectedItem;
        private string _uri;
        private string _resLoadResult;
        private string _path;
        private string _fullName;
        private string _resLoadResultFromApplication;
        private string _resLoadResultFromGeneralFuncWithUri;

        private const string ResPrefix = "    ";

        public ObservableCollection<String> Resources { get; private set; }

        public string ResLoadResult
        {
            get { return _resLoadResult; }
            private set
            {
                if (value == _resLoadResult) return;
                _resLoadResult = value;
                OnPropertyChanged();
            }
        }

        public string ResLoadResultFromApplication
        {
            get { return _resLoadResultFromApplication; }
            private set
            {
                if (value == _resLoadResultFromApplication) return;
                _resLoadResultFromApplication = value;
                OnPropertyChanged();
            }
        }

        public string ResLoadResultFromGeneralFuncWithUri
        {
            get { return _resLoadResultFromGeneralFuncWithUri; }
            private set
            {
                if (value == _resLoadResultFromGeneralFuncWithUri) return;
                _resLoadResultFromGeneralFuncWithUri = value;
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
                OnPropertyChanged(()=>Title);
            }
        }

        public string FullName
        {
            get { return _fullName; }
            private set
            {
                if (value == _fullName) return;
                _fullName = value;
                OnPropertyChanged();
                OnPropertyChanged(()=>Title);
            }
        }

        public string Title
        {
            get { return FullName + " (" + Path + ")"; }
        }

        public Assembly Assembly
        {
            get { return _assembly; }
            set
            {
                if (Equals(value, _assembly)) return;
                _assembly = value;

                FullName = _assembly.FullName;
                Path = _assembly.Location;

                string[] names = _assembly.GetManifestResourceNames();

                foreach (string resName in names)
                {
                    Resources.Add(resName);

                    using (var stream = _assembly.GetManifestResourceStream(resName))
                    {
                        using (var reader = new System.Resources.ResourceReader(stream))
                        {
                            SortedSet<string> list = new SortedSet<string>();

                            foreach (DictionaryEntry entry in reader)
                            {
                                list.Add(ResPrefix + entry.Key.ToString());
                            }

                            list.ForEach(item=>Resources.Add(item));
                        }
                    }
                }

                OnPropertyChanged();
            }
        }

        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value == _selectedItem) return;
                _selectedItem = value;

                if (_selectedItem.StartsWith(ResPrefix))
                {
                    string resPath = _selectedItem.Trim();
                    Uri uri = ResourceHelper.GetLocationUri(resPath, Assembly);

                    try
                    {
                        ResLoadResult = "Unable to load resource";
                        
                        string[] names = Assembly.GetManifestResourceNames();
                        foreach (string name in names)
                        {
                            // Can use that instead: string resName = asm.GetName().Name + ".g.resources";
                            Stream stream = Assembly.GetManifestResourceStream(names[0]);
                            ResourceReader reader = new ResourceReader(stream);
                            string resType;
                            byte[] resData;
                            reader.GetResourceData(resPath, out resType, out resData);

                            if (resData != null)
                            {
                                ResLoadResult = string.Format("Success! Type: [{0}], Number of byte: [{1}]", resType, resData.Length);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ResLoadResult = "Exception while trying to load resource data: " + ex.ToString();
                    }

                    try
                    {
                        StreamResourceInfo streamResourceInfoInfo = System.Windows.Application.GetResourceStream(uri);
                        if (streamResourceInfoInfo != null)
                        {
                            ResLoadResultFromApplication = "Success";
                        }
                        else
                        {
                            ResLoadResultFromApplication = "Error";
                        }
                    }
                    catch (Exception ex)
                    {
                        ResLoadResultFromApplication = "Exception: " + ex.ToString();
                    }

                    try
                    {
                        Stream stream = ResourceHelper.LoadResourceFromUri(uri); 
                        if (stream != null)
                        {
                            ResLoadResultFromGeneralFuncWithUri = "Success";
                        }
                        else
                        {
                            ResLoadResultFromGeneralFuncWithUri = "Error";
                        }
                    }
                    catch (Exception ex)
                    {
                        ResLoadResultFromGeneralFuncWithUri = "Exception: " + ex.ToString();
                    }



                    Uri = uri.ToString();
                }

                OnPropertyChanged();
            }
        }

        public string Uri
        {
            get { return _uri; }
            set
            {
                if (value == _uri) return;
                _uri = value;
                OnPropertyChanged();
            }
        }
    }
}
