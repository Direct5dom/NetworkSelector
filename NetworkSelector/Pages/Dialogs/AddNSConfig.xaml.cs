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
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using NetworkSelector.Models;
using System.Xml.Linq;

namespace NetworkSelector.Pages.Dialogs
{
    public sealed partial class AddNSConfig : ContentDialog
    {
        public NSModel NSData { get; private set; }
        public AddNSConfig(NSModel nsModel)
        {
            this.InitializeComponent();
            PrimaryButtonClick += MyDialog_PrimaryButtonClick;
            SecondaryButtonClick += MyDialog_SecondaryButtonClick;

            // ��ʼ��Dialog�е��ֶΣ�ʹ�ô����NSModel���������
            NSData = nsModel;
            configName.Text = nsModel.Name;
            ipAddr.Text = nsModel.IPAddr;
            mask.Text = nsModel.Mask;
            gateway.Text = nsModel.Gateway;
            dns1.Text = nsModel.DNS1;
            dns2.Text = nsModel.DNS2;
    }
    private void MyDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
            // ��"ȷ��"��ť����¼��б����û����������
            NSData.Name=configName.Text;
            NSData.IPAddr=ipAddr.Text;
            NSData.Mask=mask.Text;
            NSData.Gateway=gateway.Text;
            NSData.DNS1=dns1.Text;
            NSData.DNS2=dns2.Text;
    }

    private void MyDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        // ��"ȡ��"��ť����¼��в����κβ���
    }
    private void InnerChanged()
    {
    }
    private void TextChanged(object sender, TextChangedEventArgs e)
    {
    }

    private void networkInterfaceName_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    // �о���������
    private void listNetworkInterface()
    {
    }
}
}
