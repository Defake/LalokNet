using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatabaseWorker.Controllers;
using VK_API;

namespace DatabaseUpdater
{
	public partial class Form1 : Form
	{
		private readonly VkDatabaseUpdater _dbUpdater;

		private ListViewItem _selectedGroup;

		public Form1()
		{
			_dbUpdater = new VkDatabaseUpdater(LogMessage);
			
			InitializeComponent();
			LogMessage("Here will being displayed log information");

			using (var groupController = new GroupController())
				foreach (var group in groupController.GetAllEntities())
					GroupList.Items.Add(group.StringId);
		}

		private void ButtonStart_Click(object sender, EventArgs e)
		{
			_dbUpdater.Start();
			LogMessage("Database updating started");
		}

		private void ButtonStop_Click(object sender, EventArgs e)
		{
			_dbUpdater.Stop();
			LogMessage("Sended request to stop database updating...");
			
		}

		delegate void LogCallback(string text);

		public void LogMessage(string text)
		{ 
			if (LogTextBox.InvokeRequired)
			{
				LogCallback d = LogMessage;
				Invoke(d, text);
			}
			else
			{
				LogTextBox.AppendText(text + "\n");
				LogTextBox.ScrollToCaret();
			}
		}

		private void ButtonGroupAdd_Click(object sender, EventArgs e)
		{
			if (TextBoxGroup.Text == "")
				return;

			if (GroupList.Items.Cast<ListViewItem>().Any(groupListItem => groupListItem.Text == TextBoxGroup.Text))
				return;

			GroupList.Items.Add(TextBoxGroup.Text);

			using (var cont = new GroupController())
				cont.AddOrUpdateEntity(new VkClient(new VkApiInteraction(), new VkToDbMapper()).GetGroup(TextBoxGroup.Text));

			LogMessage("Group added. Relaunch DB updater or wait for the next update cycle");

			TextBoxGroup.Text = "";
		}

		private void GroupList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			if (e.IsSelected)
				_selectedGroup = e.Item;
		}

		private void ButtonDeleteGroup_Click(object sender, EventArgs e)
		{
			if (_selectedGroup != null)
			{
				using (var gc = new GroupController())
					gc.Delete(gc.GetGroupByStringId(_selectedGroup.Text).VkId);

				GroupList.Items.Remove(_selectedGroup);
				_selectedGroup = null;
			}
		}
	}
}
