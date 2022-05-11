using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Microsoft.Win32;
using System;
using System.Data;
using System.Data.Sql;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BK_Restore
{
	public partial class FormLogin : XtraForm
	{
		private bool isLoadServers = false;

		public FormLogin()
		{
			InitializeComponent();
			//cboServers_DropDown(sender, e);
			GetListServer();
			txtPassword.Properties.PasswordChar = '•';
			//cboServers.SelectedIndex = 0;
		}
		public async Task GetListServer()
		{
			cboServers.Items.Clear();
			string myServer = Environment.MachineName;
			DataTable servers = null;
			var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
			var rk = localMachine.OpenSubKey("SOFTWARE\\Microsoft\\Microsoft SQL Server");
			var instances = (String[])rk.GetValue("InstalledInstances");
			//RegistryKey rk = Registry.LocalMachine.OpenSubKey
			//(@"SOFTWARE\Microsoft\Microsoft SQL Server");
			//String[] instances = (String[])rk.GetValue("InstalledInstances");
			if (instances.Length > 0)
			{
				foreach (String element in instances)
				{
					if (element == "MSSQLSERVER")
						cboServers.Items.Add(System.Environment.MachineName);
					else
						cboServers.Items.Add(System.Environment.MachineName + @"\" + element);
				}
			}
			//await Task.Run(() =>
			//{
			//	servers = SqlDataSourceEnumerator.Instance.GetDataSources();
			//});
			//for (int i = 0; i < servers.Rows.Count; i++)
			//{
			//	if (myServer == servers.Rows[i]["ServerName"].ToString()) ///// used to get the servers in the local machine////
			//	{
			//		if ((servers.Rows[i]["InstanceName"] as string) != null)
			//			cboServers.Items.Add(servers.Rows[i]["ServerName"] + "\\" + servers.Rows[i]["InstanceName"]);
			//		else
			//			cboServers.Items.Add(servers.Rows[i]["ServerName"].ToString());
			//	}
			//}

			cboServers.SelectedIndex = 0;
		}

		//public async Task GetListServer()
		//{
		//	cboServers.Items.Clear();
		//	string myServer = Environment.MachineName;
		//	DataTable servers = null;
		//	await Task.Run(() =>
		//	{
		//		servers = SqlDataSourceEnumerator.Instance.GetDataSources();
		//	});
		//	for (int i = 0; i < servers.Rows.Count; i++)
		//	{
		//		if (myServer == servers.Rows[i]["ServerName"].ToString()) ///// used to get the servers in the local machine////
		//		{
		//			if ((servers.Rows[i]["InstanceName"] as string) != null)
		//				cboServers.Items.Add(servers.Rows[i]["ServerName"] + "\\" + servers.Rows[i]["InstanceName"]);
		//			else
		//				cboServers.Items.Add(servers.Rows[i]["ServerName"].ToString());
		//		}
		//	}

		//	cboServers.SelectedIndex = 0;
		//}

		private async void cboServers_DropDown(object sender, EventArgs e)
		{
			if (isLoadServers == false)
			{
				//SplashScreenManager.ShowForm(this, typeof(WaitFormCustom), false, false, false, ParentFormState.Locked);
				await GetListServer();
				//SplashScreenManager.CloseForm(false);
				isLoadServers = true;
			}
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			Program.ServerName = cboServers.Text;
			Program.UserName = txtLogin.Text;
			Program.Password = txtPassword.Text;

			if (Program.Connect() == true)
			{
				FormMain formMain = new FormMain();
				this.Hide();
				formMain.Closed += (s, args) =>
				{
					this.Show();
				};
				formMain.Show();
			}
			else
			{
				XtraMessageBox.Show("Đăng nhập thất bại", "ERROR",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
