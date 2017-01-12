namespace DatabaseUpdater
{
	partial class Form1
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.ButtonStart = new System.Windows.Forms.Button();
			this.ButtonStop = new System.Windows.Forms.Button();
			this.GroupList = new System.Windows.Forms.ListView();
			this.TextBoxGroup = new System.Windows.Forms.TextBox();
			this.LogTextBox = new System.Windows.Forms.RichTextBox();
			this.ButtonGroupAdd = new System.Windows.Forms.Button();
			this.ButtonDeleteGroup = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ButtonStart
			// 
			this.ButtonStart.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ButtonStart.Location = new System.Drawing.Point(58, 12);
			this.ButtonStart.Name = "ButtonStart";
			this.ButtonStart.Size = new System.Drawing.Size(117, 39);
			this.ButtonStart.TabIndex = 0;
			this.ButtonStart.Text = "Start Updater";
			this.ButtonStart.UseVisualStyleBackColor = true;
			this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
			// 
			// ButtonStop
			// 
			this.ButtonStop.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ButtonStop.Location = new System.Drawing.Point(58, 75);
			this.ButtonStop.Name = "ButtonStop";
			this.ButtonStop.Size = new System.Drawing.Size(117, 39);
			this.ButtonStop.TabIndex = 1;
			this.ButtonStop.Text = "Stop Updater";
			this.ButtonStop.UseVisualStyleBackColor = true;
			this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
			// 
			// GroupList
			// 
			this.GroupList.Location = new System.Drawing.Point(231, 9);
			this.GroupList.Name = "GroupList";
			this.GroupList.Size = new System.Drawing.Size(121, 108);
			this.GroupList.TabIndex = 2;
			this.GroupList.UseCompatibleStateImageBehavior = false;
			this.GroupList.View = System.Windows.Forms.View.SmallIcon;
			this.GroupList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.GroupList_ItemSelectionChanged);
			// 
			// TextBoxGroup
			// 
			this.TextBoxGroup.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.TextBoxGroup.Location = new System.Drawing.Point(358, 9);
			this.TextBoxGroup.Name = "TextBoxGroup";
			this.TextBoxGroup.Size = new System.Drawing.Size(118, 22);
			this.TextBoxGroup.TabIndex = 3;
			// 
			// LogTextBox
			// 
			this.LogTextBox.Location = new System.Drawing.Point(12, 136);
			this.LogTextBox.Name = "LogTextBox";
			this.LogTextBox.ReadOnly = true;
			this.LogTextBox.Size = new System.Drawing.Size(465, 202);
			this.LogTextBox.TabIndex = 4;
			this.LogTextBox.Text = "";
			// 
			// ButtonGroupAdd
			// 
			this.ButtonGroupAdd.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ButtonGroupAdd.Location = new System.Drawing.Point(358, 37);
			this.ButtonGroupAdd.Name = "ButtonGroupAdd";
			this.ButtonGroupAdd.Size = new System.Drawing.Size(118, 33);
			this.ButtonGroupAdd.TabIndex = 5;
			this.ButtonGroupAdd.Text = "Add a new group";
			this.ButtonGroupAdd.UseVisualStyleBackColor = true;
			this.ButtonGroupAdd.Click += new System.EventHandler(this.ButtonGroupAdd_Click);
			// 
			// ButtonDeleteGroup
			// 
			this.ButtonDeleteGroup.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ButtonDeleteGroup.Location = new System.Drawing.Point(358, 84);
			this.ButtonDeleteGroup.Name = "ButtonDeleteGroup";
			this.ButtonDeleteGroup.Size = new System.Drawing.Size(118, 33);
			this.ButtonDeleteGroup.TabIndex = 6;
			this.ButtonDeleteGroup.Text = "Delete group";
			this.ButtonDeleteGroup.UseVisualStyleBackColor = true;
			this.ButtonDeleteGroup.Click += new System.EventHandler(this.ButtonDeleteGroup_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(489, 350);
			this.Controls.Add(this.ButtonDeleteGroup);
			this.Controls.Add(this.ButtonGroupAdd);
			this.Controls.Add(this.LogTextBox);
			this.Controls.Add(this.TextBoxGroup);
			this.Controls.Add(this.GroupList);
			this.Controls.Add(this.ButtonStop);
			this.Controls.Add(this.ButtonStart);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ButtonStart;
		private System.Windows.Forms.Button ButtonStop;
		private System.Windows.Forms.Button ButtonGroupAdd;
		private System.Windows.Forms.RichTextBox LogTextBox;
		private System.Windows.Forms.TextBox TextBoxGroup;
		private System.Windows.Forms.ListView GroupList;
		private System.Windows.Forms.Button ButtonDeleteGroup;
	}
}

