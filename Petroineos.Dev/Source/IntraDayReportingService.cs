using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.Unity;
using Petroineos.Dev.Model;
using Services;
using Timer = System.Timers.Timer;



namespace Petroineos.Dev
{
    public partial class IntraDayReportingService : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Timer _timer;
        readonly string _timerDurationinMinutes = ConfigurationManager.AppSettings["TimeBetweenRunsInMin"];
        private new IUnityContainer Container { get; set; }
        public IntraDayReportingService()
        {
            InitializeComponent();
            RegisterTypes();
        }

        private void RegisterTypes()
        {
            Container = new UnityContainer();
            Container.RegisterType<IReportOutputGenerator, ReportOutputGenerator>();
            Container.RegisterType<IReportController, ReportController>();
            Container.RegisterType<IPowerService, PowerService>();
            Container.RegisterType<IPowerPeriodCalculator, PowerPeriodCalculator>();
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("Starting IntraDay Reporting service");
            // This can be refactored where we can create an interface for timer to test invoke of this method
            _timer = new Timer { Interval = TimeSpan.FromMinutes( Convert.ToDouble(_timerDurationinMinutes)).TotalMilliseconds };
            _timer.Elapsed += _timer_Elapsed;
            //An extract must run when the service first starts and then run at the interval specified as spec
            _timer_Elapsed(null,null);
            _timer.Start();

        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var instance = Container.Resolve<IReportController>();

            Task.Factory.StartNew(() =>
            {
                instance.Run();
            }).ContinueWith((t) =>
            {
                if (!t.IsFaulted) return;

                t.Exception?.Handle(exception =>
                {
                    Log.Error($"Error while {exception.Message}");
                    return true;
                }
                    );
            });
        }


        protected override void OnStop()
        {
            Log.Info("Stopping IntraDay Reporting service");
            _timer.Enabled = false;
            _timer = null;
            Log.Info("IntraDay Reporting service Stopped");
        }

        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
