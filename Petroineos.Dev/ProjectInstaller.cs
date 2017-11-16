using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Petroineos.Dev
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
