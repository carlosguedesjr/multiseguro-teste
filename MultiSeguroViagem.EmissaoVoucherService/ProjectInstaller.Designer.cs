namespace MultiSeguroViagem.EmissaoVoucherService
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
            this.serviceProcessInstallerVoucher = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerVoucher = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerVoucher
            // 
            this.serviceProcessInstallerVoucher.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstallerVoucher.Password = null;
            this.serviceProcessInstallerVoucher.Username = null;
            // 
            // serviceInstallerVoucher
            // 
            this.serviceInstallerVoucher.Description = "Serviço para emissão de voucher automático";
            this.serviceInstallerVoucher.DisplayName = "MultiSeguroViagem Emissão de Voucher";
            this.serviceInstallerVoucher.ServiceName = "MultiSeguroViagem Emissão de Voucher";
      // 
      // ProjectInstaller
      // 
      this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerVoucher,
            this.serviceInstallerVoucher});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerVoucher;
        private System.ServiceProcess.ServiceInstaller serviceInstallerVoucher;
    }
}