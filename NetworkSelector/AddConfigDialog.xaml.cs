﻿// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;
using System.Net;
using Validation;
using Windows.Services.Maps;
using System.Threading.Tasks;

namespace NetworkSelector
{
    public sealed partial class AddConfigDialog : ContentDialog
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public AddConfigDialog()
        {
            this.InitializeComponent();

            string configInner = localSettings.Values["ConfigIDTemp"] as string;
            if (configInner != null)
            {
                // IPAddr.Text + "," + mask.Text + "," + gateway.Text + "," + DNS1.Text + "," + DNS2.Text + "," + configName.Text + "," + netInterface.Text;
                string[] configInnerSplit = configInner.Split(',');
                configName.Text = configInnerSplit[5];
                netInterface.Text = configInnerSplit[6];
                IPAddr.Text = configInnerSplit[0];
                mask.Text = configInnerSplit[1];
                gateway.Text = configInnerSplit[2];
                DNS1.Text = configInnerSplit[3];
                DNS2.Text = configInnerSplit[4];
            }

            if (localSettings.Values["DHCPStatus"] as string == "True")
            {
                IPAddr.IsEnabled = false;
                mask.IsEnabled = false;
                gateway.IsEnabled = false;
                DNS1.IsEnabled = false;
                DNS2.IsEnabled = false;
                configName.IsEnabled = false;
                netInterface.IsEnabled = true;
            }
            else
            {
                IPAddr.IsEnabled = true;
                mask.IsEnabled = true;
                gateway.IsEnabled = true;
                DNS1.IsEnabled = true;
                DNS2.IsEnabled = true;
                configName.IsEnabled = true;
                netInterface.IsEnabled = true;
            }
        }
        public void TextChanged(object sender, TextChangedEventArgs e)
        {
            localSettings.Values["ConfigIDTemp"] = IPAddr.Text + "," + mask.Text + "," + gateway.Text + "," + DNS1.Text + "," + DNS2.Text + "," + configName.Text + "," + netInterface.Text;
            //Test.Text = localSettings.Values["ConfigIDTemp"] as string;
        }
    }
}