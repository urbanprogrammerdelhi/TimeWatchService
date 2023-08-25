using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using TimeWatchService.TimeWatchContext;

namespace TimeWatchService
{
    public partial class MainService : ServiceBase
    {
        private Timer timeWatcherTimer = null;


        public MainService()
        {
            try
            {
              
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(ex);
                ex.SendExceptionMail();

            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                timeWatcherTimer = new Timer();
                this.timeWatcherTimer.Interval = 1000 * 60 * 1;
                this.timeWatcherTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.TimeInWatch);
                timeWatcherTimer.Enabled = true;
                LogWriter.WriteLog("Time watch Service was being started");
            }
            catch (Exception ex)
            {

                LogWriter.WriteLog(ex);
                ex.SendExceptionMail();
            }
        }
        private void TimeInWatch(object sender, ElapsedEventArgs e)
        {
            try
            {
                LogWriter.WriteLog("Invoking Insert Data method begins");
                InsertData();
            }
            catch (Exception ex)
            {

                LogWriter.WriteLog(ex);
                ex.SendExceptionMail();
            }
            finally
            {
                LogWriter.WriteLog("Invoking Insert Data method ends");

            }
        }
        private static void InsertData()
        {
            LogWriter.WriteLog("Insert Data method begins");

            try
            {
                Timewatch objUserDetails = new Timewatch();
                DataSet dsresult = new DataSet();
                XmlElement visitorData = objUserDetails.GetVisitorID();
                if (visitorData != null )
                {
                    XmlNodeReader nodereader = new XmlNodeReader(visitorData);
                    dsresult.ReadXml(nodereader, XmlReadMode.Auto);
                    if (dsresult != null && dsresult.Tables != null && dsresult.Tables.Count > 0)
                    {
                        DataTable dt = dsresult.Tables[0];
                        if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                        {
                            string cs = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
                            using (SqlConnection con = new SqlConnection(cs))
                            {
                                SqlCommand cmd = new SqlCommand("udp_InsertInTimeWatch", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                SqlParameter param = new SqlParameter();
                                param.ParameterName = "@Visitordata";
                                param.Value = dt;
                                cmd.Parameters.Add(param);
                                con.Open();
                                var recordsAffected=cmd.ExecuteNonQuery();
                                LogWriter.WriteLog($"Records affected {recordsAffected.ParseToText()}");
                                con.Close();

                            }
                        }
                        else
                        {
                            LogWriter.WriteLog("No visitor data found");

                        }
                    }
                    else
                    {
                        LogWriter.WriteLog("No visitor data found");

                    }
                }
                else
                {
                    LogWriter.WriteLog("No visitor data found");

                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog(ex);
                ex.SendExceptionMail();

            }
            finally
            {
                LogWriter.WriteLog("Insert Data method ends");

            }

        }

        protected override void OnStop()
        {
            LogWriter.WriteLog("Time watch service stopped");
        }
    }
}
