// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace NetworkSelector
{
    public sealed partial class SettingsPage : Page
    {
        // 启用本地设置数据
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        // 材料ComboBox列表List
        public List<string> material { get; } = new List<string>()
        {
            "Mica",
            "Acrylic"
        };

        public List<string> CMDDisplays { get; } = new List<string>()
        {
            "是",
            "否"
        };

        // 页面初始化
        public SettingsPage()
        {
            // 初始化
            this.InitializeComponent();

            //backgroundMaterial.PlaceholderText = localSettings.Values["materialStatus"] as string;
            // 读取本地设置数据，调整ComboBox状态
            if (localSettings.Values["materialStatus"] as string == "Mica")
            {
                backgroundMaterial.SelectedItem = material[0];
            }
            else if (localSettings.Values["materialStatus"] as string == "Acrylic")
            {
                backgroundMaterial.SelectedItem = material[1];
            }
            else
            {
                // 非法输入，设置默认材料为Mica
                localSettings.Values["materialStatus"] = "Mica";
                backgroundMaterial.SelectedItem = material[0];
                // 非法输入，扔出警报
                //throw new Exception($"Wrong material type: {localSettings.Values["materialStatus"]}");
            }

            if (localSettings.Values["CMDDisplay"] as string == "是")
            {
                CMDDisplay.SelectedItem = CMDDisplays[0];
            }
            else if (localSettings.Values["CMDDisplay"] as string == "否")
            {
                CMDDisplay.SelectedItem = CMDDisplays[1];
            }
            else
            {
                localSettings.Values["CMDDisplay"] = "否";
                CMDDisplay.SelectedItem = CMDDisplays[1];
            }
        }

        // 背景材料设置ComboBox改动事件
        private void backgroundMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string materialStatus = e.AddedItems[0].ToString();
            switch (materialStatus)
            {
                case "Mica":
                    if (localSettings.Values["materialStatus"] as string != "Mica")
                    {
                        localSettings.Values["materialStatus"] = "Mica";
                        Microsoft.Windows.AppLifecycle.AppInstance.Restart("");
                    }
                    else
                    {
                        localSettings.Values["materialStatus"] = "Mica";
                    }
                    break;
                case "Acrylic":
                    if (localSettings.Values["materialStatus"] as string != "Acrylic")
                    {
                        localSettings.Values["materialStatus"] = "Acrylic";
                        Microsoft.Windows.AppLifecycle.AppInstance.Restart("");
                    }
                    else
                    {
                        localSettings.Values["materialStatus"] = "Acrylic";
                    }
                    break;
                default:
                    throw new Exception($"Invalid argument: {materialStatus}");
            }
        }

        private void CMDDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string CMDDisplay = e.AddedItems[0].ToString();
            switch (CMDDisplay)
            {
                case "是":
                    if (localSettings.Values["CMDDisplay"] as string != "是")
                    {
                        localSettings.Values["CMDDisplay"] = "是";
                    }
                    else
                    {
                        localSettings.Values["CMDDisplay"] = "是";
                    }
                    break;
                case "否":
                    if (localSettings.Values["CMDDisplay"] as string != "否")
                    {
                        localSettings.Values["CMDDisplay"] = "否";
                    }
                    else
                    {
                        localSettings.Values["CMDDisplay"] = "否";
                    }
                    break;
            }
        }
    }
}
