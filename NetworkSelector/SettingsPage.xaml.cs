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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NetworkSelector
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        // ���ñ�����������
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        // ����ComboBox�б�List
        public List<string> material { get; } = new List<string>()
        {
            "Mica",
            "Acrylic"
        };

        // ҳ���ʼ��
        public SettingsPage()
        {
            // ��ʼ��
            this.InitializeComponent();

            //backgroundMaterial.PlaceholderText = localSettings.Values["materialStatus"] as string;
            // ��ȡ�����������ݣ�����ComboBox״̬
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
                // �Ƿ����룬����Ĭ�ϲ���ΪMica
                localSettings.Values["materialStatus"] = "Mica";
                backgroundMaterial.SelectedItem = material[0];
                // �Ƿ����룬�ӳ�����
                //throw new Exception($"Wrong material type: {localSettings.Values["materialStatus"]}");
            }
        }

        // ������������ComboBox�Ķ��¼�
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
    }
}
