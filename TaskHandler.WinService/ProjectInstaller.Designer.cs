namespace TaskHandler
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TaskProcessorProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.TaskProcessorInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // TaskProcessorProcessInstaller
            // 
            this.TaskProcessorProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.TaskProcessorProcessInstaller.Password = null;
            this.TaskProcessorProcessInstaller.Username = null;
            // 
            // TaskProcessorInstaller
            // 
            this.TaskProcessorInstaller.Description = "Generic.Taskhandler";
            this.TaskProcessorInstaller.DisplayName = "Generic.Taskhandler";
            this.TaskProcessorInstaller.ServiceName = "Generic.Taskhandler";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.TaskProcessorInstaller,
            this.TaskProcessorProcessInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller TaskProcessorProcessInstaller;
        private System.ServiceProcess.ServiceInstaller TaskProcessorInstaller;
    }
}