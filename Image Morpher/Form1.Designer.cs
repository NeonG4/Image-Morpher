namespace Image_Morpher
{
    partial class FormMorpher
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            timerTick = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // timerTick
            // 
            timerTick.Enabled = true;
            timerTick.Interval = 26;
            timerTick.Tick += timerTick_Tick;
            // 
            // FormMorpher
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(615, 352);
            DoubleBuffered = true;
            Margin = new Padding(2);
            Name = "FormMorpher";
            Text = "Image Morpher";
            FormClosing += FormMorpher_FormClosing;
            Paint += FormMorpher_Paint;
            KeyPress += FormMorpher_KeyPress;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer timerTick;
    }
}
