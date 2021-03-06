﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using Demo.InsuranceCalculation.Services;
using Demo.InsuranceCalculation.ViewControllers;
using Demo.Presentation.Dialog;
using Microsoft.Practices.Unity;

namespace Demo.InsuranceCalculation.Views
{
    /// <summary>
    /// Interaction logic for InsuranceCalculationView.xaml
    /// </summary>
    public partial class InsuranceCalculationView : UserControl
    {
        public InsuranceCalculationView(IUnityContainer container)
        {
            InitializeComponent();
            DataContext = new InsuranceCalculationViewController(container.Resolve<IInsurancePolicyAssessmentService>(), container.Resolve<IDialogService>());
        }
    }
}
