namespace Mp3Renamer
{
    partial class Mp3RenamerForm
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripSeparator sepMenuItem;
            System.Windows.Forms.ToolStripMenuItem patternMenuItem;
            System.Windows.Forms.ToolStripMenuItem convertCaseMenuItem;
            System.Windows.Forms.ToolStripMenuItem fileMenuItem;
            System.Windows.Forms.ToolStripMenuItem optionsMenuItem;
            System.Windows.Forms.ToolStripSeparator sep2MenuItem;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mp3RenamerForm));
            this.patternAutomaticMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patternDashMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patternFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patternArtistAlbumFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noChangeCaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uppercaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstLetterUpperMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sentenceCaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowerCaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFilesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.includeSubfoldersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intelligentConversionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTrackNumberMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertSelectionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grid = new System.Windows.Forms.DataGridView();
            this.status = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.addFilesLink = new System.Windows.Forms.LinkLabel();
            sepMenuItem = new System.Windows.Forms.ToolStripSeparator();
            patternMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            convertCaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            optionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sep2MenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.status.SuspendLayout();
            this.SuspendLayout();
            // 
            // sepMenuItem
            // 
            sepMenuItem.Name = "sepMenuItem";
            sepMenuItem.Size = new System.Drawing.Size(270, 6);
            // 
            // patternMenuItem
            // 
            patternMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.patternAutomaticMenuItem,
            this.patternDashMenuItem,
            this.patternFolderToolStripMenuItem,
            this.patternArtistAlbumFolderMenuItem});
            patternMenuItem.Name = "patternMenuItem";
            patternMenuItem.Size = new System.Drawing.Size(223, 22);
            patternMenuItem.Text = "&Patrón";
            // 
            // patternAutomaticMenuItem
            // 
            this.patternAutomaticMenuItem.Checked = true;
            this.patternAutomaticMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.patternAutomaticMenuItem.Name = "patternAutomaticMenuItem";
            this.patternAutomaticMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.patternAutomaticMenuItem.Size = new System.Drawing.Size(274, 22);
            this.patternAutomaticMenuItem.Text = "&Automático";
            // 
            // patternDashMenuItem
            // 
            this.patternDashMenuItem.Name = "patternDashMenuItem";
            this.patternDashMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.patternDashMenuItem.Size = new System.Drawing.Size(274, 22);
            this.patternDashMenuItem.Text = "Ar&tista - Título";
            this.patternDashMenuItem.Click += new System.EventHandler(this.OnPatternMenuItemClick);
            // 
            // patternFolderToolStripMenuItem
            // 
            this.patternFolderToolStripMenuItem.Name = "patternFolderToolStripMenuItem";
            this.patternFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.patternFolderToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.patternFolderToolStripMenuItem.Text = "Ál&bum desde carpeta";
            this.patternFolderToolStripMenuItem.Click += new System.EventHandler(this.OnPatternMenuItemClick);
            // 
            // patternArtistAlbumFolderMenuItem
            // 
            this.patternArtistAlbumFolderMenuItem.Name = "patternArtistAlbumFolderMenuItem";
            this.patternArtistAlbumFolderMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.patternArtistAlbumFolderMenuItem.Size = new System.Drawing.Size(274, 22);
            this.patternArtistAlbumFolderMenuItem.Text = "A&rtista + Álbum desde carpeta";
            this.patternArtistAlbumFolderMenuItem.Click += new System.EventHandler(this.OnPatternMenuItemClick);
            // 
            // convertCaseMenuItem
            // 
            convertCaseMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noChangeCaseMenuItem,
            this.uppercaseMenuItem,
            this.firstLetterUpperMenuItem,
            this.sentenceCaseMenuItem,
            this.lowerCaseMenuItem});
            convertCaseMenuItem.Name = "convertCaseMenuItem";
            convertCaseMenuItem.Size = new System.Drawing.Size(223, 22);
            convertCaseMenuItem.Text = "&Convertir";
            // 
            // noChangeCaseMenuItem
            // 
            this.noChangeCaseMenuItem.Checked = true;
            this.noChangeCaseMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noChangeCaseMenuItem.Name = "noChangeCaseMenuItem";
            this.noChangeCaseMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D0)));
            this.noChangeCaseMenuItem.Size = new System.Drawing.Size(230, 22);
            this.noChangeCaseMenuItem.Text = "&Ninguno";
            this.noChangeCaseMenuItem.Click += new System.EventHandler(this.OnRenameCellsMenuItemClick);
            // 
            // uppercaseMenuItem
            // 
            this.uppercaseMenuItem.Name = "uppercaseMenuItem";
            this.uppercaseMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.uppercaseMenuItem.Size = new System.Drawing.Size(230, 22);
            this.uppercaseMenuItem.Text = "&Mayúsculas";
            this.uppercaseMenuItem.Click += new System.EventHandler(this.OnRenameCellsMenuItemClick);
            // 
            // firstLetterUpperMenuItem
            // 
            this.firstLetterUpperMenuItem.Name = "firstLetterUpperMenuItem";
            this.firstLetterUpperMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.firstLetterUpperMenuItem.Size = new System.Drawing.Size(230, 22);
            this.firstLetterUpperMenuItem.Text = "Oración (&primera letra)";
            this.firstLetterUpperMenuItem.Click += new System.EventHandler(this.OnRenameCellsMenuItemClick);
            // 
            // sentenceCaseMenuItem
            // 
            this.sentenceCaseMenuItem.Name = "sentenceCaseMenuItem";
            this.sentenceCaseMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D3)));
            this.sentenceCaseMenuItem.Size = new System.Drawing.Size(230, 22);
            this.sentenceCaseMenuItem.Text = "&Oración";
            this.sentenceCaseMenuItem.Click += new System.EventHandler(this.OnRenameCellsMenuItemClick);
            // 
            // lowerCaseMenuItem
            // 
            this.lowerCaseMenuItem.Name = "lowerCaseMenuItem";
            this.lowerCaseMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D4)));
            this.lowerCaseMenuItem.Size = new System.Drawing.Size(230, 22);
            this.lowerCaseMenuItem.Text = "M&inúsculas";
            this.lowerCaseMenuItem.Click += new System.EventHandler(this.OnRenameCellsMenuItemClick);
            // 
            // fileMenuItem
            // 
            fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectFolderMenuItem,
            this.selectFilesMenuItem,
            this.saveMenuItem,
            sepMenuItem,
            this.exitMenuItem});
            fileMenuItem.Name = "fileMenuItem";
            fileMenuItem.Size = new System.Drawing.Size(60, 20);
            fileMenuItem.Text = "&Archivo";
            // 
            // selectFolderMenuItem
            // 
            this.selectFolderMenuItem.Image = global::Mp3Renamer.Properties.Resources.add_folder;
            this.selectFolderMenuItem.Name = "selectFolderMenuItem";
            this.selectFolderMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.selectFolderMenuItem.Size = new System.Drawing.Size(273, 22);
            this.selectFolderMenuItem.Text = "Seleccionar &carpeta...";
            this.selectFolderMenuItem.Click += new System.EventHandler(this.OnSelectFolderMenuItemClick);
            // 
            // selectFilesMenuItem
            // 
            this.selectFilesMenuItem.Image = global::Mp3Renamer.Properties.Resources.add_files;
            this.selectFilesMenuItem.Name = "selectFilesMenuItem";
            this.selectFilesMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectFilesMenuItem.Size = new System.Drawing.Size(273, 22);
            this.selectFilesMenuItem.Text = "Seleccionar &archivos...";
            this.selectFilesMenuItem.Click += new System.EventHandler(this.OnSelectFilesMenuItemClick);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Image = global::Mp3Renamer.Properties.Resources.save;
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.saveMenuItem.Size = new System.Drawing.Size(273, 22);
            this.saveMenuItem.Text = "&Aplicar cambios";
            this.saveMenuItem.Click += new System.EventHandler(this.OnSaveMenuItemClick);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Image = global::Mp3Renamer.Properties.Resources.exit;
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(273, 22);
            this.exitMenuItem.Text = "&Salir";
            this.exitMenuItem.Click += new System.EventHandler(this.OnExitMenuItemClick);
            // 
            // optionsMenuItem
            // 
            optionsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.includeSubfoldersMenuItem,
            this.intelligentConversionMenuItem,
            this.removeTrackNumberMenuItem,
            patternMenuItem,
            convertCaseMenuItem});
            optionsMenuItem.Name = "optionsMenuItem";
            optionsMenuItem.Size = new System.Drawing.Size(69, 20);
            optionsMenuItem.Text = "&Opciones";
            // 
            // includeSubfoldersMenuItem
            // 
            this.includeSubfoldersMenuItem.Checked = true;
            this.includeSubfoldersMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeSubfoldersMenuItem.Name = "includeSubfoldersMenuItem";
            this.includeSubfoldersMenuItem.Size = new System.Drawing.Size(223, 22);
            this.includeSubfoldersMenuItem.Text = "Incluir &subdirectorios";
            this.includeSubfoldersMenuItem.Click += new System.EventHandler(this.OnIncludeSubfoldersMenuItemClick);
            // 
            // intelligentConversionMenuItem
            // 
            this.intelligentConversionMenuItem.Checked = true;
            this.intelligentConversionMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.intelligentConversionMenuItem.Name = "intelligentConversionMenuItem";
            this.intelligentConversionMenuItem.Size = new System.Drawing.Size(223, 22);
            this.intelligentConversionMenuItem.Text = "Conversión &Inteligente";
            this.intelligentConversionMenuItem.Click += new System.EventHandler(this.OnIntelligentConversionMenuItemClick);
            // 
            // removeTrackNumberMenuItem
            // 
            this.removeTrackNumberMenuItem.Checked = true;
            this.removeTrackNumberMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.removeTrackNumberMenuItem.Name = "removeTrackNumberMenuItem";
            this.removeTrackNumberMenuItem.Size = new System.Drawing.Size(223, 22);
            this.removeTrackNumberMenuItem.Text = "Eliminar número de canción";
            this.removeTrackNumberMenuItem.Click += new System.EventHandler(this.OnRemoveTrackNumberMenuItemClick);
            // 
            // sep2MenuItem
            // 
            sep2MenuItem.Name = "sep2MenuItem";
            sep2MenuItem.Size = new System.Drawing.Size(199, 6);
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            fileMenuItem,
            this.editMenuItem,
            optionsMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(757, 24);
            this.menu.TabIndex = 0;
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyMenuItem,
            this.pasteMenuItem,
            this.cleanListMenuItem,
            sep2MenuItem,
            this.selectAllMenuItem,
            this.invertSelectionMenuItem});
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(58, 20);
            this.editMenuItem.Text = "&Edición";
            // 
            // copyMenuItem
            // 
            this.copyMenuItem.Image = global::Mp3Renamer.Properties.Resources.copy;
            this.copyMenuItem.Name = "copyMenuItem";
            this.copyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyMenuItem.ShowShortcutKeys = false;
            this.copyMenuItem.Size = new System.Drawing.Size(202, 22);
            this.copyMenuItem.Text = "Cop&iar";
            this.copyMenuItem.Click += new System.EventHandler(this.OnCopyMenuItemClick);
            // 
            // pasteMenuItem
            // 
            this.pasteMenuItem.Image = global::Mp3Renamer.Properties.Resources.paste;
            this.pasteMenuItem.Name = "pasteMenuItem";
            this.pasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteMenuItem.ShowShortcutKeys = false;
            this.pasteMenuItem.Size = new System.Drawing.Size(202, 22);
            this.pasteMenuItem.Text = "&Pegar";
            this.pasteMenuItem.Click += new System.EventHandler(this.OnPasteMenuItemClick);
            // 
            // cleanListMenuItem
            // 
            this.cleanListMenuItem.Image = global::Mp3Renamer.Properties.Resources.clear;
            this.cleanListMenuItem.Name = "cleanListMenuItem";
            this.cleanListMenuItem.Size = new System.Drawing.Size(202, 22);
            this.cleanListMenuItem.Text = "&Limpiar lista";
            this.cleanListMenuItem.Click += new System.EventHandler(this.OnCleanListMenuItemClick);
            // 
            // selectAllMenuItem
            // 
            this.selectAllMenuItem.Name = "selectAllMenuItem";
            this.selectAllMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.selectAllMenuItem.Size = new System.Drawing.Size(202, 22);
            this.selectAllMenuItem.Text = "S&eleccionar todo";
            this.selectAllMenuItem.Click += new System.EventHandler(this.OnSelectAllMenuItemClick);
            // 
            // invertSelectionMenuItem
            // 
            this.invertSelectionMenuItem.Name = "invertSelectionMenuItem";
            this.invertSelectionMenuItem.Size = new System.Drawing.Size(202, 22);
            this.invertSelectionMenuItem.Text = "In&vertir selección";
            this.invertSelectionMenuItem.Click += new System.EventHandler(this.OnInvertSelectionMenuItemClick);
            // 
            // grid
            // 
            this.grid.AllowDrop = true;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grid.BackgroundColor = System.Drawing.Color.White;
            this.grid.Location = new System.Drawing.Point(12, 27);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.Size = new System.Drawing.Size(733, 407);
            this.grid.TabIndex = 1;
            this.grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnGridKeyDown);
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.status.Location = new System.Drawing.Point(0, 437);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(757, 22);
            this.status.TabIndex = 2;
            this.status.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(16, 17);
            this.statusLabel.Text = "   ";
            // 
            // addFilesLink
            // 
            this.addFilesLink.AllowDrop = true;
            this.addFilesLink.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addFilesLink.BackColor = System.Drawing.Color.White;
            this.addFilesLink.Location = new System.Drawing.Point(12, 27);
            this.addFilesLink.Name = "addFilesLink";
            this.addFilesLink.Size = new System.Drawing.Size(733, 407);
            this.addFilesLink.TabIndex = 1;
            this.addFilesLink.TabStop = true;
            this.addFilesLink.Text = "Arrastre archivos o carpeta a esta zona o haga click aquí para añadir archvos";
            this.addFilesLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.addFilesLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnAddFilesLinkClicked);
            // 
            // Mp3RenamerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 459);
            this.Controls.Add(this.addFilesLink);
            this.Controls.Add(this.status);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.Name = "Mp3RenamerForm";
            this.Text = "Mp3 Renamer";
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem selectFolderMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectFilesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem includeSubfoldersMenuItem;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.LinkLabel addFilesLink;
        private System.Windows.Forms.ToolStripMenuItem uppercaseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem firstLetterUpperMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sentenceCaseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowerCaseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patternDashMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patternFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertSelectionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patternArtistAlbumFolderMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patternAutomaticMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noChangeCaseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cleanListMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intelligentConversionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTrackNumberMenuItem;
    }
}

