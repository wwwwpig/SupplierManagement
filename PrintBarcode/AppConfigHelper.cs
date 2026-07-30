using System;
using System.Configuration;

public static class AppConfigHelper
{
    /// <summary>
    /// 读取配置文件中的值（支持强类型）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="key">配置键名</param>
    /// <param name="defaultValue">默认值（如果读取失败或不存在）</param>
    /// <returns>转换后的配置值</returns>
    public static T GetValue<T>(string key, T defaultValue = default)
    {
        try
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            // 处理特殊类型
            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(value, out bool boolResult))
                    return (T)(object)boolResult;
                // 支持 0/1 转换为 bool
                if (int.TryParse(value, out int intResult))
                    return (T)(object)(intResult != 0);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// 写入配置值到配置文件
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="key">配置键名</param>
    /// <param name="value">要写入的值</param>
    /// <returns>成功返回true，失败返回false</returns>
    public static bool SetValue<T>(string key, T value)
    {
        try
        {
            // 打开配置文件
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // 先删除旧的配置项（如果存在）
            if (config.AppSettings.Settings[key] != null)
            {
                config.AppSettings.Settings.Remove(key);
            }

            // 添加新的配置项
            config.AppSettings.Settings.Add(key, value?.ToString());

            // 保存配置文件
            config.Save(ConfigurationSaveMode.Modified);

            // 刷新配置节，使修改立即生效
            ConfigurationManager.RefreshSection("appSettings");

            return true;
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show($"写入配置失败: {ex.Message}", "错误",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            return false;
        }
    }
}
