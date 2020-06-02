using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using LaptopBatteryGraph.Models;
using LaptopBatteryGraph.Providers;
using System;
using System.Windows;
using System.Windows.Input;

namespace LaptopBatteryGraph.ViewModels
{
    public class ViewModel_MainWindow : ViewModelBase
    {
        #region events

        #endregion

        #region vars
        BatteryStatus _batteryStatus;
        #endregion

        #region properties
        public ViewModel_Chart Chart
        {
            get { return SimpleIoc.Default.GetInstance<ViewModel_Chart>(); }
        }

        private Model_BatteryDetails _batteryDetails = new Model_BatteryDetails();
        public Model_BatteryDetails BatteryDetails
        {
            get { return _batteryDetails; }
            set { Set(nameof(BatteryDetails), ref _batteryDetails, value); }
        }

        private Model_BatteryDetails _OldBatteryDetailValues = new Model_BatteryDetails();
        public Model_BatteryDetails OldBatteryDetailValues
        {
            get { return _OldBatteryDetailValues; }
            set { Set(nameof(OldBatteryDetailValues), ref _OldBatteryDetailValues, value); }
        }

        private DateTime _timeOnBattery = DateTime.Now;
        public DateTime TimeOnBattery
        {
            get { return _timeOnBattery; }
            set { Set(nameof(TimeOnBattery), ref _timeOnBattery, value); }
        }

        private DateTime _InititalDateTime = DateTime.Now;
        public DateTime InititalDateTime
        {
            get { return _InititalDateTime; }
            set { Set(nameof(InititalDateTime), ref _InititalDateTime, value); }
        }
        #endregion

        #region commands
        public ICommand Command_ShowAboutMe { get; set; }
        public ICommand Command_ShowBatteryInformaiton { get; set; }
        public ICommand Command_ExitApp { get; set; }
        #endregion

        #region ctors
        public ViewModel_MainWindow()
        {
            InitCommands();
        }
        #endregion

        #region command methods
        void Command_ShowAboutMe_Click()
        {
            MessageBox.Show("© Jayson Ragasa - Jara.IO - 2018");
        }

        void Command_ShowBatteryInformaiton_Click()
        {
            MessageBox.Show("Coming Soon.");
        }

        void Command_ExitApp_Click()
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region methods
        void InitCommands()
        {
            if (Command_ShowAboutMe == null) Command_ShowAboutMe = new RelayCommand(Command_ShowAboutMe_Click);
            if (Command_ShowBatteryInformaiton == null) Command_ShowBatteryInformaiton = new RelayCommand(Command_ShowBatteryInformaiton_Click);
            if (Command_ExitApp == null) Command_ExitApp = new RelayCommand(Command_ExitApp_Click);
        }

        void DesignData()
        {
            this.BatteryDetails.BatteryStatus = "Discharging";
            this.BatteryDetails.ChargeDischargeRate = 22800;
            this.BatteryDetails.DesignCapacity = 48994;
            this.BatteryDetails.FullCharge = 45858;
            this.BatteryDetails.RemainingCapacity = 33227;
            this.BatteryDetails.RemainingCapacityInPercent = 64.5;
            this.BatteryDetails.BatteryHealthInPercent = 93.5;
        }

        public void Initialize()
        {
            _batteryStatus = new BatteryStatus();
            _batteryStatus.Start();
            _batteryStatus.BatteryStatusUpdate += (s, bi) =>
            {
                this.BatteryDetails.BatteryStatus = bi.ChargeStatus;
                this.BatteryDetails.ChargeDischargeRate = bi.DischargeRate;
                this.BatteryDetails.DesignCapacity = bi.DesignedMaxCapacity;
                this.BatteryDetails.FullCharge = bi.FullChargeCapacity;
                this.BatteryDetails.RemainingCapacity = (int)bi.CurrentCapacity;
                this.BatteryDetails.RemainingCapacityInPercent = bi.CurrentCapacityPercent;
                this.BatteryDetails.BatteryHealthInPercent = bi.BatteryHealthPercent;

                Chart.DischargeRateSeries.Values.Add(this.BatteryDetails.ChargeDischargeRate);
                Chart.RemainingCapacity.Values.Add(this.BatteryDetails.RemainingCapacity);

                if(Chart.DischargeRateSeries.Values.Count > 50)
                    Chart.MaxValue = Chart.DischargeRateSeries.Values.Count;

                if(Chart.MaxValue > 200)
                {
                    Chart.DischargeRateSeries.Values.RemoveAt(0);
                    Chart.RemainingCapacity.Values.RemoveAt(0);
                }
            };
        }
        #endregion

        public override void Cleanup()
        {
            base.Cleanup();

            _batteryStatus.Stop();
        }

        //void UpdateRemainingDischargeTime()
        //{
        //    this.BatteryDetails.RemainingTime = PowerManager.RemainingDischargeTime;
        //    TimeSpan remaining = PowerManager.RemainingDischargeTime;
        //    this.BatteryDetails.RemainingTimeText = $"{remaining.Hours}H {remaining.Minutes}m {remaining.Seconds}s";
        //}
    }
}
