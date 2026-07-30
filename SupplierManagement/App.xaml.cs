using System.Configuration;
using System.Data;
using System.Windows;
using WebFirst.Services;

namespace SupplierManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // 1) 在任何 WPF 框架创建窗口或加载 XAML 资源之前先初始化数据库连接
            var conn = ConfigurationManager.ConnectionStrings["conn"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(conn))
            {
                MessageBox.Show("未找到连接字符串 'conn'，请检查 App.config 并确保已添加 System.Configuration.ConfigurationManager 包。",
                    "配置错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
                return;
            }

            try
            {
                Repository<object>.Configure(conn);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化数据库连接失败：{ex.Message}",
                    "初始化错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
                return;
            }

            // 2) 在完成初始化后再让 WPF 继续默认启动流程（创建窗口等）
            base.OnStartup(e);
        }
    }

}
