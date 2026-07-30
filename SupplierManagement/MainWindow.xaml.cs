using SupplierManagement.Common;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WebFirst.Services;

namespace SupplierManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private Button currentButton;

        UserManager userManager = new UserManager();
        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();

            // 默认选中首页并加载首页内容
            //btnDesign.Background = new SolidColorBrush(Color.FromRgb(0, 122, 204));
            //currentButton = btnDesign;
            //LoadContent("Design");
        }
        // 初始化时间更新定时器
        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeText.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // 导航按钮点击事件
        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            // 重置之前选中按钮的背景色
            if (currentButton != null)
            {
                currentButton.Background = Brushes.Transparent;
            }

            // 设置当前按钮的背景色
            clickedButton.Background = new SolidColorBrush(Color.FromRgb(0, 122, 204));
            currentButton = clickedButton;

            // 根据Tag加载不同内容
            string tag = clickedButton.Tag as string;
            LoadContent(tag);

            // 更新状态栏
            statusText.Text = $"当前页面: {clickedButton.Content.ToString().Substring(2)}";
        }

        // 加载不同的UserControl页面
        private void LoadContent(string pageTag)
        {
            UserControl page = null;

            try
            {
                switch (pageTag)
                {
                    case "Design":
                    //方式1: 直接实例化UserControl（推荐）
                        page = new Views.DesignUserControl();
                        break;
                    case "Buyer":
                        page = new Views.BuyUserControl();
                        break;
                    case "Quality":
                        page = new Views.QualityInspectorUserControl();
                        break;
                    case "InWarehouse":
                        page = new Views.InWarehouseUserControl();
                        break;
                    case "OutWarehouse":
                        page = new Views.OutWarehouseUserControl();
                        break;
                    case "Query":
                        page = new Views.QueryUserControl();
                        break;
                    case "User":
                        page = new Views.UserUserControl();
                        break;
                    case "UserRole":
                        page = new Views.UserRoleUserControl();
                        break;
                    case "RolePermission":
                        page = new Views.RolePermissionUserControl();
                        break;
                    case "UpdatePwd":
                        Views.UpdatePwdWindow upw = new Views.UpdatePwdWindow();
                        upw.ShowDialog();
                        break;
                    default:
                        // 如果页面不存在，显示默认内容
                        ShowDefaultContent($"页面 {pageTag} 正在开发中...");
                        return;
                }

                // 将UserControl加载到Frame中
                if (page != null)
                {
                    contentFrame.Content = page;
                }
            }
            catch (Exception ex)
            {
                // 如果UserControl还未创建，显示提示信息
                ShowDefaultContent($"页面加载失败\n\n{ex.Message}\n\n请创建对应的UserControl文件");
            }
        }

        // 显示默认内容（当UserControl不存在时）
        private void ShowDefaultContent(string message)
        {
            Grid contentGrid = new Grid();
            contentGrid.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));

            TextBlock contentText = new TextBlock
            {
                Text = message,
                FontSize = 20,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            contentGrid.Children.Add(contentText);
            contentFrame.Content = contentGrid;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

        /// <summary>
        /// 在状态栏上显示文本（线程安全）。
        /// </summary>
        public void SetStatus(string text)
        {
            if (Dispatcher == null || Dispatcher.HasShutdownStarted || Dispatcher.HasShutdownFinished)
            {
                return;
            }

            if (Dispatcher.CheckAccess())
            {
                statusText.Text = text;
            }
            else
            {
                Dispatcher.Invoke(() => statusText.Text = text, DispatcherPriority.Normal);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                //关闭应用程序
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> permissions = userManager.GetPermissionsByUserID(Utils.user.Id);
                foreach (var permission in permissions)
                {
                    switch (permission)
                    {
                        case "设计管理":
                            btnDesign.Visibility = Visibility.Visible;
                            break;
                        case "采购管理":
                            btnBuy.Visibility = Visibility.Visible;
                            break;
                        case "质检管理":
                            btnQuality.Visibility = Visibility.Visible;
                            break;
                        case "入库管理":
                            btnInWarehouse.Visibility = Visibility.Visible;
                            break;
                        case "出库管理":
                            btnOutWarehouse.Visibility = Visibility.Visible;
                            break;
                        case "查询":
                            btnQuery.Visibility = Visibility.Visible;
                            break;
                        case "用户管理":
                            btnUser.Visibility = Visibility.Visible;
                            break;
                        case "用户角色管理":
                            btnUserRole.Visibility = Visibility.Visible;
                            break;
                        case "角色权限管理":
                            btnRolePermission.Visibility = Visibility.Visible;
                            break;
                        case "修改密码":
                            btnUpdatePwd.Visibility = Visibility.Visible;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                Log.Error(ex);
            }
        }
    }
}