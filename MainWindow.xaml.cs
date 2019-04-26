using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp11
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        /// <summary>
        /// プロパティの変更を通知するためのマルチキャスト イベント。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティが既に目的の値と一致しているかどうかを確認します。必要な場合のみ、
        /// プロパティを設定し、リスナーに通知します。
        /// </summary>
        /// <typeparam name="T">プロパティの型。</typeparam>
        /// <param name="storage">get アクセス操作子と set アクセス操作子両方を使用したプロパティへの参照。</param>
        /// <param name="value">プロパティに必要な値。</param>
        /// <param name="propertyName">リスナーに通知するために使用するプロパティの名前。
        /// この値は省略可能で、
        /// CallerMemberName をサポートするコンパイラから呼び出す場合に自動的に指定できます。</param>
        /// <returns>値が変更された場合は true、既存の値が目的の値に一致した場合は
        /// false です。</returns>
        protected bool SetProperty<T>(ref T storage, T value, bool isDiposeOld = false, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            if (isDiposeOld)
            {
                // 入替前にDisposeできるものはする。
                var disposable = storage as IDisposable; if (disposable != null) disposable.Dispose();
            }

            // ﾌﾟﾛﾊﾟﾃｨ値変更
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// プロパティ値が変更されたことをリスナーに通知します。
        /// </summary>
        /// <param name="propertyName">リスナーに通知するために使用するプロパティの名前。
        /// この値は省略可能で、
        /// <see cref="CallerMemberNameAttribute"/> をサポートするコンパイラから呼び出す場合に自動的に指定できます。</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Dictionary<string, string>> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private ObservableCollection<Dictionary<string, string>> _Items = new ObservableCollection<Dictionary<string, string>>();

        public ObservableCollection<string> Headers
        {
            get { return _Headers; }
            set { SetProperty(ref _Headers, value); }
        }
        private ObservableCollection<string> _Headers = new ObservableCollection<string>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var random = new Random();
            var row = random.Next(1, 100);
            var col = random.Next(1, 100);

            Headers.Clear();
            Enumerable.Range(1, col).ToList().ForEach(i => Headers.Add($"C{i}"));

            Items.Clear();
            foreach (var r in Enumerable.Range(1, row))
            {
                Items.Add(Headers.ToDictionary(s => s, s => $"{s}-R{r}"));
            }
        }
    }
}
