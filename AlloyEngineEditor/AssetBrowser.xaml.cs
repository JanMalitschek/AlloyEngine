using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Alloy;
using Alloy.Assets;
using AlloyEditorInterface.Contracts;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AlloyEngineEditor
{
    /// <summary>
    /// Interaction logic for AssetBrowser.xaml
    /// </summary>
    public partial class AssetBrowser : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Tuple<string, string, int>> assets { get; private set; } = new ObservableCollection<Tuple<string, string, int>>();

        int currentAssetID = -1;
        private bool notifyChanges = true;
        #region Texture
        private TextureContract currentTextureValue = null;
        public TextureContract CurrentTexture
        {
            get
            {
                return currentTextureValue;
            }
            set
            {
                currentTextureValue = value;
                NotifyPropertyChange();
            }
        }
        public string[] FilterValues { get; set; }
        public string[] WrappingValues { get; set; }
        #endregion
        #region Shader
        private ShaderContract currentShaderValue = null;
        public ShaderContract CurrentShader
        {
            get
            {
                return currentShaderValue;
            }
            set
            {
                currentShaderValue = value;
                NotifyPropertyChange();
            }
        }
        #endregion

        public AssetBrowser()
        {
            var db = MainWindow.service.GetAssetDatabase();
            foreach (var a in db)
                assets.Add(new Tuple<string, string, int>(a.Item1, a.Item2, a.Item3));

            FilterValues = Enum.GetNames(typeof(Texture.Filter));
            WrappingValues = Enum.GetNames(typeof(Texture.Wrapping));

            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        void Import(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Import");
        }

        void Select(object sender, MouseButtonEventArgs e)
        {
            notifyChanges = false;
            var source = e.Source as FrameworkElement;
            var data = source.DataContext as Tuple<string, string, int>;
            currentAssetID = data.Item3;

            CurrentTexture = null;
            CurrentShader = null;
            if (data.Item2 == "Texture2D")
                CurrentTexture = MainWindow.service.GetTexture(data.Item3);
            else if(data.Item2 == "Shader")
                CurrentShader = MainWindow.service.GetShader(data.Item3);
            notifyChanges = true;
        }

        #region Texture Settings
        void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if(notifyChanges)
                MainWindow.service.ChangeTextureFilter(currentAssetID, (Texture.Filter)filterCmb.SelectedIndex);
        }
        void WrappingChanged(object sender, SelectionChangedEventArgs e)
        {
            if(notifyChanges)
                MainWindow.service.ChangeTextureWrapping(currentAssetID, (Texture.Wrapping)wrappingCmb.SelectedIndex);
        }
        #endregion
    }
}
