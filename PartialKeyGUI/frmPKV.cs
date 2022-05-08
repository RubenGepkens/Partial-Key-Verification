using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PartialKeyVerification;
using PartialKeyVerification.Checksum;
using PartialKeyVerification.Hash;

namespace PartialKeyGUI
{
    public partial class frmPKV : Form
    {
        public frmPKV()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var generator = new PartialKeyGenerator(new Adler16(), new Jenkins96(), new uint[] { 1 }) { Spacing = 5 };
            var key = generator.Generate("bob@smith.com");

            Console.WriteLine("{0}", key.ToString());

            var isValid = PartialKeyValidator.ValidateKey(new Adler16(), new Jenkins96(), key, 0, 1);

            Console.WriteLine("{0}", isValid);
        }
    }
}
