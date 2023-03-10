// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using WinRT;
using static System.Net.Mime.MediaTypeNames;

namespace NetworkSelector
{
    public sealed partial class NetSetting : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        // 页面初始化
        public NetSetting()
        {
            this.InitializeComponent();

            if (localSettings.Values["configName"] == null)
            {
                configName.SelectedItem = ConfigSelector[0];
                localSettings.Values["configName"] = ConfigSelector[0];
                refreshContent("自动");
            }
            else
            {
                configName.SelectedItem = localSettings.Values["configName"];
                refreshContent(localSettings.Values["configName"].ToString());
            }

            netName.Text = localSettings.Values[localSettings.Values["configName"].ToString() + "netName"] as string;
            refreshStatus();
        }
        public List<string> ConfigSelector { get; } = new List<string>()
        {
            "自动",
            "预设1",
            "预设2",
            "预设3",
            "预设4",
            "预设5",
            "预设6",
            "预设7",
            "预设8",
            "预设9",
            "预设10"
        };
        // 应用保存的设置
        public void applyConfig()
        {
            Process process = new Process();
            process.StartInfo.FileName = "PowerShell.exe";
            process.StartInfo.Arguments = localSettings.Values["netshCMD"] as string;
            //是否使用操作系统shell启动
            process.StartInfo.UseShellExecute = false;
            //是否在新窗口中启动该进程的值 (不显示程序窗口)
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            process.Close();
        }
        private void refreshCMD(string IPAddr, string Mask, string Gateway, string DNS1, string DNS2)
        {
            // 根据选择自动或手动，写入不同的netshCMD
            if ((configName.SelectedItem as string) == "自动")
            {
                localSettings.Values["netshCMD"] =
                    "netsh interface ip set address '" + netName.Text + "' dhcp;"
                    + "netsh interface ip set dns name='" + netName.Text + "' source=dhcp;";
            }
            else
            {
                localSettings.Values["netshCMD"] =
                "netsh interface ip set address name='" + netName.Text + "' source=static addr='" + IPAddr + "' mask='" + Mask + "' gateway='" + Gateway + "'; "
                + "netsh interface ip set dns name='" + netName.Text + "' source=static addr='" + DNS1 + "' register=primary;"
                + "netsh interface ip add dns name='" + netName.Text + "' addr='" + DNS2 + "' index=2;";
            }
            netshCMD.Text = localSettings.Values["netshCMD"] as string;
        }
        // 刷新Content 调取localSettings存储内容
        public void refreshContent(string ConfigNameStr)
        {
            addConfigButton.IsEnabled = true;
            List<Item> items = new List<Item>();
            if (ConfigNameStr != "自动")
            {
                string configInner = localSettings.Values["ConfigID" + ConfigNameStr] as string;
                if (configInner != null)
                {
                    string[] configInnerSplit = configInner.Split(',');
                    // IPAddr.Text + "," + mask.Text + "," + gateway.Text + "," + DNS1.Text + "," + DNS2.Text;
                    string IPAddr = configInnerSplit[0];
                    string Mask = configInnerSplit[1];
                    string Gateway = configInnerSplit[2];
                    string DNS1 = configInnerSplit[3];
                    string DNS2 = configInnerSplit[4];

                    items.Add(new Item(
                        "IP 地址：" + IPAddr,
                        "子网掩码：" + Mask,
                        "网关：" + Gateway,
                        "首选 DNS：" + DNS1,
                        "次选 DNS：" + DNS2
                        ));
                    addConfigButton.Content = "修改配置";
                    refreshCMD(IPAddr, Mask, Gateway, DNS1, DNS2);
                }
                else
                {
                    items.Add(new Item(
                        "IP 地址：",
                        "子网掩码：",
                        "网关：",
                        "首选 DNS：",
                        "次选 DNS："
                        ));
                    addConfigButton.Content = "添加配置";
                    netshCMD.Text = "";
                }
            }
            else
            {
                items.Add(new Item(
                    "IP 地址：DHCP",
                    "子网掩码：DHCP",
                    "网关：DHCP",
                    "首选 DNS：DHCP",
                    "次选 DNS：DHCP"
                    ));
                addConfigButton.Content = "添加配置";
                addConfigButton.IsEnabled = false;
                refreshCMD("DHCP", "DHCP", "DHCP", "DHCP", "DHCP");
            }
            MyGridView.ItemsSource = items;
        }
        // 刷新Status 调用localSettings存储状态
        public void refreshStatus()
        {
            if (localSettings.Values["CMDDisplay"] as string == "是")
            {
                netshCMD.Visibility = Visibility.Visible;
            }
            else if (localSettings.Values["CMDDisplay"] as string == "否")
            {
                netshCMD.Visibility = Visibility.Collapsed;
            }
            else
            {
                localSettings.Values["CMDDisplay"] = "否";
                netshCMD.Visibility = Visibility.Collapsed;
            }
        }
        // 自动或预设切换
        private void configName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refreshContent(configName.SelectedItem.ToString());
            refreshStatus();
        }
        private async void addConfigButton_Click(object sender, RoutedEventArgs e)
        {
            AddConfigDialog configDialog = new AddConfigDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            configDialog.XamlRoot = this.XamlRoot;
            configDialog.Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            //configDialog.Title = "添加一个 IP 测试";
            configDialog.PrimaryButtonText = "添加";
            configDialog.CloseButtonText = "关闭";
            configDialog.DefaultButton = ContentDialogButton.Primary;
            //dialog.Content = new AddPingDialog();

            var result = await configDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string ConfigNameStr = configName.SelectedItem.ToString();
                localSettings.Values["ConfigID" + ConfigNameStr] = localSettings.Values["ConfigIDTemp"];
                refreshContent(ConfigNameStr);
            }
        }
        private void applyConfigButton_Click(object sender, RoutedEventArgs e)
        {
            applyConfig();
            localSettings.Values["configName"] = configName.SelectedItem;
        }
        public void TextChanged(object sender, TextChangedEventArgs e)
        {
            localSettings.Values[configName.SelectedItem.ToString() + "netName"] = netName.Text;
            refreshContent(configName.SelectedItem.ToString());
        }
    }
    public class Item
    {
        // IP地址
        public string IPAddr { get; set; }
        // 子网掩码
        public string Mask { get; set; }
        // 网关
        public string Gateway { get; set; }
        // DNS
        public string DNS1 { get; set; }
        public string DNS2 { get; set; }
        public Item(string ipAddr, string mask, string gateway, string dns1, string dns2)
        {
            IPAddr = ipAddr;
            Mask = mask;
            Gateway = gateway;
            DNS1 = dns1;
            DNS2 = dns2;
        }
    }
}
